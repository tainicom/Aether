﻿#region License
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
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Gluon;
using tainicom.Aether.Engine.Data;

namespace tainicom.Aether.Core
{
    public class GluonPlasma: BasePlasma<IAether>, ITickable
    {
        EnabledList<IAether> _enabledParticles;
        private bool isEnumerating = false; 

        public GluonPlasma()
        {
            _enabledParticles = new EnabledList<IAether>();
        }
        
        public void Tick(GameTime gameTime)
        {
            _enabledParticles.Process();
            isEnumerating = true;
            foreach (IGluon item in _enabledParticles)
            {
                item.Tick(gameTime);
            }
            isEnumerating = false;
            return;
        }

        protected override void InsertItem(int index, IAether item)
        {
            if (isEnumerating)
                throw new InvalidOperationException("Can't modify collection inside Tick() method.");
            base.InsertItem(index, item);
            _enabledParticles.Add(item);
            return;
        }

        protected override void RemoveItem(int index)
        {
            if (isEnumerating)
                throw new InvalidOperationException("Can't modify collection inside Tick() method.");
            IAether item = this[index];
            if (_enabledParticles.Contains(item)) _enabledParticles.Remove(item);
            base.RemoveItem(index);
        }
        
        public void Enable(IGluon item)
        {
            _enabledParticles.Enable(item);
        }

        public void Disable(IGluon item)
        {
            _enabledParticles.Disable(item);
        }

    }
}
