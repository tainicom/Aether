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

using Microsoft.Xna.Framework;

namespace tainicom.Aether.Elementary.Leptons
{
    /// <summary>
    /// Particle that have position, rotation and scale
    /// </summary>
    public interface ILepton : ILocalTransform, IPosition, IAether
    {
        //Matrix LocalTransform { get; } //Defined in ILocalTransform 
        
        Quaternion Rotation { get; set; }
        Vector3 Scale { get; set; }
        //Vector3 Position { get; set; } //Defined in IPosition
    }
}
