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
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Data;
using tainicom.Aether.Elementary.Managers;
using tainicom.Aether.Engine.Data;

namespace tainicom.Aether.Engine
{
    public partial class AetherEngine
    {
        #region Aether Fields

        private AetherContext _context;
        
        internal AetherEngineData EngineData;

        #endregion

        public AetherContext Context { get { return _context; } }

        public float TotalTime { get { return (float)EngineData.TotalTime; } }

        
        public AetherEngine(AetherContext context)
        {
            this._context = context;
            //initialize trivial data
            EngineData = new AetherEngineData();

            Managers = new ManagerCollection(this);
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
            foreach (IAetherManager particleManager in Managers)
            {
                particleManager.RegisterParticle(uid, particle);
            }

            return uid;
        }

        public void UnregisterParticle(IAether particle)
        {
            UniqueID uid = FindUniqueId(particle);
            //notify managers
            foreach (IAetherManager particleManager in Managers)
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

        public void PreRender(GameTime gameTime)
        {
            PreRenderManagers(gameTime);
        }

        public void Render(GameTime gameTime)
        {
            RenderManagers(gameTime);
        }

        public virtual void AddChild<TAether>(IPlasmaList<TAether> parent, TAether child)
            where TAether : IAether
        {
            parent.Add(child);
        }

        public virtual void InsertChild<TAether>(IPlasmaList<TAether> parent, int index, TAether child)
            where TAether : IAether
        {
            parent.Insert(index, child);
        }

        public virtual void RemoveChild<TAether>(IPlasmaList<TAether> parent, TAether child)
            where TAether : IAether
        {
            parent.Remove(child);
        }

    }
}
