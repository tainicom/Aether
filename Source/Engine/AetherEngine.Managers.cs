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

using Microsoft.Xna.Framework;
using tainicom.Aether.Core.Managers;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Managers;
using tainicom.Aether.Engine.Data;

namespace tainicom.Aether.Engine
{
    public partial class AetherEngine
    {
        #region Aether Fields

        public ManagerCollection Managers { get; private set; }
        
        //Managers
        public LeptonsManager LeptonsMgr;
        public ChrononsManager ChrononsMgr;
        public PhotonsManager PhotonsMgr;
        public CamerasManager CamerasMgr;
        public MaterialsManager MaterialsMgr;
        
        #endregion
        
        private void CreateManagers()
        {
            LeptonsMgr = new LeptonsManager();
            ChrononsMgr = new ChrononsManager();
            CamerasMgr = new CamerasManager();
            MaterialsMgr = new MaterialsManager();
            PhotonsMgr = new PhotonsManager();
            
            Managers.Add(LeptonsMgr);
            Managers.Add(ChrononsMgr);
            Managers.Add(PhotonsMgr);
            Managers.Add(CamerasMgr);
            Managers.Add(MaterialsMgr);

            foreach (IAetherManager particleManager in Managers)
            {
                if (particleManager is IInitializable)
                    ((IInitializable)particleManager).Initialize(this);
            }

            return;
        }

        private void TickManagers(GameTime gameTime)
        {
            foreach (IAetherManager particleManager in Managers)
            {
                if (!particleManager.IsEnabled) continue;
                particleManager.Tick(gameTime);
            }
        }

        private void PreRenderManagers(GameTime gameTime)
        {
            foreach (IAetherManager particleManager in Managers)
            {
                if (!particleManager.IsEnabled) continue;
                if (particleManager is IRenderableManager)
                    ((IRenderableManager)particleManager).PreRender(gameTime);
            }
        }

        private void RenderManagers(GameTime gameTime)
        {
            foreach (IAetherManager particleManager in Managers)
            {
                if (!particleManager.IsEnabled) continue;
                if (particleManager is IRenderableManager)
                    ((IRenderableManager)particleManager).Render(gameTime);
            }
        }
        
    }
}
