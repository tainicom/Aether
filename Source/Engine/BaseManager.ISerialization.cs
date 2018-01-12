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

using tainicom.Aether.Elementary;
using System;
using System.Collections.Generic;
using tainicom.Aether.Elementary.Data;
using System.Collections;
using tainicom.Aether.Elementary.Serialization;

namespace tainicom.Aether.Engine
{
    abstract public partial class BaseManager<TValue> : IAetherSerialization
    {   
        #if (WINDOWS)
        public virtual void Save(IAetherWriter writer)
        {
            //write particles
            writer.WriteParticles("Particles", particles);
        }
        #endif

        public virtual void Load(IAetherReader reader)
        {            
            //read particles
            reader.ReadParticles("Particles", particles);
        }

    }
}
