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

using System;
using tainicom.Aether.Elementary.Chronons;
using tainicom.Aether.Elementary.Data;

namespace tainicom.Aether.Elementary.Managers
{
    public interface IAetherManager : ITickable, IDisposable
    {
        string Name { get; }

        bool IsEnabled { get; set; }
        
        /// <summary>
        /// Do not call this directly. It is called by AetherEngine upon AetherEngine.RegisterParticle(...).
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="particle"></param>
        void RegisterParticle(UniqueID uid, IAether particle);

        /// <summary>
        /// Do not call this directly. It is called by AetherEngine upon AetherEngine.UnregisterParticle(...).
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="particle"></param>
        void UnregisterParticle(UniqueID uid);
        
    }
}
