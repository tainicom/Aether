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
using System.Text;
using System.Runtime.Serialization;
using tainicom.Aether.Engine;
using tainicom.Aether.Core.Managers;
using tainicom.Aether.Elementary.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Elementary.Managers;
using tainicom.Aether.Elementary;
using tainicom.Aether.Core.Walkers;

//namespace tainicom.Aether.Engine
namespace tainicom.Aether.Engine
{
    public partial class AetherEngine
    {
        #region Aether Fields

        private IList<IAetherManager> Managers { get; set; }
        
        //Managers
        public LeptonsManager LeptonsMgr;
        public GluonsManager GluonsMgr;
        public PhotonsManager PhotonsMgr;
        public CamerasManager CamerasMgr;
        public MaterialsManager MaterialsMgr;
        
        #endregion
        
        protected virtual void CreateManagers()
        {
            LeptonsMgr = new LeptonsManager(this, _context, "Leptons");
            GluonsMgr = new GluonsManager(this, _context, "Gluons");
            CamerasMgr = new CamerasManager(this, _context, "Cameras");
            MaterialsMgr = new MaterialsManager(this, _context, "Materials");
            PhotonsMgr = new PhotonsManager(this, _context, "Photons");

            AddManager(LeptonsMgr);
            AddManager(GluonsMgr);
            AddManager(PhotonsMgr);
            AddManager(CamerasMgr);
            AddManager(MaterialsMgr);

            foreach (IAetherManager particleManager in Managers)
            {
                if (particleManager is IInitializable)
                    ((IInitializable)particleManager).Initialize(this);
            }

            return;
        }

        protected void AddManager(IAetherManager manager)
        {
            Managers.Add(manager);
        }

        private void TickManagers(GameTime gameTime)
        {
            foreach (IAetherManager particleManager in Managers)
            {
                if (!particleManager.IsEnabled) continue;
                particleManager.Tick(gameTime);
            }
        }

        private void RenderManagers(GameTime gameTime)
        {
            foreach (IAetherManager particleManager in Managers)
            {
                if (!particleManager.IsEnabled) continue;
                if (particleManager is IRenderable)
                    ((IRenderable)particleManager).Render(gameTime);
            }
        }
        
    }
}
