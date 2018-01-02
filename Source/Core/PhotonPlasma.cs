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

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Gluon;
using tainicom.Aether.Elementary.Photons;
using tainicom.Aether.Engine.Data;

namespace tainicom.Aether.Core
{
    public class PhotonPlasma: BasePlasma<IAether>, IPhotonPlasma, ITickable
    {
        protected EnabledList<IAether> _enabledParticles;

        public IEnumerator<IAether> VisibleParticles { get { return _enabledParticles.GetEnumerator(); } }

        public PhotonPlasma()
        {
            _enabledParticles = new EnabledList<IAether>();
        }
        
        public void Tick(GameTime gameTime)
        {
            _enabledParticles.Process();
            return;
        }

        protected override void InsertItem(int index, IAether item)
        {
            base.InsertItem(index, item);
            _enabledParticles.Add((IPhoton)item);
            return;
        }

        protected override void RemoveItem(int index)
        {
            IAether item = this[index];
            if (_enabledParticles.Contains((IPhoton)item))
                _enabledParticles.Remove((IPhoton)item);
            base.RemoveItem(index);
        }
        
        public void Enable(IPhoton item)
        {
            _enabledParticles.Enable(item);
        }

        public void Disable(IPhoton item)
        {
            _enabledParticles.Disable(item);
        }

        public bool IsEnabled(IPhoton photon, bool includePending)
        {
            return _enabledParticles.Contains(photon, includePending);
        }
    }
}
