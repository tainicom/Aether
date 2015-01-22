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

using tainicom.Aether.Elementary;
using tainicom.Aether.Engine.Data;
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary.Leptons;

namespace tainicom.Aether.Core
{
    public class LeptonPlasma: BasePlasma, ITickable
    {
        EnabledList<IAether> _enabledParticles;

        public LeptonPlasma()
        {
            _enabledParticles = new EnabledList<IAether>();
        }
        
        public void Tick(GameTime gameTime)
        {
            _enabledParticles.Process();
            foreach (ILepton item in _enabledParticles)
            {
                //item.Tick(gameTime);
            }
            return;
        }

        protected override void InsertItem(int index, IAether item)
        {
            base.InsertItem(index, item);
            _enabledParticles.Add(item);
            return;
        }

        protected override void RemoveItem(int index)
        {
            IAether item = this[index];
            if (_enabledParticles.Contains(item)) _enabledParticles.Remove(item);
            base.RemoveItem(index);
        }
        
        public void Enable(ILepton item)
        {
            _enabledParticles.Enable(item);
        }

        public void Disable(ILepton item)
        {
            _enabledParticles.Disable(item);
        }

    }
}
