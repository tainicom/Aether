#region License
//   Copyright 2021 Kastellanos Nikolaos
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
    public sealed class ComponentNode
    {
        internal readonly IComponent _component;
        internal ComponentNode _nextComponentNode;
        internal ComponentNode _prevComponentNode;

        public ComponentNode(IComponent component)
        {
            if (component == null)
                throw new ArgumentNullException("component");

            _component = component;
            _prevComponentNode = this;
            _nextComponentNode = this;
        }

        ~ComponentNode()
        {
        }
    }

}
