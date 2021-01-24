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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tainicom.Aether.Elementary.Serialization;

namespace tainicom.Aether.Engine
{
    public partial class AetherEngine : IAetherSerialization
    {        
        public virtual void Save(IAetherWriter writer)
        {   
            writer.Write("EngineData", (IAetherSerialization)EngineData);
            //writer.Write("Entities", (IAetherSerialization)Entities);

            //write particles
            writer.WriteParticles("Particles", particles);   
            //write managers
            writer.WriteParticleManagers("Managers", Managers);
        }

        public virtual void Load(IAetherReader reader)
        {
            reader.Read("EngineData", EngineData);

            //read particles
            reader.ReadParticles("Particles", particles);
            //read managers
            reader.ReadParticleManagers("Managers", Managers);

        }
    }
}
