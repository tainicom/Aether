﻿#region License
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
    public interface IAetherWriter
    {
        void Close();

        void Write(string name, IAetherSerialization value);
        void WriteParticle(string name, IAether particle);
        void WriteParticles(string name, Dictionary<Data.UniqueID, IAether> particles);
        void WriteParticles(string name, IList<IAether> particles);
        
        void WriteParticleManagers(string name, List<IAetherManager> particleManagers);
        
        void WriteBoolean(string name, bool value);
        void WriteInt64(string name, Int64 value);
        void WriteInt64(Int64 value);
        void WriteInt32(string name, int value);
        void WriteInt32(int value);
        void WriteUInt64(string name, UInt64 value);
        void WriteUInt64(UInt64 value);
        void WriteFloat(string name, float value);
        void WriteFloat(float value);
        void WriteDouble(string name, double value);
        void WriteDouble(double value);
        void WriteString(string name, string value);
        void WriteString(string value);

        [Obsolete("Use WriteBytes. Base64 will be used for text serializers.")]
        void WriteBase64(string name, byte[] buffer);

        void WriteBytes(string name, byte[] value);

        void WriteVector2(string name, Vector2 value);
        void WriteVector3(string name, Vector3 value);
        void WriteMatrix(string name, Matrix value);
        void WriteQuaternion(string name, Quaternion value);

        void WriteColor(string name, Color value);
        void WriteColor(Color value);
        void WriteTimeSpan(string name, TimeSpan value);
        void WriteGuid(Guid uid);

        void WriteList(string name, List<string> values);
        void WriteList(string name, List<Vector2> values);
        void WriteList(string name, List<Vector3> values);
        void WriteList<T>(string name, List<T> values) where T : IAetherSerialization;


        void WriteBoundingBox(string name, BoundingBox BoundingBox);

        
    }
}