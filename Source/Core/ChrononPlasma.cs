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
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Chronons;
using tainicom.Aether.Elementary.Serialization;
using tainicom.Aether.Engine.Data;

namespace tainicom.Aether.Core
{
    public class ChrononPlasma: BasePlasma<IChronon>, IChronon
    {
        EnabledList<IChronon> _enabledParticles;
        private bool isTicking = false;

        public ChrononPlasma()
        {
            _enabledParticles = new EnabledList<IChronon>();
        }
        
        public void Tick(GameTime gameTime)
        {
            _enabledParticles.Process();
            isTicking = true;
            foreach (IChronon item in _enabledParticles)
            {
                item.Tick(gameTime);
            }
            isTicking = false;
            return;
        }

        protected override void InsertItem(int index, IChronon item)
        {
            if (isTicking)
                throw new InvalidOperationException("Can't modify collection inside Tick() method.");
            base.InsertItem(index, item);
            _enabledParticles.Add(item);
            return;
        }

        protected override void RemoveItem(int index)
        {
            if (isTicking)
                throw new InvalidOperationException("Can't modify collection inside Tick() method.");
            IChronon item = this[index];
            if (_enabledParticles.Contains(item)) _enabledParticles.Remove(item);
            base.RemoveItem(index);
        }
        
        public void Enable(IChronon item)
        {
            _enabledParticles.Enable(item);
        }

        public void Disable(IChronon item)
        {
            _enabledParticles.Disable(item);
        }

        #region Implement IAetherSerialization
        public override void Save(IAetherWriter writer)
        {
            writer.WriteInt32("Version", 1);

            base.Save(writer);
        }

        public override void Load(IAetherReader reader)
        {
            int version;
            reader.ReadInt32("Version", out version);

            switch (version)
            {
                case 1:
                    base.Load(reader);
                  break;
                default:
                  throw new InvalidOperationException("unknown version " + version);
            }
        }
        #endregion


    }
}
