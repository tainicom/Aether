#region License
//   Copyright 2020-2021 Kastellanos Nikolaos
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
using System.Collections;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Components;
using tainicom.Aether.Elementary.Serialization;

namespace tainicom.Aether.Core.Components
{
    public abstract class Component : IComponent, IAetherSerialization
    {
        internal Component _nextComponent;
        internal Component _prevComponent;

        public ComponentNode Entity { get; private set; }

        public Component()
        {
            Entity = new ComponentNode(this);

            _nextComponent = this;
            _prevComponent = this;
        }

        ~Component()
        {
        }

        public virtual void Save(IAetherWriter writer)
        {

        }

        public virtual void Load(IAetherReader reader)
        {
            
        }

        #region static methods

        public static EntityComponentsIterator<T> GetComponents<T>(IAether element)
            where T : class, IAether
        {
            return new EntityComponentsIterator<T>(element);
        }

        #endregion
    }
}
