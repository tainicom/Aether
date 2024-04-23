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
using tainicom.Aether.Elementary;


namespace tainicom.Aether.Core.Components
{
    public struct EntityComponentsIterator<T>
        where T : class, IAether
    {
        private IAether element;

        public EntityComponentsIterator(IAether element)
        {
            this.element = element;
        }

        public EntityComponentsEnumerator<T> GetEnumerator()
        {
            return new EntityComponentsEnumerator<T>(element);
        }
    }

    public struct EntityComponentsEnumerator<T>
        where T : class, IAether
    {
        private readonly IAether _head;
        private T _current;

        bool isComponent;
        private Component currentComponent;

        public EntityComponentsEnumerator(IAether element)
        {
            this._head = element;
            this._current = default(T);

            this.currentComponent = null;
            this.isComponent = false;
            if (_head is Component)
                this.isComponent = true;
        }

        public T Current { get { return _current; } }

        public bool MoveNext()
        {
            if (!isComponent)
                return MoveNextInterface();
            else
                return MoveNextComponent();
        }

        private bool MoveNextInterface()
        {
            if (_current == null && _head is T)
            {
                _current = (T)_head;
                return true;
            }

            return false;
        }

        private bool MoveNextComponent()
        {
            while (true)
            {
                if (Current == null)
                {
                    currentComponent = (Component)_head;
                }
                else
                {
                    // get next component
                    currentComponent = currentComponent._nextComponent;                    
                    if (Object.ReferenceEquals(currentComponent, _head))
                        break;
                }

                if (currentComponent is T)
                {
                    _current = (T)currentComponent.Entity._component;
                    return true;
                }
            }

            _current = null;
            return false;
        }
    }

}
