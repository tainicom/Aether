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
using System.IO;
using tainicom.Aether.Elementary.Serialization;
using tainicom.Aether.Elementary.Data;
using tainicom.Aether.Engine;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Managers;
using Microsoft.Xna.Framework;

namespace tainicom.Aether.Core.Serialization
{
#if (WINDOWS)
    public class AetherBinaryWriter : IAetherWriter
    {
        public readonly AetherEngine Engine;
        private Stream stream;
        private BinaryWriter writer;

        HashSet<UniqueID> serialisedParticles = new HashSet<UniqueID>();
        Dictionary<Type, int> knownTypes = new Dictionary<Type, int>();

        public AetherBinaryWriter(AetherEngine engine, Stream stream)
        {
            this.Engine = engine;
            this.stream = stream;
            writer = new BinaryWriter(stream, Encoding.UTF8);
        }

        public void Close()
        {
            writer.Dispose();
        }

        public void Write(string name, IAetherSerialization value)
        {
            //writer.Write(name);
            value.Save(this);
        }

        public void WriteParticles<T>(string name, IDictionary<UniqueID, T> particles) where T : IAether
        {
            //writer.Write(name);
            writer.Write((Int32)particles.Count);
            foreach (KeyValuePair<UniqueID, T> pair in particles)
            {
                IAether particle = pair.Value;
                UniqueID uid = pair.Key;
                WriteParticle(uid, particle);
            }
        }

        public void WriteParticles<T>(string name, IList<T> particles) where T:IAether
        {
            //writer.Write(name); 
            writer.Write((Int32)particles.Count);
            foreach (IAether particle in particles)
            {
                UniqueID uid = Engine.GetParticleUID(particle);
                WriteParticle(uid, particle);
            }
        }

        public void WriteParticle(string name, IAether particle)
        {
            //writer.Write(name);
            UniqueID uid = Engine.GetParticleUID(particle);
            bool isParticleUnknown = uid == UniqueID.Unknown;
            writer.Write(isParticleUnknown);
            if (!isParticleUnknown)
                WriteParticle(uid, particle);
        }

        private void WriteParticle(UniqueID uid, IAether particle)
        {
            string particleName = Engine.GetParticleName(particle);
            
            uid.Save(this);

            bool isParticleSerialized = serialisedParticles.Contains(uid);
            writer.Write(isParticleSerialized);
            if (!isParticleSerialized)
            {
                if (!uid.Equals(UniqueID.Unknown))
                    serialisedParticles.Add(uid);

                writer.Write(particleName); //name
                WriteType(particle.GetType()); //type

                IAetherSerialization serialisableParticle = particle as IAetherSerialization;
                bool isSerialisableParticle = serialisableParticle != null;
                writer.Write(isSerialisableParticle); //mark whether Particle has data to serialize
                if (isSerialisableParticle)
                    serialisableParticle.Save(this);
            }

            return;
        }

        private void WriteType(Type particleType)
        {
            int typeIndex;
            if (knownTypes.TryGetValue(particleType, out typeIndex))
            {
                WriteInt32(typeIndex);
            }
            else
            {
                typeIndex = knownTypes.Count;
                knownTypes.Add(particleType, typeIndex);
                WriteInt32(typeIndex);
                string typeName = particleType.FullName + ", " + particleType.Assembly.GetName().Name;
                writer.Write(typeName);
            }
        }

        public void WriteParticleManagers(string name, IList<IAetherManager> particleManagers)
        {
            //writer.Write(name);
            writer.Write((Int32)particleManagers.Count);
            foreach (IAetherManager manager in particleManagers)
            {
                WriteParticleManager(manager);
            }
        }

        private void WriteParticleManager(IAetherManager manager)
        {
            writer.Write(manager.Name);
            Type managerType = manager.GetType();
            string typeName = managerType.FullName + ", " + managerType.Assembly.GetName().Name;
            writer.Write(typeName);

            IAetherSerialization serialisableParticle = manager as IAetherSerialization;
            bool isSerialisableParticle = serialisableParticle != null;
            writer.Write(isSerialisableParticle);
            if (isSerialisableParticle)
                serialisableParticle.Save(this);
        }

        public void WriteBoolean(string name, bool value)
        {
            //writer.Write(name);
            writer.Write(value);
        }

        public void WriteInt64(string name, long value)
        {
            //writer.Write(name);
            writer.Write((Int64)value);
        }

        public void WriteInt64(long value)
        {
            writer.Write((Int64)value);
        }

        public void WriteInt32(string name, int value)
        {
            //writer.Write(name);
            writer.Write((Int32)value);
        }

        public void WriteInt32(int value)
        {
            writer.Write((Int32)value);
        }

        public void WriteUInt64(string name, UInt64 value)
        {
            //writer.Write(name);
            writer.Write((UInt64)value);
        }

