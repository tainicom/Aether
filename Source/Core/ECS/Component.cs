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
using System.Collections.Generic;
using tainicom.Aether.Core.Managers;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.ECS;
using tainicom.Aether.Elementary.Managers;
using tainicom.Aether.Elementary.Serialization;

namespace tainicom.Aether.Core.ECS
{
    public abstract class Component : IComponent, IAetherSerialization
    {
        internal Component _nextComponent;
        internal Component _prevComponent;

        public Component()
        {
            _nextComponent = this;
            _prevComponent = this;
        }

        ~Component()
        {
        }

        //void IComponent.AttachComponent<T>(T component)
        //{
        //    Component newComponent = null;

        //    if (component is Component)
        //    {
        //        newComponent = component as Component;
        //    }
        //    else
        //    {
        //        newComponent = new ComponentProxy(component);
        //    }

        //    newComponent._prevComponent = this;
        //    newComponent._nextComponent = _nextComponent;
        //    this._nextComponent._prevComponent = newComponent;
        //    this._nextComponent = newComponent;
        //}

        //void IComponent.DettachComponent<T>(T component)
        //{
        //    if (component is Component)
        //    {
        //        throw new NotImplementedException();
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        [Obsolete("Use AetherEngine.Entities.GetEntityComponents<T>(Component)")]
        public EntityComponents<T> GetEntityComponents<T>()
            where T : class, IAether
        {
            return new EntityComponents<T>(this);
        }

        public virtual void Save(IAetherWriter writer)
        {

        }

        public virtual void Load(IAetherReader reader)
        {
            
        }

    }

    internal sealed class ComponentProxy : Component
    {
        internal IAether Value { get; private set; }

        public ComponentProxy(IAether component)
        {
            this.Value = component;
        }
    }
}
