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
using tainicom.Aether.Core.ECS;
using tainicom.Aether.Core.Managers;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Leptons;
using tainicom.Aether.Elementary.Photons;
using tainicom.Aether.Engine;

namespace tainicom.Aether.Core.Walkers
{
    public class PhotonsWalker : BaseWalker<IPhotonNode>, IPhotonWalker, IInitializable, ILepton
    {
        protected IPhotonNode startingElement;

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
        PhotonPlasma PhotonRoot { get { return (PhotonPlasma)engine.PhotonsMgr.Root;  } }
        
        public Matrix Projection { get; set; }
        public Matrix View { get; set; }

        public PhotonsWalker(): base()
        {
            this.startingElement = null;
            BreadcrumbQueue = new Queue<Breadcrumb>(16);
        }

        void IInitializable.Initialize(AetherEngine engine)
        {
            this.engine = engine;
            this.Manager = engine.PhotonsMgr;
            this.startingElement = (IPhotonNode)engine.PhotonsMgr.Root;
        }

        public override void Reset()
        {
            currentNode.Enumerator = null;
            Current = default(IPhotonNode);
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
                var enumerator = GetParticles((IPlasma<IPhotonNode>)Current);
                currentNode = new Breadcrumb(enumerator);
                return true;
            }

            if (currentNode.Enumerator.MoveNext())
            {
                Current = (IPhotonNode)currentNode.Enumerator.Current;

                var plasma = Current as IPlasma<IPhotonNode>;
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

        private IEnumerator<IPhotonNode> GetParticles(IPlasma<IPhotonNode> plasma)
        {
            IPhotonPlasma photonPlasma = plasma as IPhotonPlasma;
            if (photonPlasma != null)
                return photonPlasma.VisibleParticles;
            return plasma.GetEnumerator();
        }

        public void Render(GameTime gameTime, IPhotonNode photonNode)
        {
            IPhoton photon = photonNode as IPhoton;
            if (photon == null) return;

            IMaterial material = photon.Material;
            if (material == null) return;

            var component = photonNode as Component;
            if (component != null)
            {
                foreach (var leptonNode in engine.Entities.GetEntityComponents<ILeptonNode>(component))
                {
                    Matrix worldTransform;
                    LeptonsManager.GetWorldTransform(leptonNode, out worldTransform);
                    RenderPhoton(photon, material, ref worldTransform);
                }
            }
            else
            {
                Matrix worldTransform;
                LeptonsManager.GetWorldTransform(photon, out worldTransform);
                RenderPhoton(photon, material, ref worldTransform);
            }
        }

        void RenderPhoton(IPhoton photon, IMaterial material, ref Matrix worldTransform)
        {
                ((IShaderMatrices)material).Projection = this.Projection;
                ((IShaderMatrices)material).View = this.View;
                ((IShaderMatrices)material).World = worldTransform;
                material.Apply();
                material.ApplyTextures(photon.Textures);
                photon.Accept(material);
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
