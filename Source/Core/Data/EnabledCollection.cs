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

using System.Collections.Generic;

namespace tainicom.Aether.Engine.Data
{
    internal class EnabledList<T>: List<T>, IEnumerable<T>
    {
        List<T> enableList = new List<T>();
        List<T> disableList = new List<T>();

        public void Enable(T item)
        {
            if (enableList.Contains(item)) return;
            if (this.Contains(item))
            {
                if (disableList.Contains(item))
                    disableList.Remove(item);
                //else
                return;
            }
            enableList.Add(item);
        }

        public void Disable(T item)
        {
            if (disableList.Contains(item)) return;
            if (!this.Contains(item))
            {
                if (enableList.Contains(item))
                    enableList.Remove(item);
                else return;
            }
            disableList.Add(item);
        }

        public void Process()
        {
            if (disableList.Count > 0) RemoveDisabled();
            if (enableList.Count > 0) AddEnabled();
        }

        public bool Contains(T item, bool includePending)
        {
            if (!includePending) return this.Contains(item);
            
            if (enableList.Contains(item)) return true;
            if (this.Contains(item) && !disableList.Contains(item)) return true;
            return false;
        }

        private void AddEnabled()
        {
            foreach (T item in enableList) this.Add(item);
            enableList.Clear();
        }
        private void RemoveDisabled()
        {
            foreach (T item in disableList) this.Remove(item);
            disableList.Clear();
        }
        

    }
}
