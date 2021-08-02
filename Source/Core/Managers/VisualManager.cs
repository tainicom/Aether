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
using tainicom.Aether.Core.Walkers;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Temporal;
using tainicom.Aether.Elementary.Data;
using tainicom.Aether.Elementary.Visual;
using tainicom.Aether.Elementary.Serialization;
using tainicom.Aether.Engine;

namespace tainicom.Aether.Core.Managers
{
    public class VisualManager : BaseManager<IVisualNode>, IRenderableManager
    {
        public delegate void OnRenderError(GameTime gameTime, IVisualWalker walker, IVisualNode current, ref bool handled);
        public OnRenderError RenderError;

        public IPlasmaList<IVisualNode> Root { get; protected set; }

        public IVisualWalker DefaultWalker { get; set; }

        public VisualManager(): base("VisualMgr")
        {
        }

        public override void Initialize(AetherEngine engine)
        {
            base.Initialize(engine);
            Root = new VisualCollection();
            //Root = engine.RegisterParticle(new VisualCollection(), "Root");

            DefaultWalker = new VisualWalker();
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

        public void Render(GameTime gameTime, IVisualWalker walker)
        {
            walker.Reset();
            while (walker.MoveNext())
            {
                var visualNode = walker.Current as IVisualNode;

                try
                {
                    walker.Render(gameTime, visualNode);
                }
                catch
                {
                    bool handled = false;
                    var handler = RenderError;
                    if (handler != null)
                        handler(gameTime, walker, visualNode, ref handled);

                    if (!handled)
                        throw;
                }
            }
        }

        protected override void OnRegisterParticle(UniqueID uid, IAether particle)
        {
            System.Diagnostics.Debug.Assert(particle is IVisualNode);
            _engine.AddChild<IVisualNode>(Root, (IVisualNode)particle);
        }

        protected override void OnUnregisterParticle(UniqueID uid, IAether particle)
        {
            System.Diagnostics.Debug.Assert(particle is IVisualNode);
            _engine.RemoveChild<IVisualNode>(Root, (IVisualNode)particle);
        }

        public override void Save(IAetherWriter writer)
        {
            base.Save(writer);

            //write root
            if (Root is IAetherSerialization)
                writer.Write("Root", (IAetherSerialization)Root);
        }

        public override void Load(IAetherReader reader)
        {
            base.Load(reader);

            //read root
            if (Root is IAetherSerialization)
                reader.Read("Root", (IAetherSerialization)Root);
        }
    }
}