        public void WriteUInt64(UInt64 value)
        {
            writer.Write((UInt64)value);
        }

        public void WriteUInt32(string name, uint value)
        {
            //writer.Write(name);
            writer.Write((UInt32)value);
        }

        public void WriteUInt32(uint value)
        {
            writer.Write((UInt32)value);
        }

        public void WritePackedInt64(string name, Int64 value)
        {
            //writer.Write(name);
            value = ((value << 1) ^ (value >> 63));
            Write7BitEncodedUInt64((UInt64)value);
        }

        public void WritePackedInt64(Int64 value)
        {
            value = ((value << 1) ^ (value >> 63));
            Write7BitEncodedUInt64((UInt64)value);
        }

        private void Write7BitEncodedUInt64(UInt64 value)
        {
            while (value > 0x7f)
            {
                writer.Write((byte)(value | 0x80));
                value >>= 7;
            }
            writer.Write((byte)value);
        }

        public void WritePackedInt32(string name, int value)
        {
            //writer.Write(name);
            value = (value << 1) ^ (value >> 31);
            Write7BitEncodedUInt32((uint)value);
        }

        public void WritePackedInt32(int value)
        {
            value = (value << 1) ^ (value >> 31);
            Write7BitEncodedUInt32((uint)value);
        }

        private void Write7BitEncodedUInt32(uint value)
        {
            while (value > 0x7f)
            {
                writer.Write((byte)(value | 0x80));
                value >>= 7;
            }
            writer.Write((byte)value);
        }

        public void WriteFloat(string name, float value)
        {
            //writer.Write(name);
            writer.Write(value);
        }

        public void WriteFloat(float value)
        {
            writer.Write(value);
        }

        public void WriteDouble(string name, double value)
        {
            //writer.Write(name);
            writer.Write(value);
        }

        public void WriteDouble(double value)
        {
            writer.Write(value);
        }

        public void WriteString(string name, string value)
        {
            //writer.Write(name);
            if (value == null) writer.Write(string.Empty);
            else writer.Write(value);
        }

        public void WriteString(string value)
        {
            writer.Write(value);
        }

        public void WriteBase64(string name, byte[] buffer)
        {
            WriteBytes(name, buffer);
        }

        public void WriteBytes(string name, byte[] value)
        {    
            //writer.Write(name);
            writer.Write(value.Length);
            writer.Write(value);
        }

        public void WriteVector2(string name, Vector2 value)
        {
            throw new NotImplementedException();
        }

        public void WriteVector3(string name, Vector3 value)
        {
            //writer.Write(name);
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
        }

        public void WriteVector4(string name, Vector4 value)
        {
            //writer.Write(name);
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
            writer.Write(value.W);
        }

        public void WriteMatrix(string name, Matrix value)
        {
            throw new NotImplementedException();
        }

        public void WriteQuaternion(string name, Quaternion value)
        {
            //writer.Write(name);
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
            writer.Write(value.W);
        }

        public void WriteColor(string name, Color value)
        {
            //writer.Write(name);
            writer.Write((UInt32)value.PackedValue);
        }

        public void WriteColor(Color value)
        {
            throw new NotImplementedException();
        }

        public void WriteTimeSpan(string name, TimeSpan value)
        {
            //writer.Write(name);
            writer.Write((Int64)value.Ticks);
        }

        public void WriteGuid(Guid uid)
        {
            writer.Write(uid.ToByteArray());
        }

        public void WriteList(string name, IList<string> values)
        {            
            //writer.Write(name);
            writer.Write(values.Count);
            foreach (string value in values)
            {
                writer.Write(value);
            }
        }

        public void WriteList(string name, IList<Vector2> values)
        {
            //writer.Write(name);
            writer.Write(values.Count);
            foreach (Vector2 value in values)
            {
                writer.Write(value.X);
                writer.Write(value.Y);
            }
        }

        public void WriteList(string name, IList<Vector3> values)
        {
            //writer.Write(name);
            writer.Write(values.Count);
            foreach (Vector3 value in values)
            {
                writer.Write(value.X);
                writer.Write(value.Y);
                writer.Write(value.Z);
            }
        }

        public void WriteList<T>(string name, IList<T> values) where T : IAetherSerialization
        {
            //writer.Write(name);
            writer.Write(values.Count);
            foreach (IAetherSerialization value in values)
            {
                value.Save(this);
            }
        }

        public void WriteBoundingBox(string name, BoundingBox BoundingBox)
        {
            //writer.Write(name);
            writer.Write(BoundingBox.Min.X);
            writer.Write(BoundingBox.Min.Y);
            writer.Write(BoundingBox.Min.Z);
            writer.Write(BoundingBox.Max.X);
            writer.Write(BoundingBox.Max.Y);
            writer.Write(BoundingBox.Max.Z);
        }
    }
#endif
}
