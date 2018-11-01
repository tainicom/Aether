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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Gluon;
using tainicom.Aether.Elementary.Photons;
using tainicom.Aether.Elementary.Serialization;
using tainicom.Aether.Engine.Data;

namespace tainicom.Aether.Core
{
    public class PhotonPlasma: BasePlasma<IPhotonNode>, IPhotonPlasma, ITickable
    {
        List<IPhotonNode> _visibleParticles;

        public IEnumerator<IPhotonNode> VisibleParticles { get { return _visibleParticles.GetEnumerator(); } }

        public PhotonPlasma()
        {
            _visibleParticles = new EnabledList<IPhotonNode>();
        }
        
        public void Tick(GameTime gameTime)
        {
            return; // TODO: remove
        }

        protected override void InsertItem(int index, IPhotonNode item)
        {
            base.InsertItem(index, item);
            _visibleParticles.Add(item);
            return;
        }

        protected override void RemoveItem(int index)
        {
            IAether item = this[index];
            if (_visibleParticles.Contains((IPhotonNode)item))
                _visibleParticles.Remove((IPhotonNode)item);
            base.RemoveItem(index);
        }
        
        public void Enable(IPhoton item)
        {
            if(!_visibleParticles.Contains(item))
                _visibleParticles.Add(item);
        }

        public void Disable(IPhoton item)
        {
            if(_visibleParticles.Contains(item))
                _visibleParticles.Remove(item);
        }

        public bool IsEnabled(IPhoton item)
        {
            return _visibleParticles.Contains(item);
        }


        #region Implement IAetherSerialization
#if (WINDOWS)
        public override void Save(IAetherWriter writer)
        {
            writer.WriteInt32("Version", 1);

            base.Save(writer);
            writer.WriteParticles("VisibleParticles", _visibleParticles);
        }
#endif

        public override void Load(IAetherReader reader)
        {
            int version;
            reader.ReadInt32("Version", out version);

            switch (version)
            {
                case 1:
                base.Load(reader);
                _visibleParticles.Clear();
                  reader.ReadParticles("VisibleParticles", _visibleParticles);
                  break;
                default:
                  throw new InvalidOperationException("unknown version " + version);
            }
        }
        #endregion


    }
}
