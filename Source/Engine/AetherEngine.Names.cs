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
using System.Text;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Data;

namespace tainicom.Aether.Engine
{
    public partial class AetherEngine //: IDictionary<UniqueID, IAetherParticle>
    {
        // a global collection of all names in the engine
        Dictionary<String, IAether> _names = new Dictionary<String, IAether>();
        
        public IAether this[string name]
        {
            get { return _names[name]; }
        }

        public bool ContainsName(string name)
        {
            return _names.ContainsKey(name);
        }

        public void SetParticleName<T>(T particle, string name) where T:IAether
        {
            IAether oldParticle;
            _names.TryGetValue(name, out oldParticle);
            if (oldParticle == (IAether)particle) return;
            if (oldParticle != null) throw new Exception("Name allready set to another object");
            
            // remove old name
            String oldName = GetParticleName(particle);
            if (oldName != String.Empty)
                _names.Remove(oldName);
            
            if (String.IsNullOrEmpty(name))
                return;

            // set name
            _names[name] = particle;
            return;
        }

        public string GetParticleName(IAether particle)
        {
            foreach (KeyValuePair<string, IAether> nameParticlePair in _names)
                if (nameParticlePair.Value == particle) 
                    return nameParticlePair.Key;

            return string.Empty;
        }

        public bool RemoveParticleName(IAether particle)
        {
             foreach (KeyValuePair<string, IAether> nameParticlePair in _names)
                 if (nameParticlePair.Value == particle)
                 {
                     return _names.Remove(nameParticlePair.Key);
                 }
             return false;
        }

        public IEnumerator<String> GetNames()
        {
            return _names.Keys.GetEnumerator();
        }

    }
}
