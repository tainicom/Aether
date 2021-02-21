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
using System.Collections.ObjectModel;
using System.Diagnostics;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Serialization;

namespace tainicom.Aether.Core
{
    public class BasePlasma<TAether> : Collection<TAether>, IPlasmaList<TAether>, IAetherSerialization
        where TAether : IAether
    {
        protected override void InsertItem(int index, TAether item)
        {
            Debug.Assert(!this.Contains(item));
            base.InsertItem(index, item);
            
            IAetherNotify<TAether> notify = item as IAetherNotify<TAether>;
            if (notify != null) notify.OnAttachedTo(this);
        }

        protected override void RemoveItem(int index)
        {
            IAetherNotify<TAether> notify = this[index] as IAetherNotify<TAether>;
            if (notify != null) notify.OnDettachedFrom(this);

            base.RemoveItem(index);
        }
        
        public virtual void Save(IAetherWriter writer)
        {
            writer.WriteInt32("Version", 1);

            writer.WriteParticles("Particles", this);
        }

        public virtual void Load(IAetherReader reader)
        {
            int version;
            reader.ReadInt32("Version", out version);
            
            switch (version)
            {
                case 1:
                  reader.ReadParticles("Particles", this);
                  break;
                default:
                  throw new InvalidOperationException("unknown version " + version);
            }
        }
    }
}
