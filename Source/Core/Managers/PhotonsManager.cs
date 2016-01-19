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

using tainicom.Aether.Engine;
using tainicom.Aether.Elementary.Photons;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Data;
using tainicom.Aether.Core.Walkers;
using Microsoft.Xna.Framework;

namespace tainicom.Aether.Core.Managers
{
    public class PhotonsManager: BaseManager<IPhoton>, IRenderable
    {
        public IAetherWalker DefaultWalker { get; set; }

        public PhotonsManager(): base("Photons")
        {
        }

        public override void Initialize(AetherEngine engine)
        {
            this._engine = engine;
            Root = new PhotonPlasma();
            //Root = engine.RegisterParticle(new PhotonPlasma(), "Root");

            DefaultWalker = new PhotonsWalker();
            ((IInitializable)DefaultWalker).Initialize(engine);

            return;
        }

        public override void Tick(GameTime gameTime)
        {
            /*see Render()*/
            ITickable tickableRoot = Root as ITickable;
            if(tickableRoot !=null)
                tickableRoot.Tick(gameTime);
            return;
        }

        public void Render(GameTime gameTime)
        {
            Render(gameTime, DefaultWalker);
        }

        public void Render(GameTime gameTime, IAetherWalker walker)
        {
            walker.Reset();
            while (walker.MoveNext())
                ((IRenderable)walker).Render(gameTime);
        }

        protected override void OnRegisterParticle(UniqueID uid, IAether particle)
        {
            System.Diagnostics.Debug.Assert(particle is IPhoton);
            Root.Add(particle);
        }

        protected override void OnUnregisterParticle(UniqueID uid, IAether particle)
        {
            System.Diagnostics.Debug.Assert(particle is IPhoton);
            Root.Remove(particle);
        }

    }
}
