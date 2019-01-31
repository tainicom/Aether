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


using Microsoft.Xna.Framework;
using tainicom.Aether.Core.Walkers;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Chronons;
using tainicom.Aether.Elementary.Data;
using tainicom.Aether.Elementary.Photons;
using tainicom.Aether.Elementary.Serialization;
using tainicom.Aether.Engine;

namespace tainicom.Aether.Core.Managers
{
    public class PhotonsManager: BaseManager<IPhotonNode>, IRenderableManager
    {
        public IPlasmaList<IPhotonNode> Root { get; protected set; }

        public IPhotonWalker DefaultWalker { get; set; }

        public PhotonsManager(): base("Photons")
        {
        }

        public override void Initialize(AetherEngine engine)
        {
            base.Initialize(engine);
            Root = new PhotonPlasma();
            //Root = engine.RegisterParticle(new PhotonPlasma(), "Root");

            DefaultWalker = new PhotonsWalker();
            ((IInitializable)DefaultWalker).Initialize(engine);

            return;
        }

        public override void Tick(GameTime gameTime)
        {
            /*see Render()*/
            return;
        }
        public void PreRender(GameTime gameTime)
        {
        }

        public void Render(GameTime gameTime)
        {
            Render(gameTime, DefaultWalker);
        }

        public void Render(GameTime gameTime, IPhotonWalker walker)
        {
            walker.Reset();
            while (walker.MoveNext())
                walker.Render(gameTime);
        }

        protected override void OnRegisterParticle(UniqueID uid, IAether particle)
        {
            System.Diagnostics.Debug.Assert(particle is IPhotonNode);
            _engine.AddChild(Root, (IPhotonNode)particle);
        }

        protected override void OnUnregisterParticle(UniqueID uid, IAether particle)
        {
            System.Diagnostics.Debug.Assert(particle is IPhotonNode);
            _engine.RemoveChild(Root, (IPhotonNode)particle);
        }

#if (WINDOWS)
        public override void Save(IAetherWriter writer)
        {
            base.Save(writer);

            //write root
            if (Root is IAetherSerialization)
                writer.Write("Root", (IAetherSerialization)Root);
        }
#endif

        public override void Load(IAetherReader reader)
        {
            base.Load(reader);

            //read root
            if (Root is IAetherSerialization)
                reader.Read("Root", (IAetherSerialization)Root);
        }
    }
}
