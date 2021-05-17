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
using tainicom.Aether.Elementary.Serialization;
using tainicom.Aether.Engine;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Data;
using System.IO;
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary.Managers;

namespace tainicom.Aether.Core.Serialization
{
    public class AetherBinaryReader : IAetherReader
    {
        public IAetherTypeResolver TypeResolver { get; set; }

        public readonly AetherEngine Engine;
        private Stream stream;
        private BinaryReader reader;        

        Dictionary<UniqueID, IAether> deserialisedParticles = new Dictionary<UniqueID, IAether>();
        List<Type> knownTypes = new List<Type>();

        public AetherBinaryReader(AetherEngine engine, Stream stream)
        {
            this.Engine = engine;
            this.stream = stream;
            reader = new BinaryReader(stream, Encoding.UTF8);
        }

        public void Close()
        {
            reader.Dispose();
        }

        public void Read(string name, IAetherSerialization value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            value.Load(this);
        }

        public void ReadParticles<T>(string name, IDictionary<UniqueID, T> particles) where T : IAether
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            int particlesCount = reader.ReadInt32();
            for (int i = 0; i < particlesCount; i++)
            {
                UniqueID uid;
                IAether particle;
                ReadParticle(out uid, out particle);
                particles.Add(uid, (T)particle);
            }
        }

