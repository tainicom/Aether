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
using System.Collections.Generic;


namespace tainicom.Aether.Core.Components
{
    public struct EntityComponents<T>
        where T : class
    {
        private readonly Component component;

        internal EntityComponents(Component component)
        {
            this.component = component;
        }

        public ComponentEnumerator<T> GetEnumerator()
        {
            return new ComponentEnumerator<T>(this.component);
        }
    }

    public struct ComponentEnumerator<T>
        where T : class
    {
        private readonly Component headComponent;
        private Component currentComponent;

        internal ComponentEnumerator(Component component)
        {
            this.headComponent = component;
            this.currentComponent = null;
        }

        public T Current
        {
            get
            {
                if (currentComponent is ComponentProxy)
                    return ((ComponentProxy)currentComponent).Value as T;
                else
                    return currentComponent as T;
            }
        }

        public bool MoveNext()
        {
            while (true)
            {
                if (currentComponent == null)
                {
                    currentComponent = headComponent;
                }
                else
                {
                    currentComponent = currentComponent._nextComponent;
                    if (currentComponent == headComponent)
                    {
                        return false;
                    }
                }

                if (currentComponent is T)
                    return true;
            }

            return false;
        }

        public void Reset()
        {
            this.currentComponent = null;
        }
    }
}
