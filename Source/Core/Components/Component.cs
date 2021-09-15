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

        public virtual void Save(IAetherWriter writer)
        {

        }

        public virtual void Load(IAetherReader reader)
        {
            
        }

        #region static methods

        [Obsolete("Use Component.GetComponents<T>(element)")]
        public static T GetFirtComponent<T>(IAether element) where T : class
        {
            T result = null;
            if (element is T)
                return (T)element;

            return result;
        }

        public static EntityComponents<T> GetEntityComponents<T>(Component component)
            where T : class, IAether
        {
            return new EntityComponents<T>(component);
        }

        public static IEnumerable<T> GetComponents<T>(IAether element)
            where T : class, IAether
        {
            var component = element as IComponent;
            if (component != null)
                return GetComponents<T>(component);
            else
                return GetInterface<T>(element);
        }

        private static IEnumerable<T> GetInterface<T>(IAether element)
          where T : class, IAether
        {
            T result = null;
            if (element is T)
                result = (T)element;
            yield return result;
        }

        public static IEnumerable<T> GetComponents<T>(IComponent component)
            where T : class, IAether
        {
            System.Diagnostics.Debug.Assert(Object.ReferenceEquals(component, component.Entity._component));
            var entityNode = component.Entity;

            do
            {
                if (entityNode._component is T)
                    yield return (T)entityNode._component;

                entityNode = entityNode._nextComponentNode;
            }
            while (!Object.ReferenceEquals(entityNode, component.Entity));

            yield break;
        }

        #endregion
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
