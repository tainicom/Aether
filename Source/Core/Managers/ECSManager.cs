#region License
//   Copyright 2019 Kastellanos Nikolaos
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
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Data;
using tainicom.Aether.Elementary.ECS;
using tainicom.Aether.Elementary.Serialization;
using tainicom.Aether.Engine;

namespace tainicom.Aether.Core.Managers
{
    class ECSManager : BaseManager<IECSNode>
    {
        public ECSManager(): base("ECS")
        {
        }

        public override void Initialize(AetherEngine engine)
        {
            base.Initialize(engine);
            
            return;
        }

        public override void Tick(GameTime gameTime)
        {
            return;
        }
        
        protected override void OnRegisterParticle(UniqueID uid, IAether particle)
        {
            System.Diagnostics.Debug.Assert(particle is IECSNode);

        }

        protected override void OnUnregisterParticle(UniqueID uid, IAether particle)
        {
            System.Diagnostics.Debug.Assert(particle is IECSNode);

        }


        public override void Save(IAetherWriter writer)
        {
            writer.WriteInt32("Version", 1);

            base.Save(writer);

        }

        public override void Load(IAetherReader reader)
        {
            int version;
            reader.ReadInt32("Version", out version);

            switch (version)
            {
                case 1:
                    base.Load(reader);

                    break;
                default:
                    throw new InvalidOperationException("unknown version " + version);
            }

        }
    }
}
