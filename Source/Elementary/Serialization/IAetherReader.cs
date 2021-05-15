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
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary.Managers;

namespace tainicom.Aether.Elementary.Serialization
{    
    public interface IAetherReader
    {
        IAetherTypeResolver TypeResolver { get; set; }
        void Close();

        void Read(string name, IAetherSerialization value);
        void ReadParticle(string name, out IAether particle);
        void ReadParticles<T>(string name, IDictionary<Data.UniqueID, T> particles) where T : IAether;
        void ReadParticles<T>(string name, IList<T> particles) where T : IAether;

        void ReadParticleManagers(string name, IList<IAetherManager> particleManagers);
        
        void ReadBoolean(string name, out bool value);
        void ReadInt64(string name, out Int64 value);
        void ReadInt64(out Int64 value);
        void ReadInt32(string name, out int value);
        void ReadInt32(out int value);
        void ReadUInt64(string name, out UInt64 value);
        void ReadUInt64(out UInt64 value);
        void ReadUInt32(string name, out uint value);
        void ReadUInt32(out uint value);
        void ReadPackedInt64(string name, out Int64 value);
        void ReadPackedInt64(out Int64 value);
        void ReadPackedInt32(string name, out int value);
        void ReadPackedInt32(out int value);
        void ReadFloat(string name, out float value);
        void ReadFloat(out float value);
        void ReadDouble(string name, out double value);
        void ReadDouble(out double value);
        void ReadString(string name, out string value);
        void ReadString(out string value);

        [Obsolete("Use ReadBytes. Base64 will be used for text serializers.")]
        void ReadBase64(string name, out byte[] buffer);
        
        void ReadBytes(string name, out byte[] value);

        void ReadVector2(string name, out Vector2 value);
        void ReadVector3(string name, out Vector3 value);
        void ReadVector4(string name, out Vector4 value);
        void ReadMatrix(string name, out Matrix value);
        void ReadQuaternion(string name, out Quaternion value);

        void ReadColor(string name, out Color value);
        void ReadColor(out Color value);
        void ReadTimeSpan(string name, out TimeSpan value);

        void ReadGuid(out Guid value);

        void ReadList(string name, out List<string> values);
        void ReadList(string name, out List<Vector2> values);
        void ReadList(string name, out List<Vector3> values);
        void ReadList<T>(string name, out List<T> values) where T : IAetherSerialization;
        
        void ReadBoundingBox(string name, out BoundingBox value);

    }
}
