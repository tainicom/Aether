#region License
//   Copyright 2015 Kastellanos Nikolaos
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
using tainicom.Aether.Elementary;
using tainicom.Aether.Engine;
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary.Photons;
using tainicom.Aether.Elementary.Cameras;
using tainicom.Aether.Core.Cameras;
using tainicom.Aether.Elementary.Leptons;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Core.Materials;

namespace tainicom.Aether.Core.Walkers
{
    public class WireframePhotonsWalker : DepthFirstWalker, IInitializable, ILepton, IRenderable, IShaderMatrices
    {
        private AetherEngine engine;
        BasicMaterial material;
        
        public Matrix Projection { get; set; }
        public Matrix View { get; set; }
        public Matrix World { get; set; }

        public WireframePhotonsWalker(): base(null)
        {
        }

        void IInitializable.Initialize(AetherEngine engine)
        {
            this.engine = engine;
            this.Manager = engine.PhotonsMgr;
            this.startingElement = engine.PhotonsMgr.Root;
            
            //create material
            material = new BasicMaterial();
            material.Initialize(engine);
            material.RasterizerState = new RasterizerState() { FillMode = FillMode.WireFrame };
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override bool MoveNext()
        {
            return base.MoveNext();
        }

        public void Render(GameTime gameTime)
        {
            Render(gameTime, Current);
        }

        public void Render(GameTime gameTime, IAetherWalker walker)
        {
            throw new NotSupportedException("Can'not assign walker on a walker");
        }

        private void Render(GameTime gameTime, IAether particle)
        {
            ICamera camera = Camera.Current;

            IDrawable drawable = particle as IDrawable;
            IPhoton photon = particle as IPhoton;
            ILepton lepton = particle as ILepton;

            if (photon !=null && photon.Material is MaterialBase)
                this.material.PrimitiveType = ((MaterialBase)photon.Material).PrimitiveType;

            if (photon != null && lepton != null && photon.Material != null && (drawable == null || drawable.Visible))
            {
                ((IShaderMatrices)material).World = lepton.LocalTransform;
                ((IShaderMatrices)material).View = this.View;
                ((IShaderMatrices)material).Projection = this.Projection;
                material.Apply();
                material.ApplyTextures(photon.Textures);
                photon.Accept(material);
                return;
            }            
            
            return;
        }

        public Matrix LocalTransform { get { return Matrix.Identity; } }

        public Quaternion Rotation
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Vector3 Scale
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Vector3 Position
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

    }
}
