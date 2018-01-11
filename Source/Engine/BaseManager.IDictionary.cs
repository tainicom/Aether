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

using tainicom.Aether.Elementary;
using System;
using System.Collections.Generic;
using tainicom.Aether.Elementary.Data;
using System.Collections;

namespace tainicom.Aether.Engine
{
    abstract public partial class BaseManager<TValue> : IDictionary<UniqueID, IAether> where TValue : class, IAether
    {
        Dictionary<UniqueID, IAether> particles = new Dictionary<UniqueID, IAether>();

        #region IDictionary Members

        public void Add(UniqueID key, IAether value)
        {
            particles.Add(key,value);
        }

        public bool ContainsKey(UniqueID key)
        {
            return particles.ContainsKey(key);
        }

        public ICollection<UniqueID> Keys
        {
            get { return particles.Keys; }
        }

        public bool Remove(UniqueID key)
        {
            return particles.Remove(key);
        }

        public bool TryGetValue(UniqueID key, out IAether value)
        {
            bool result = particles.TryGetValue(key, out value);
            return result;
        }

        public ICollection<IAether> Values
        {
            get { return particles.Values; }
        }

        public IAether this[UniqueID key]
        {
            get { return particles[key]; }
            set { particles[key] = value; }
        }

        public void Add(KeyValuePair<UniqueID, IAether> item)
        {
            particles.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            particles.Clear();
        }

        public bool Contains(KeyValuePair<UniqueID, IAether> item)
        {
            return particles.ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<UniqueID, IAether>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return particles.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<UniqueID, IAether> item)
        {
            return particles.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<UniqueID, IAether>> GetEnumerator()
        {
            return particles.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return particles.GetEnumerator();
        }

        #endregion



    }
}
