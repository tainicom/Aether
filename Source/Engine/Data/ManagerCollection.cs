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

using System.Collections.ObjectModel;
using tainicom.Aether.Elementary.Managers;

namespace tainicom.Aether.Engine.Data
{
    public partial class ManagerCollection : Collection<IAetherManager>
    {
        private AetherEngine _engine;

        public ManagerCollection(AetherEngine engine)
        {
            this._engine = engine;
        }

        public IAetherManager this[string name] 
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Name == name) 
                        return this[i];
                }
                return null;
            }
        }
        
        public TManager GetManager<TManager>() where TManager : class, IAetherManager
        {
            var itemType = typeof(TManager);
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].GetType() == itemType)
                    return (TManager)this[i];
            }
            return null;
        }

        protected override void InsertItem(int index, IAetherManager item)
        {
			// check if manager allready in use.
            var itemType = item.GetType();
            foreach (var manager in this)
            {
                if (manager.GetType().Equals(itemType))
                    throw new AetherException(string.Format("Manager of type {0} allready in use.", itemType.Name));
            }
            
            base.InsertItem(index, item);
        }
        
    }
}