        public void ReadParticles<T>(string name, IList<T> particles) where T : IAether
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            int particlesCount = reader.ReadInt32();
            for (int i = 0; i < particlesCount; i++)
            {
                UniqueID uid;
                IAether particle;
                ReadParticle(out uid, out particle);
                particles.Add((T)particle);
            }
        }

        public void ReadParticle(string name, out IAether particle)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            bool isParticleUnknown = reader.ReadBoolean();
            UniqueID uniqueID;
            if (isParticleUnknown) particle = null;
            else ReadParticle(out uniqueID, out particle);
        }

        private void ReadParticle(out UniqueID uid, out IAether particle)
        {
            uid = new UniqueID();
            uid.Load(this);

            bool isParticleSerialized = reader.ReadBoolean();
            if (!isParticleSerialized)
            {
                string particleName = reader.ReadString();
                Type particleType = ReadType();

                if (Engine.ContainsName(particleName))
                {
                    particle = Engine[particleName];
                }
                else
                {
                    particle = (IAether)Activator.CreateInstance(particleType);
                }

                if (!uid.Equals(UniqueID.Unknown))
                    deserialisedParticles.Add(uid, particle);

                bool isSerialisableParticle = reader.ReadBoolean();
                if (isSerialisableParticle)
                {
                    IAetherSerialization serialisableParticle = particle as IAetherSerialization;
                    serialisableParticle.Load(this);
                }

                if (particleName != string.Empty)
                    Engine.SetParticleName(particle, particleName);
            }
            else
            {
                particle = deserialisedParticles[uid];
            }

            return;
        }

        private Type ReadType()
        {
            Type particleType;
            int typeIndex;
            ReadInt32(out typeIndex);
            bool isKnownType = typeIndex < knownTypes.Count;
            if (isKnownType)
            {
                particleType = knownTypes[typeIndex];
            }
            else
            {
                System.Diagnostics.Debug.Assert(typeIndex == knownTypes.Count);
                string AssemblyQualifiedName = reader.ReadString();
                particleType = TypeResolver.ResolveType(AssemblyQualifiedName);
                knownTypes.Add(particleType);
            }

            return particleType;
        }

        public void ReadParticleManagers(string name, IList<IAetherManager> particleManagers)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            int particleManagersCount = reader.ReadInt32();
            for (int i = 0; i < particleManagersCount; i++)
            {
                ReadParticleManager(particleManagers);
            }
        }

        private void ReadParticleManager(IList<IAetherManager> particleManagers)
        {
            string managerName = reader.ReadString();
            string assemblyQualifiedName = reader.ReadString();
            Type managerType = Type.GetType(assemblyQualifiedName);
            IAetherManager manager = null;
            foreach (IAetherManager mgr in particleManagers)
                if (mgr.Name == managerName) { manager = mgr; break; }
            if (manager == null)
            {
                manager = (IAetherManager)Activator.CreateInstance(managerType);
                particleManagers.Add(manager);
                if (manager is IInitializable)
                    ((IInitializable)manager).Initialize(this.Engine);
            }
            IAetherSerialization serialisableParticle = manager as IAetherSerialization;
            bool isSerialisableParticle = reader.ReadBoolean();
            if (isSerialisableParticle)
            {
                System.Diagnostics.Debug.Assert(manager != null, "According to the binary file, manager should be IAetherSerialization.");
                serialisableParticle.Load(this);
            }
        }

        public void ReadBoolean(string name, out bool value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            value = reader.ReadBoolean();
        }

        public void ReadInt64(string name, out Int64 value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            value = reader.ReadInt64();
        }

        public void ReadInt64(out Int64 value)
        {
            value = reader.ReadInt64();
        }

        public void ReadInt32(string name, out int value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            value = reader.ReadInt32();
        }

        public void ReadInt32(out int value)
        {
            value = reader.ReadInt32();
        }

        public void ReadUInt64(string name, out UInt64 value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            value = reader.ReadUInt64();
        }

        public void ReadUInt64(out UInt64 value)
        {
            value = reader.ReadUInt64();
        }

        public void ReadUInt32(string name, out uint value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            value = reader.ReadUInt32();
        }

        public void ReadUInt32(out uint value)
        {
            value = reader.ReadUInt32();
        }

        public void ReadPackedInt64(string name, out Int64 value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            value = (Int64)Read7BitEncodedUInt32();
            value = (Int64)((UInt64)value >> 1) ^ (-(value & 1));
        }

        public void ReadPackedInt64(out Int64 value)
        {
            value = (Int64)Read7BitEncodedUInt32();
            value = (Int64)((UInt64)value >> 1) ^ (-(value & 1));
        }
        
        private UInt64 Read7BitEncodedUInt64()
        {
            Int64 value = 0;
            int shift = 0;
            byte current = 0;
            do
            {
                current = reader.ReadByte();
                value |= (Int64)((current & 0x7f) << shift);
                shift += 7;
            }
            while ((shift <= 7*8) && ((current & 0x80) != 0));

            return (UInt64)value;
        }

        public void ReadPackedInt32(string name, out int value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            value = (int)Read7BitEncodedUInt32();
            value = (int)((uint)value >> 1) ^ (-(value & 1));
        }

        public void ReadPackedInt32(out int value)
        {
            value = (int)Read7BitEncodedUInt32();
            value = (int)((uint)value >> 1) ^ (-(value & 1));
        }
        
        private uint Read7BitEncodedUInt32()
        {
            int value = 0;
            int shift = 0;
            byte current = 0;
            do
            {
                current = reader.ReadByte();
                value |= (current & 0x7f) << shift;
                shift += 7;
            }
            while ((shift <= 7*4) && ((current & 0x80) != 0));
            
            return (uint)value;
        }

        public void ReadFloat(string name, out float value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            value = reader.ReadSingle();
        }

        public void ReadFloat(out float value)
        {
            throw new NotImplementedException();
        }

        public void ReadDouble(string name, out double value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            value = reader.ReadDouble();
        }

        public void ReadDouble(out double value)
        {
            value = reader.ReadDouble();
        }

        public void ReadString(string name, out string value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            value = reader.ReadString();
        }

        public void ReadString(out string value)
        {
            value = reader.ReadString();
        }

        public void ReadBase64(string name, out byte[] buffer)
        {
            ReadBytes(name, out buffer);
        }

        public void ReadBytes(string name, out byte[] value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            int count = reader.ReadInt32();
            value = reader.ReadBytes(count);
        }

        public void ReadVector2(string name, out Vector2 value)
        {
            throw new NotImplementedException();
        }

        public void ReadVector3(string name, out Vector3 value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            #if WP7
            value = new Vector3();
            #endif
            value.X = reader.ReadSingle();
            value.Y = reader.ReadSingle();
            value.Z = reader.ReadSingle();
        }
        
        public void ReadVector4(string name, out Vector4 value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
#if WP7
            value = new Vector4();
#endif
            value.X = reader.ReadSingle();
            value.Y = reader.ReadSingle();
            value.Z = reader.ReadSingle();
            value.W = reader.ReadSingle();
        }

        public void ReadMatrix(string name, out Matrix value)
        {
            throw new NotImplementedException();
        }

        public void ReadQuaternion(string name, out Quaternion value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            #if WP7
            value = new Quaternion();
            #endif
            value.X = reader.ReadSingle();
            value.Y = reader.ReadSingle();
            value.Z = reader.ReadSingle();
            value.W = reader.ReadSingle();
        }

        public void ReadColor(string name, out Color value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            value = new Color();
            value.PackedValue = reader.ReadUInt32();
        }

        public void ReadColor(out Color value)
        {
            throw new NotImplementedException();
        }

        public void ReadTimeSpan(string name, out TimeSpan value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            value = TimeSpan.FromTicks(reader.ReadInt64());
        }

        public void ReadGuid(out Guid value)
        {
            value = new Guid(reader.ReadBytes(16));
        }

        public void ReadList(string name, out List<string> values)
        {
            values = new List<string>();

            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string item = reader.ReadString();
                values.Add(item);
            }
        }

        public void ReadList(string name, out List<Vector2> values)
        {
            values = new List<Vector2>();

            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                Vector2 item;
                #if WP7
                item = new Vector2();
                #endif
                item.X = reader.ReadSingle();
                item.Y = reader.ReadSingle();
                values.Add(item);
            }
        }

        public void ReadList(string name, out List<Vector3> values)
        {
            values = new List<Vector3>();

            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                Vector3 item;
                #if WP7
                item = new Vector3();
                #endif
                item.X = reader.ReadSingle();
                item.Y = reader.ReadSingle();
                item.Z = reader.ReadSingle();
                values.Add(item);
            }
        }

        public void ReadList<T>(string name, out List<T> values) where T : IAetherSerialization
        {
            values = new List<T>();

            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                T item = (T)Activator.CreateInstance(typeof(T));
                item.Load(this);
                values.Add(item);
            }
        }

        public void ReadBoundingBox(string name, out BoundingBox value)
        {
            //string name2 = reader.ReadString();
            //System.Diagnostics.Debug.Assert(name == name2);
            #if WP7
            value = new BoundingBox();
            #endif
            value.Min.X = reader.ReadSingle();
            value.Min.Y = reader.ReadSingle();
            value.Min.Z = reader.ReadSingle();
            value.Max.X = reader.ReadSingle();
            value.Max.Y = reader.ReadSingle();
            value.Max.Z = reader.ReadSingle();
        }
    }
}