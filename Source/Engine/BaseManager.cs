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
using tainicom.Aether.Elementary;
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary.Data;
using tainicom.Aether.Core;
using tainicom.Aether.Elementary.Managers;


namespace tainicom.Aether.Engine
{
    abstract public partial class BaseManager<TValue> : IAetherManager, IInitializable
    {
        #region Private / Protected
        protected AetherEngine _engine;
        #endregion

        #region Public Properties
        public bool IsEnabled { get; set; }
        public string Name {get; private set;}
        #endregion

        #region Constructors

        public BaseManager(string name)
        {
            this.Name = name;
            IsEnabled = true;
        }
        #endregion

        #region Methods
        
        public virtual void Initialize(AetherEngine engine)
        {
            this._engine = engine;
        }

        Type[] GetTypeList()
        {
            return null;
        }
        
        public void RegisterParticle(UniqueID uid, IAether particle)
        {
            TValue value = particle as TValue;
            if (value == null) return;
            this.Add(uid, (TValue)particle);
            OnRegisterParticle(uid, particle);
        }

        public void UnregisterParticle(UniqueID uid)
        {
            if (!this.ContainsKey(uid)) return;
            IAether particle = this[uid];
            OnUnregisterParticle(uid, particle);
            this.Remove(uid);
        }
        
        abstract public void Tick(GameTime gameTime);
        
        protected virtual void OnRegisterParticle(UniqueID uid, IAether particle) { }
        protected virtual void OnUnregisterParticle(UniqueID uid, IAether particle) { }
        
        public override string ToString()
        {
            return string.Format("ParticleManager '{0}', Count={1}, Type={2}", Name, Count, typeof(TValue).Name);            
        }

        #endregion

        #region Events

        #endregion

        #region Event Handlers
        #endregion

    }
}
