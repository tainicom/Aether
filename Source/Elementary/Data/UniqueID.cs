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
using tainicom.Aether.Elementary.Serialization;

namespace tainicom.Aether.Elementary.Data
{
    /*
    //[DataContract(Namespace = "")]
    public struct UniqueID : IComparable, IComparable<UniqueID>, IEquatable<UniqueID>, IFormattable,
        IAetherSerialization
    {
        //[DataMember]
        UInt64 uid;

        public static UniqueID Unknown { get { return new UniqueID(UInt64.MaxValue); } }
        public static UniqueID Initial { get { return new UniqueID(0); } }

        private UniqueID(UInt64 uid) { this.uid = uid; }

        internal UniqueID GetNext()
        {            
            UInt64 nextUid = uid + 1;
            if (nextUid == UInt64.MaxValue) throw new AetherException("Run out of Unique IDs");
            return new UniqueID(nextUid);
        }

        public int CompareTo(object obj) { return CompareTo((UniqueID)obj); }
        public int CompareTo(UniqueID other) { return uid.CompareTo(other.uid); }
        public bool Equals(UniqueID other) { return uid.Equals(other.uid); }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return uid.ToString("X16");
        }

        public void Save(IAetherWriter writer)
        {
            writer.WriteUInt64(uid);
        }

        public void Load(IAetherReader reader)
        {
            throw new NotImplementedException();
        }
    }
    */

    public struct UniqueID : IComparable, IComparable<UniqueID>, IEquatable<UniqueID>, IFormattable,
        IAetherSerialization
    {
        Guid uid;

        public static UniqueID Unknown { get { return new UniqueID(new Guid(-1, -1, -1, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF)); } }
        public static UniqueID Initial { get { return new UniqueID(new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)); } }

        private UniqueID(Guid uid) { this.uid = uid; }

        //TODO: make internal 
        public /*internal*/ UniqueID GetNext()
        {
            Guid nextUid = Guid.NewGuid();
            return new UniqueID(nextUid);
        }

        public int CompareTo(object obj) { return CompareTo((UniqueID)obj); }
        public int CompareTo(UniqueID other) { return uid.CompareTo(other.uid); }
        public bool Equals(UniqueID other) { return uid.Equals(other.uid); }
        public static bool operator ==(UniqueID a, UniqueID b) { return a.Equals(b); }
        public static bool operator !=(UniqueID a, UniqueID b) { return !a.Equals(b); }
        
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return uid.ToString("D");
        }

        public string ToString()
        {
            return uid.ToString("D");
        }

        public void Save(IAetherWriter writer)
        {
            writer.WriteGuid(uid);
        }

        public void Load(IAetherReader reader)
        {
            reader.ReadGuid(out uid);
        }
    }




}
