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
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Temporal;
using tainicom.Aether.Elementary.Data;
using tainicom.Aether.Elementary.Spatial;
using tainicom.Aether.Elementary.Serialization;
using tainicom.Aether.Engine;

namespace tainicom.Aether.Core.Managers
{
    public class SpatialManager : BaseManager<ISpatialNode>
    {
        public IPlasmaList<ISpatialNode> Root { get; protected set; }

        public SpatialManager(): base("Leptons")
        {
            
        }

        public override void Initialize(AetherEngine engine)
        {
            base.Initialize(engine);
            Root = new LeptonPlasma();
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
            ITickable tickableRoot = Root as ITickable;
            if (tickableRoot != null)
                tickableRoot.Tick(gameTime);
        }

        protected override void OnRegisterParticle(UniqueID uid, IAether particle)
        {
            System.Diagnostics.Debug.Assert(particle is ISpatialNode);
            //_engine.AddChild(Root, (ISpatialNode)particle);
        }

        protected override void OnUnregisterParticle(UniqueID uid, IAether particle)
        {
            System.Diagnostics.Debug.Assert(particle is ISpatialNode);
            //_engine.RemoveChild(Root, (ISpatialNode)particle);
        }

        public static Matrix GetWorldTransform(IAether particle)
        {
            Matrix result;
            GetWorldTransform(particle, out result);
            return result;
        }

        public static void GetWorldTransform(IAether particle, out Matrix world)
        {
            var worldTransform = particle as IWorldTransform;
            if (worldTransform != null)
            {
                world = worldTransform.WorldTransform;
                return;
            }
            var localTransform = particle as ILocalTransform;
            if (localTransform != null)
            {
                world = localTransform.LocalTransform;
                return;
            }
            var position = particle as IPosition;
            if (position != null)
            {
                world = Matrix.CreateTranslation(position.Position);
                return;
            }
            
            world = Matrix.Identity;
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
