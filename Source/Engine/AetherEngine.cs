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
using System.Runtime.Serialization;
using tainicom.Aether.Engine;
using tainicom.Aether.Core.Managers;
using tainicom.Aether.Elementary.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Elementary.Managers;
using tainicom.Aether.Elementary;
using tainicom.Aether.Core.Walkers;

//namespace tainicom.Aether.Engine
namespace tainicom.Aether.Engine
{
    public partial class AetherEngine
    {
        #region Aether Fields

        private AetherContext _context;

        public AetherEngineData EngineData;
        
        internal IPlasma Root;

        public AetherContext Context { get { return _context; } }

        #endregion
        
        public AetherEngine(AetherContext context)
        {
            this._context = context;
            //initialize trivial data
            EngineData = new AetherEngineData();

            _particleManagers = new List<IAetherManager>();
            CreateManagers();
        }
        
        internal T RegisterParticle<T>(T particle, string name) where T:IAether
        {
            RegisterParticle(particle);
            SetParticleName(particle, name);
            return particle;
        }
                
        public UniqueID RegisterParticle(IAether particle)
        {
            // check if particle is allready registered
            UniqueID uid = FindUniqueId(particle);
            if (!uid.Equals(UniqueID.Unknown))
            {
                return uid;
            }

            //create new uid and add particle
            uid = EngineData.NextUniqueID.GetNext();
            EngineData.NextUniqueID = uid;
            this.Add(uid, particle);

            //notify managers
            foreach (IAetherManager particleManager in _particleManagers)
            {
                particleManager.RegisterParticle(uid, particle);
            }

            return uid;
        }

        public void UnregisterParticle(IAether particle)
        {
            UniqueID uid = FindUniqueId(particle);
            //notify managers
            foreach (IAetherManager particleManager in _particleManagers)
            {
                particleManager.UnregisterParticle(uid);
            }
            this.Remove(uid);
        }

        private UniqueID FindUniqueId(IAether particle)
        {
            if(!this.Values.Contains(particle)) 
                return UniqueID.Unknown;

            foreach(UniqueID uid in this.Keys)
                if(this[uid]==particle) return uid;
            
            return UniqueID.Unknown;
        }

        public void Tick(GameTime gameTime)
        {
            TickManagers(gameTime);            
            EngineData.TotalTime += gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }
                
        public void Render(GameTime gameTime)
        {
            RenderManagers(gameTime);
        }
        
        public virtual void AddChild(IPlasma parent, IAether child)
        {
            parent.Add(child);
        }

        public virtual void RemoveChild(IPlasma parent, IAether child)
        {
            parent.Remove(child);
        }

        public static T GetComponent<T>(IAether element) where T: class
        {
            T result = null;
            try { return (T)element; }
            catch(InvalidCastException ice) { }
            
            return result;
        }
    }
}
