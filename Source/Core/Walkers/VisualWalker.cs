#region License
//   Copyright 2015-2018 Kastellanos Nikolaos
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using tainicom.Aether.Core.Components;
using tainicom.Aether.Core.Managers;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Spatial;
using tainicom.Aether.Elementary.Visual;
using tainicom.Aether.Engine;

namespace tainicom.Aether.Core.Walkers
{
    public class VisualWalker : BaseWalker<IVisualNode>, IVisualWalker, IInitializable, ISpatial
    {
        protected IVisualNode startingElement;

        protected struct Breadcrumb
        {
            public IEnumerator Enumerator;

            public Breadcrumb(IEnumerator Enumerator)
            {
                this.Enumerator = Enumerator;
            }
        }

        protected Breadcrumb currentNode;
        protected Queue<Breadcrumb> BreadcrumbQueue;

        private AetherEngine engine;
        VisualCollection VisualRoot { get { return (VisualCollection)engine.VisualMgr.Root;  } }
        
        public Matrix Projection { get; set; }
        public Matrix View { get; set; }

        public VisualWalker(): base()
        {
            this.startingElement = null;
            BreadcrumbQueue = new Queue<Breadcrumb>(16);
        }

        void IInitializable.Initialize(AetherEngine engine)
        {
            this.engine = engine;
            this.Manager = engine.VisualMgr;
            this.startingElement = (IVisualNode)engine.VisualMgr.Root;
        }

        public override void Reset()
        {
            currentNode.Enumerator = null;
            Current = default(IVisualNode);
        }

        public override bool MoveNext()
        {
            return internalMoveNext();
        }
        
        //this method is used to break Recursive through the Super class when DepthFirstWalker is inherited
        private bool internalMoveNext()
        {
            if (currentNode.Enumerator == null)
            {
                Current = startingElement;
                BreadcrumbQueue.Clear();
                var enumerator = GetParticles((IPlasma<IVisualNode>)Current);
                currentNode = new Breadcrumb(enumerator);
                return true;
            }

            if (currentNode.Enumerator.MoveNext())
            {
                Current = (IVisualNode)currentNode.Enumerator.Current;

                var plasma = Current as IPlasma<IVisualNode>;
                if (plasma != null)
                {
                    BreadcrumbQueue.Enqueue(currentNode);
                    var enumerator = GetParticles(plasma);
                    currentNode = new Breadcrumb(enumerator);
                }
                return true;
            }
            else
            {
                if (BreadcrumbQueue.Count > 0)
                {
                    currentNode = BreadcrumbQueue.Dequeue();
                    internalMoveNext(); //MoveNext();
                    return true;
                }
            }

            return false;
        }

        private IEnumerator<IVisualNode> GetParticles(IPlasma<IVisualNode> plasma)
        {
            IVisualPlasma visualNodes = plasma as IVisualPlasma;
            if (visualNodes != null)
                return visualNodes.VisibleParticles;
            return plasma.GetEnumerator();
        }

        public void Render(GameTime gameTime, IVisualNode visualNode)
        {
            IVisual visual = visualNode as IVisual;
            if (visual == null) return;

            IMaterial material = visual.Material;
            if (material == null) return;

            var component = visualNode as Component;
            if (component != null)
            {
                foreach (var spatialNode in Component.GetEntityComponents<ISpatialNode>(component))
                {
                    Matrix worldTransform;
                    SpatialManager.GetWorldTransform(spatialNode, out worldTransform);
                    RenderVisual(visual, material, ref worldTransform);
                }
            }
            else
            {
                Matrix worldTransform;
                SpatialManager.GetWorldTransform(visual, out worldTransform);
                RenderVisual(visual, material, ref worldTransform);
            }
        }

        void RenderVisual(IVisual visual, IMaterial material, ref Matrix worldTransform)
        {
            ((IShaderMatrices)material).Projection = this.Projection;
            ((IShaderMatrices)material).View = this.View;
            ((IShaderMatrices)material).World = worldTransform;
            material.Apply();
            material.ApplyTextures(visual.Textures);
            visual.Accept(material);
        }

        public Matrix LocalTransform { get { return Matrix.Identity; } }

        public Quaternion Rotation
        {
            get { return Quaternion.Identity; }
            set { }
        }

        public Vector3 Scale
        {
            get { return Vector3.One; }
            set { }
        }

        public Vector3 Position
        {
            get { return Vector3.Zero; }
            set {  }
        }

    }
}
