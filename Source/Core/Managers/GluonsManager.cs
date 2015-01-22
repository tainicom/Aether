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

using tainicom.Aether.Elementary.Gluon;
using tainicom.Aether.Engine;
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Data;
using System;

namespace tainicom.Aether.Core.Managers
{
    public class GluonsManager : BaseManager<IGluon>
    {
        protected IPlasma maxStepParticles;

        public GluonsManager(AetherEngine engine, AetherContext aetherContext, string name): base(engine, aetherContext, name)
        {
            
        }

        public override void Initialize(AetherEngine engine)
        {
            Root = new GluonPlasma();
            maxStepParticles = new BasePlasma();
        }
        
        //protected override void Dispose(bool disposing)
        //{
        //    if (isDisposed) return;
        //    if (disposing)
        //    {   
        //    }
        //
        //    isDisposed = true;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        /// <remarks>Do not access this Method directly. Only AetherEngine should call it during the game loop.</remarks>
        /// <permission cref=""></permission>        
        public override void Tick(GameTime gameTime)
        {
			((ITickable)Root).Tick(gameTime);
            //((ITickable)Root).Tick(GetMaxStep(gameTime));
        }

        GameTime _maxStepGameTime = new GameTime();
        private GameTime GetMaxStep(GameTime gameTime)
        {
            if (maxStepParticles.Count == 0) return gameTime;

            TimeSpan ElapsedGameTime = gameTime.ElapsedGameTime;
            for (int i = 0; i < maxStepParticles.Count; i++)
            {
                TimeSpan maximumStep = ((IMaximumStep)maxStepParticles).MaximumStep;
                if(maximumStep<ElapsedGameTime)
                    ElapsedGameTime = maximumStep;
            }

            #if WINDOWS || WP7
            _maxStepGameTime = new GameTime(gameTime.TotalGameTime, ElapsedGameTime, gameTime.IsRunningSlowly);
            #else
            _maxStepGameTime.TotalGameTime = gameTime.TotalGameTime;
            _maxStepGameTime.ElapsedGameTime = ElapsedGameTime;
            _maxStepGameTime.IsRunningSlowly = gameTime.IsRunningSlowly;
            #endif

            return _maxStepGameTime;
        }

        protected override void OnRegisterParticle(UniqueID uid, IAether particle)
        {
            System.Diagnostics.Debug.Assert(particle is IGluon);
            Root.Add(particle);

            if (particle is IMaximumStep)
                maxStepParticles.Add(particle);
        }

        protected override void OnUnregisterParticle(UniqueID uid, IAether particle)
        {
            System.Diagnostics.Debug.Assert(particle is IGluon);
            Root.Remove(particle);

            if(particle is IMaximumStep)
                maxStepParticles.Remove(particle);
        }
        
        
    }

}
