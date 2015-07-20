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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Leptons;
using tainicom.Aether.Elementary.Photons;
using tainicom.Aether.Engine;
using tainicom.Aether.Core.Managers;

namespace tainicom.Aether.Core.Walkers
{
    public class PhotonsWalker : DepthFirstWalker, IInitializable, ILepton, IRenderable, IShaderMatrices
    {
        private AetherEngine engine;
        PhotonPlasma PhotonRoot { get { return (PhotonPlasma)engine.PhotonsMgr.Root;  } }
        
        public Matrix Projection { get; set; }
        public Matrix View { get; set; }
        public Matrix World { get; set; }

        public PhotonsWalker(): base(null)
        {
        }

        void IInitializable.Initialize(AetherEngine engine)
        {
            this.engine = engine;
            this.Manager = engine.PhotonsMgr;
            this.startingElement = engine.PhotonsMgr.Root;
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override bool MoveNext()
        {
            return base.MoveNext();
        }

        protected override IList<IAether> GetParticles(IPlasma plasma)
        {
            IPhotonPlasma photonPlasma = plasma as IPhotonPlasma;
            if (photonPlasma != null)
                return photonPlasma.VisibleParticles;
            return plasma;
        }

        public void Render(GameTime gameTime)
        {
            try 
            { 
            Render(gameTime, Current);
        }
            catch(Exception e) { throw e; }

            return;
        }

        public void Render(GameTime gameTime, IAetherWalker walker)
        {
            throw new NotSupportedException("Can'not assign walker on a walker");
        }

        private void Render(GameTime gameTime, IAether particle)
        {
            Matrix worldTransform;
            LeptonsManager.GetWorldTransform(particle, out worldTransform);

            IDrawable drawable = particle as IDrawable;
            IPhoton photon = particle as IPhoton;
            ILepton lepton = particle as ILepton;
            IMaterial material = (photon != null) ? photon.Material : null;

            if (photon != null && lepton != null && material != null && (drawable==null || drawable.Visible))
            {
                ((IShaderMatrices)material).World = worldTransform;
                ((IShaderMatrices)material).View = this.View;
                ((IShaderMatrices)material).Projection = this.Projection;
                material.Apply();
                material.ApplyTextures(photon.Textures);
                photon.Accept(material);
                return;
            }
            else if (drawable != null)
            {
                //drawable.Draw(gameTime);
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
