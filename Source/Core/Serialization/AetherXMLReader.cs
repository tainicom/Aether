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
using tainicom.Aether.Elementary.Serialization;
using System.IO;
using System.Xml;
using tainicom.Aether.Engine;
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Data;
using tainicom.Aether.Elementary.Managers;

namespace tainicom.Aether.Core.Serialization
{   
    public class AetherXMLReader : IAetherReader
    {
        public IAetherTypeResolver TypeResolver { get; set; }

        public readonly AetherEngine Engine;
        private Stream stream;
        private XmlReader reader;
        
        Dictionary<UniqueID,IAether> deserialisedParticles = new Dictionary<UniqueID,IAether>();
        
        public AetherXMLReader(AetherEngine engine, Stream stream)
        {
            this.Engine = engine;
            this.stream = stream;
            reader = XmlReader.Create(stream);
        } 
        
        public void Close()
        {
            #if WINDOWS
            reader.Close();
			#else
            reader.Dispose();
			#endif
        }

        public void Read(string name, IAetherSerialization value)
        {
            reader.ReadStartElement(name);
            value.Load(this);
            reader.ReadEndElement();
        }
        
        public void ReadParticles<T>(string name, IDictionary<Elementary.Data.UniqueID, T> particles) where T:IAether
        {
            reader.ReadToFollowing(name);
            if (reader.IsEmptyElement) { reader.Read(); return; }
            
            reader.ReadStartElement(name);
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    UniqueID uid;
                    IAether particle;
                    ReadParticle(out uid, out particle);
                    if (particle == null) throw new Exception("Can't read particle.");
                    particles.Add(uid,(T)particle);
                }
            }
            reader.ReadEndElement();
        }

        public void ReadParticles<T>(string name, IList<T> particles) where T : IAether
        {
            reader.ReadToFollowing(name);
            if (reader.IsEmptyElement) { reader.Read(); return; }

            reader.ReadStartElement(name);
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    UniqueID uid;
                    IAether particle;
                    ReadParticle(out uid, out particle);
                    particles.Add((T)particle);
                }
            }
            reader.ReadEndElement();
        }

        public void ReadParticle(string name, out IAether particle)
        {
            particle = null;

            //reader.ReadStartElement(name);
            //UniqueID uid;
            //ReadParticle(out uid, out particle);
            //reader.ReadEndElement();

            reader.ReadToFollowing(name);
            if (reader.IsEmptyElement) { reader.Read(); return; }

            reader.ReadStartElement(name);
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    UniqueID uid;
                    ReadParticle(out uid, out particle);
                    if (particle == null) throw new Exception("Cant read particle.");
                    break;
                }
            }
            reader.ReadEndElement();
        }

        private void ReadParticle(out UniqueID uid, out IAether particle)
        {
            uid = new UniqueID();
            particle = null;
            string elementName = reader.Name;

            //read attribute
            string particleName = string.Empty;
            string typeName = string.Empty;
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "UID": uid.Load(this);
                        break;
                    case "Name": particleName = reader.ReadContentAsString();
                        break;
                    case "Type": typeName = reader.ReadContentAsString();
                        break;
                }
            }
            reader.MoveToElement();
            bool isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();

            if (elementName == "AetherParticle")
            {
                if (Engine.ContainsName(particleName))
                {
                    //TOD: is this even possible?
                    //isn't it a bug to deserialize an object using a name for cache key?
                    //System.Diagnostics.Debug.Assert(false);
                    particle = Engine[particleName];
                }
                else
                {
                    Type particleType = TypeResolver.ResolveType(typeName);
                    particle = (IAether)Activator.CreateInstance(particleType);
                }
                
                if (!uid.Equals(UniqueID.Unknown))
                    deserialisedParticles.Add(uid, particle);

                //particle = (IAetherParticle)FormatterServices.GetUninitializedObject(particleType); //this behaves the same the build in Serialisation.
                IAetherSerialization serialisableParticle = particle as IAetherSerialization;
                if (serialisableParticle != null)
                    serialisableParticle.Load(this);

                particle = TypeResolver.Convert(particle);
                deserialisedParticles[uid] = particle; // update converted particle
                
                if(particleName!=string.Empty)
                    Engine.SetParticleName(particle, particleName);

                if (!isEmptyElement) reader.ReadEndElement();
            }
            else if (elementName == "AetherParticleRef")
            {
                particle = deserialisedParticles[uid];
            }

            return;
        }

        public void ReadParticleManagers(string name, IList<IAetherManager> particleManagers)
        {
            reader.ReadStartElement(name);
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    ReadParticleManager(particleManagers);
                }
            }
            //reader.ReadEndElement();
        }

        private void ReadParticleManager(IList<IAetherManager> particleManagers)
        {   
            string elementName = reader.Name;

            //read attribute
            string managerName = string.Empty;
            string type = string.Empty;
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Name": managerName = reader.ReadContentAsString();
                        break;
                    case "Type": type = reader.ReadContentAsString();
                        break;
                }
            }
            reader.MoveToElement();
            bool isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();

            if (elementName == "AetherParticleManager")
            {
                IAetherManager manager = null;
                foreach (IAetherManager mgr in particleManagers)
                    if (mgr.Name == managerName) { manager = mgr; break; }
                if (manager == null)
                {
                    manager = (IAetherManager)Activator.CreateInstance(Type.GetType(type, false));
                    particleManagers.Add(manager);
                    if (manager is IInitializable)
                        ((IInitializable)manager).Initialize(this.Engine);
                }
                IAetherSerialization serialisableParticle = manager as IAetherSerialization;
                if (serialisableParticle != null)
                    serialisableParticle.Load(this);
                if (!isEmptyElement) reader.ReadEndElement();
            }

            return;
        }
        
        public void ReadBoolean(string name, out bool value)
        {
            reader.ReadStartElement(name);
            ReadBoolean(out value);
            reader.ReadEndElement();
        }
        private void ReadBoolean(out bool value)
        {
            value = reader.ReadContentAsBoolean();
        }

        public void ReadInt64(string name, out Int64 value)
        {
            reader.ReadStartElement(name);
            ReadInt64(out value);
            reader.ReadEndElement();
        }
        public void ReadInt64(out Int64 value)
        {
            value = reader.ReadContentAsLong();
        }
        
        public void ReadInt32(string name, out int value)
        {
            reader.ReadStartElement(name);
            ReadInt32(out value);
            reader.ReadEndElement();
        }
        public void ReadInt32(out int value)
        {
            value = reader.ReadContentAsInt();
        }

        public void ReadUInt64(string name, out UInt64 value)
        {
            reader.ReadStartElement(name);
            ReadUInt64(out value);
            reader.ReadEndElement();
        }
        public void ReadUInt64(out UInt64 value)
        {   
            //TODO: currently we convert Int64 to UInt64. We should read the unsigned value.
            value = (UInt64)reader.ReadContentAsLong();
            //value = Convert.ToUInt64(reader.Value);
        }
        
        public void ReadUInt32(string name, out uint value)
        {
            reader.ReadStartElement(name);
            ReadUInt32(out value);
            reader.ReadEndElement();
        }

        public void ReadUInt32(out uint value)
        {
            value = (uint)reader.ReadContentAsInt();
        }

        public void ReadPackedInt64(string name, out Int64 value)
        {
            reader.ReadStartElement(name);
            ReadPackedInt64(out value);
            reader.ReadEndElement();
        }

        public void ReadPackedInt64(out Int64 value)
        {
            ReadInt64(out value);
        }

        public void ReadPackedInt32(string name, out int value)
        {
            reader.ReadStartElement(name);
            ReadPackedInt32(out value);
            reader.ReadEndElement();
        }

        public void ReadPackedInt32(out int value)
        {
            ReadInt32(out value);
        }

        public void ReadFloat(string name, out float value)
        {
            reader.ReadStartElement(name);
            ReadFloat(out value);
            reader.ReadEndElement();
        }
        public void ReadFloat(out float value)
        {            
            value = reader.ReadContentAsFloat();
        }

        public void ReadDouble(string name, out double value)
        {
            reader.ReadStartElement(name);
            ReadDouble(out value);
            reader.ReadEndElement();
        }
        public void ReadDouble(out double value)
        {
            value = reader.ReadContentAsDouble();
        }

        public void ReadString(string name, out string value)
        {
            while (!reader.IsStartElement())
                reader.Read();

            if (reader.Name != name)
                throw new XmlException(String.Format("Element '{0}' was not found", name));

            reader.MoveToElement();
            if (reader.IsEmptyElement)
            { 
                reader.Read();
                value = null;
                return;
            }

            reader.ReadStartElement(name);
            ReadString(out value);
            reader.ReadEndElement();
        }
        public void ReadString(out string value)
        {
            value = reader.ReadContentAsString();
        }

        public void ReadBase64(string name, out byte[] buffer)
        {
            reader.ReadStartElement(name);
            //byte[] tmp = new byte[1024];
            //using(MemoryStream ms = new MemoryStream())
            //{
            //    int r;
            //    while ( (r = reader.ReadContentAsBase64(tmp, 0, tmp.Length)) > 0)
            //    {
            //        ms.Write(tmp, 0, r);
            //    }
            //    buffer = ms.ToArray();
            //}
            string base64 = reader.ReadContentAsString();
            buffer = Convert.FromBase64String(base64);
            reader.ReadEndElement();
            return;
        }

        public void ReadBytes(string name, out byte[] value)
        {
            reader.ReadStartElement(name);
            string base64 = reader.ReadContentAsString();
            value = Convert.FromBase64String(base64);
            reader.ReadEndElement();
        }

        public void ReadVector2(string name, out Vector2 value)
        {
            value = Vector2.Zero; // assign a default value because 'value' is 'out' modifier 

            while (!reader.IsStartElement())
                reader.Read();

            if (reader.Name != name)
                throw new XmlException(String.Format("Element '{0}' was not found", name));
            //read attribute
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "X": value.X = reader.ReadContentAsFloat();
                        break;
                    case "Y": value.Y = reader.ReadContentAsFloat();
                        break;
                }
            }

            reader.MoveToElement();
            if (reader.IsEmptyElement) reader.Read();
            else reader.ReadEndElement();
        }

        public void ReadVector3(string name, out Vector3 value)
        {
            value = Vector3.Zero; // assign a default value because 'value' is 'out' modifier 

            while (!reader.IsStartElement())
                reader.Read();

            if (reader.Name != name)
                throw new XmlException(String.Format("Element '{0}' was not found", name));
            //read attribute
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "X": value.X = reader.ReadContentAsFloat(); 
                        break;
                    case "Y": value.Y = reader.ReadContentAsFloat();
                        break;
                    case "Z": value.Z = reader.ReadContentAsFloat();
                        break;
                }
            }

            reader.MoveToElement();
            if (reader.IsEmptyElement) reader.Read();
            else reader.ReadEndElement();
        }
        
        public void ReadVector4(string name, out Vector4 value)
        {
            value = Vector4.Zero; // assign a default value because 'value' is 'out' modifier 

            while (!reader.IsStartElement())
                reader.Read();

            if (reader.Name != name)
                throw new XmlException(String.Format("Element '{0}' was not found", name));
            //read attribute
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "X": value.X = reader.ReadContentAsFloat();
                        break;
                    case "Y": value.Y = reader.ReadContentAsFloat();
                        break;
                    case "Z": value.Z = reader.ReadContentAsFloat();
                        break;
                    case "W": value.W = reader.ReadContentAsFloat();
                        break;
                }
            }

            reader.MoveToElement();
            if (reader.IsEmptyElement) reader.Read();
            else reader.ReadEndElement();
        }
        
        public void ReadMatrix(string name, out Matrix value)
        {
            value = Matrix.Identity; // assign a default value because 'value' is 'out' modifier 

            while (!reader.IsStartElement())
                reader.Read();

            if (reader.Name != name)
                throw new XmlException(String.Format("Element '{0}' was not found", name));
            //read attribute
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "M11": value.M11 = reader.ReadContentAsFloat();
                        break;
                    case "M12": value.M12 = reader.ReadContentAsFloat();
                        break;
                    case "M13": value.M13 = reader.ReadContentAsFloat();
                        break;
                    case "M14": value.M14 = reader.ReadContentAsFloat();
                        break;
                    case "M21": value.M11 = reader.ReadContentAsFloat();
                        break;
                    case "M22": value.M22 = reader.ReadContentAsFloat();
                        break;
                    case "M23": value.M23 = reader.ReadContentAsFloat();
                        break;
                    case "M24": value.M24 = reader.ReadContentAsFloat();
                        break;
                    case "M31": value.M31 = reader.ReadContentAsFloat();
                        break;
                    case "M32": value.M32 = reader.ReadContentAsFloat();
                        break;
                    case "M33": value.M33 = reader.ReadContentAsFloat();
                        break;
                    case "M34": value.M34 = reader.ReadContentAsFloat();
                        break;
                    case "M41": value.M41 = reader.ReadContentAsFloat();
                        break;
                    case "M42": value.M42 = reader.ReadContentAsFloat();
                        break;
                    case "M43": value.M43 = reader.ReadContentAsFloat();
                        break;
                    case "M44": value.M44 = reader.ReadContentAsFloat();
                        break;
                }
            }

            reader.MoveToElement();
            if (reader.IsEmptyElement) reader.Read();
            else reader.ReadEndElement();
        }

        public void ReadQuaternion(string name, out Quaternion value)
        {
            value = Quaternion.Identity; // assign a default value because 'value' is 'out' modifier 

            while (!reader.IsStartElement())
                reader.Read();

            if (reader.Name != name)
                throw new XmlException(String.Format("Element '{0}' was not found", name));
            //read attribute
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "X": value.X = reader.ReadContentAsFloat();
                        break;
                    case "Y": value.Y = reader.ReadContentAsFloat();
                        break;
                    case "Z": value.Z = reader.ReadContentAsFloat();
                        break;
                    case "W": value.W = reader.ReadContentAsFloat();
                        break;
                }
            }

            reader.MoveToElement();
            if (reader.IsEmptyElement) reader.Read();
            else reader.ReadEndElement();
        }
        
        public void ReadColor(string name, out Color value)
        {
            reader.ReadStartElement(name);
            ReadColor(out value);
            reader.ReadEndElement();
        }
        public void ReadColor(out Color value)
        {   
            string val = reader.ReadContentAsString();
            val.Trim(); 
            int idx = 0;
            int r, g, b, a;
   
            if (val[idx] == '#') 
            {
                idx++;
                if ((val.Length-idx) == 8)
                {
                    a = int.Parse(val.Substring(idx, 2), System.Globalization.NumberStyles.HexNumber);
                    idx += 2;
                } else a = 255;
                r = int.Parse(val.Substring(idx + 0, 2), System.Globalization.NumberStyles.HexNumber);
                g = int.Parse(val.Substring(idx + 2, 2), System.Globalization.NumberStyles.HexNumber);
                b = int.Parse(val.Substring(idx + 4, 2), System.Globalization.NumberStyles.HexNumber);
            }
            else throw new Exception("Can't read color. Unsuported format.");
            
            value = new Color(r,g,b,a);
        }

        public void ReadGuid(out Guid value)
        {            
            value = Guid.Parse(reader.ReadContentAsString());
        }

        public void ReadTimeSpan(string name, out TimeSpan value)
        {
            reader.ReadStartElement(name);
            value = TimeSpan.Parse(reader.ReadContentAsString());
            reader.ReadEndElement();
        }

        public void ReadList(string name, out List<string> values)
        {
            values = new List<String>();

            reader.ReadToFollowing(name);
            if (reader.IsEmptyElement) { reader.Read(); return; }

            reader.ReadStartElement(name);
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                string item;
                ReadString("Item", out item);
                values.Add(item);
                while (reader.NodeType == XmlNodeType.Whitespace) reader.Read();
            }
            reader.ReadEndElement();
        }

        public void ReadList(string name, out List<Vector2> values)
        {
            values = new List<Vector2>();

            reader.ReadToFollowing(name);
            if (reader.IsEmptyElement) { reader.Read(); return; }

            reader.ReadStartElement(name);
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                Vector2 item;
                ReadVector2("Item", out item);
                values.Add(item);
                while (reader.NodeType == XmlNodeType.Whitespace) reader.Read();
            }
            reader.ReadEndElement();
        }

        public void ReadList(string name, out List<Vector3> values)
        {
            values = new List<Vector3>();

            reader.ReadToFollowing(name);
            if (reader.IsEmptyElement) { reader.Read(); return; }

            reader.ReadStartElement(name);
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                Vector3 item;
                ReadVector3("Item", out item);
                values.Add(item);
                while (reader.NodeType == XmlNodeType.Whitespace) reader.Read();
            }
            reader.ReadEndElement();
        }

        public void ReadList<T>(string name, out List<T> values) where T : IAetherSerialization
        {
            values = new List<T>();

            reader.ReadStartElement(name);
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                T item = (T)Activator.CreateInstance(typeof(T));
                Read("Item", (IAetherSerialization)item);
                values.Add(item);
                while (reader.NodeType == XmlNodeType.Whitespace) reader.Read();
            }
            reader.ReadEndElement();
        }
        
        public void ReadBoundingBox(string name, out BoundingBox value)
        {
            value = new BoundingBox(); // assign a default value because 'value' is 'out' modifier 

            while (!reader.IsStartElement())
                reader.Read();

            if (reader.Name != name)
                throw new XmlException(String.Format("Element '{0}' was not found", name));
            //read attribute
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "MinX": value.Min.X = reader.ReadContentAsFloat();
                        break;
                    case "MinY": value.Min.Y = reader.ReadContentAsFloat();
                        break;
                    case "MinZ": value.Min.Z = reader.ReadContentAsFloat();
                        break;
                    case "MaxX": value.Max.X = reader.ReadContentAsFloat();
                        break;
                    case "MaxY": value.Max.Y = reader.ReadContentAsFloat();
                        break;
                    case "MaxZ": value.Max.Z = reader.ReadContentAsFloat();
                        break;
                }
            }

            reader.MoveToElement();
            if (reader.IsEmptyElement) reader.Read();
            else reader.ReadEndElement();
        }

    }
}
