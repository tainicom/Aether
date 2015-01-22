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
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary.Data;
using tainicom.Aether.Elementary;
using tainicom.Aether.Engine;
using tainicom.Aether.Elementary.Managers;

namespace tainicom.Aether.Core.Serialization
{
    #if (WINDOWS)
    public class AetherXMLWriter : IAetherWriter
    {
        public readonly AetherEngine Engine;
        private Stream stream;
        private XmlWriter writer;
        
        HashSet<UniqueID> serialisedParticles = new HashSet<UniqueID>();


        public AetherXMLWriter(AetherEngine engine, Stream stream)
        {
            this.Engine = engine;
            this.stream = stream;
            writer = XmlWriter.Create(stream, GetXmlWriterSettings());
        }

        private XmlWriterSettings GetXmlWriterSettings()
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.NewLineOnAttributes = false;
            xmlWriterSettings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
            return xmlWriterSettings;
        }
        
        public void Close()
        {
            #if !NETFX_CORE
            writer.Close();
            #else
            writer.Dispose();
            #endif
        }
        
        public void Write(string name, IAetherSerialization value)
        {
            writer.WriteStartElement(name);
            value.Save(this);
            writer.WriteEndElement();
        }
        
        public void WriteParticles(string name, Dictionary<UniqueID, IAether> particles)
        {            
            writer.WriteStartElement(name);
            foreach (KeyValuePair<UniqueID, IAether> pair in particles)
            {
                IAether particle = pair.Value;
                UniqueID uid = pair.Key;
                WriteParticle(uid, particle);
            }
            writer.WriteEndElement();
        }
                
        public void WriteParticles(string name, IList<IAether> particles)
        {
            writer.WriteStartElement(name);
            foreach (IAether particle in particles)
            {
                UniqueID uid = Engine.GetParticleUID(particle);
                WriteParticle(uid, particle);
            }
            writer.WriteEndElement();
        }

        public void WriteParticle(string name, IAether particle)
        {
            writer.WriteStartElement(name);
            {
                UniqueID uid = Engine.GetParticleUID(particle);
                if (uid != UniqueID.Unknown) WriteParticle(uid, particle);
            }
            writer.WriteEndElement();
        }

        private void WriteParticle(UniqueID uid, IAether particle)
        {
            string particleName = Engine.GetParticleName(particle);

            if (serialisedParticles.Contains(uid))
            {
                writer.WriteStartElement("AetherParticleRef");
                writer.WriteStartAttribute("UID"); uid.Save(this); writer.WriteEndAttribute();
                if (particleName != String.Empty) writer.WriteAttributeString("Name", particleName);
                writer.WriteEndElement();
                return;
            }
            else
            {
                Type particleType = particle.GetType();
                if (!uid.Equals(UniqueID.Unknown))
                    serialisedParticles.Add(uid);
                writer.WriteStartElement("AetherParticle");
                writer.WriteStartAttribute("UID"); uid.Save(this); writer.WriteEndAttribute();
                if (particleName != String.Empty) writer.WriteAttributeString("Name", particleName);
                writer.WriteAttributeString("Type", particleType.AssemblyQualifiedName);
                IAetherSerialization serialisableParticle = particle as IAetherSerialization;
                if (serialisableParticle != null)
                    serialisableParticle.Save(this);
                writer.WriteEndElement();
            }

            return;
        }
        
        public void WriteParticleManagers(string name, List<Elementary.Managers.IAetherManager> particleManagers)
        {
            writer.WriteStartElement(name);
            foreach (IAetherManager manager in particleManagers)
            {                
                WriteParticleManager(manager);
            }
            writer.WriteEndElement();
        }

        private void WriteParticleManager(IAetherManager manager)
        {
            Type managerType = manager.GetType();

            writer.WriteStartElement("AetherParticleManager");
            writer.WriteAttributeString("Name", manager.Name);
            writer.WriteAttributeString("Type", managerType.AssemblyQualifiedName);
            IAetherSerialization serialisableParticle = manager as IAetherSerialization;
            if (serialisableParticle != null)
                serialisableParticle.Save(this);
            writer.WriteEndElement();
        }

        public void WriteBoolean(string name, bool value)
        {
            writer.WriteStartElement(name);
            WriteBoolean(value);
            writer.WriteEndElement();
        }
        private void WriteBoolean(bool value)
        {
            writer.WriteValue(value);
        }

        public void WriteInt64(string name, Int64 value)
        {
            writer.WriteStartElement(name);
            WriteInt64(value);
            writer.WriteEndElement();
        }
        public void WriteInt64(Int64 value)
        {
            writer.WriteValue((Int64)value);
        }
        
        public void WriteInt32(string name, Int32 value)
        {
            writer.WriteStartElement(name);
            WriteInt32(value);
            writer.WriteEndElement();
        }
        public void WriteInt32(Int32 value)
        {
            writer.WriteValue((Int32)value);
        }

        public void WriteUInt64(string name, UInt64 value)
        {
            writer.WriteStartElement(name);
            WriteUInt64(value);
            writer.WriteEndElement();
        }
        public void WriteUInt64(UInt64 value)
        {           
            //TODO: currently we convert UInt64 to Int64. We should write the unsigned value. 
            writer.WriteValue((Int64)value);
            //writer.WriteValue(Convert.ToString(value));
        }
        public void WriteFloat(string name, float value)
        {
            writer.WriteStartElement(name);
            WriteFloat(value);
            writer.WriteEndElement();
        }
        public void WriteFloat(float value)
        {
            writer.WriteValue(value);
        }
 
        public void WriteDouble(string name, double value)
        {
            writer.WriteStartElement(name);
            WriteDouble(value);
            writer.WriteEndElement();
        }
        public void WriteDouble(double value)
        {
            writer.WriteValue(value);
        }
        
        public void WriteString(string name, string value)
        {
            writer.WriteStartElement(name);
            WriteString(value);
            writer.WriteEndElement();
        }
        public void WriteString(string value)
        {
            writer.WriteString(value);
        }

        public void WriteBase64(string name, byte[] buffer)
        {
            writer.WriteStartElement(name);
            //writer.WriteBase64(buffer, 0, buffer.Length);
            string base64 = Convert.ToBase64String(buffer, Base64FormattingOptions.InsertLineBreaks);
            writer.WriteString(base64);            
            writer.WriteEndElement();
        }

        public void WriteBytes(string name, byte[] value)
        {
            writer.WriteStartElement(name);
            string base64 = Convert.ToBase64String(value, Base64FormattingOptions.InsertLineBreaks);
            writer.WriteString(base64);
            writer.WriteEndElement();
        }

        public void WriteVector2(string name, Vector2 value)
        {
            writer.WriteStartElement(name);
            writer.WriteStartAttribute("X"); writer.WriteValue(value.X); writer.WriteEndAttribute();
            writer.WriteStartAttribute("Y"); writer.WriteValue(value.Y); writer.WriteEndAttribute();            
            writer.WriteEndElement();
        }

        public void WriteVector3(string name, Vector3 value)
        {
            writer.WriteStartElement(name);
            writer.WriteStartAttribute("X"); writer.WriteValue(value.X); writer.WriteEndAttribute();
            writer.WriteStartAttribute("Y"); writer.WriteValue(value.Y); writer.WriteEndAttribute();
            writer.WriteStartAttribute("Z"); writer.WriteValue(value.Z); writer.WriteEndAttribute();
            writer.WriteEndElement();
        }

        public void WriteMatrix(string name, Matrix value)
        {
            writer.WriteStartElement(name);
            writer.WriteStartAttribute("M11"); writer.WriteValue(value.M11); writer.WriteEndAttribute();
            writer.WriteStartAttribute("M12"); writer.WriteValue(value.M12); writer.WriteEndAttribute();
            writer.WriteStartAttribute("M13"); writer.WriteValue(value.M13); writer.WriteEndAttribute();
            writer.WriteStartAttribute("M14"); writer.WriteValue(value.M14); writer.WriteEndAttribute();
            writer.WriteStartAttribute("M21"); writer.WriteValue(value.M21); writer.WriteEndAttribute();
            writer.WriteStartAttribute("M22"); writer.WriteValue(value.M22); writer.WriteEndAttribute();
            writer.WriteStartAttribute("M23"); writer.WriteValue(value.M23); writer.WriteEndAttribute();
            writer.WriteStartAttribute("M24"); writer.WriteValue(value.M24); writer.WriteEndAttribute();
            writer.WriteStartAttribute("M31"); writer.WriteValue(value.M31); writer.WriteEndAttribute();
            writer.WriteStartAttribute("M32"); writer.WriteValue(value.M32); writer.WriteEndAttribute();
            writer.WriteStartAttribute("M33"); writer.WriteValue(value.M33); writer.WriteEndAttribute();
            writer.WriteStartAttribute("M34"); writer.WriteValue(value.M34); writer.WriteEndAttribute();
            writer.WriteStartAttribute("M41"); writer.WriteValue(value.M41); writer.WriteEndAttribute();
            writer.WriteStartAttribute("M42"); writer.WriteValue(value.M42); writer.WriteEndAttribute();
            writer.WriteStartAttribute("M43"); writer.WriteValue(value.M43); writer.WriteEndAttribute();
            writer.WriteStartAttribute("M44"); writer.WriteValue(value.M44); writer.WriteEndAttribute();
            writer.WriteEndElement();
        }
        
        public void WriteQuaternion(string name, Quaternion value)
        {
            writer.WriteStartElement(name);
            writer.WriteStartAttribute("X"); writer.WriteValue(value.X); writer.WriteEndAttribute();
            writer.WriteStartAttribute("Y"); writer.WriteValue(value.Y); writer.WriteEndAttribute();
            writer.WriteStartAttribute("Z"); writer.WriteValue(value.Z); writer.WriteEndAttribute();
            writer.WriteStartAttribute("W"); writer.WriteValue(value.W); writer.WriteEndAttribute();
            writer.WriteEndElement();
        }

        public void WriteColor(string name, Color value)
        {
            writer.WriteStartElement(name);
            WriteColor(value);
            writer.WriteEndElement();
        }
        public void WriteColor(Color value)
        {
            if (value.A==255) writer.WriteString(String.Format("#{0:X2}{1:X2}{2:X2}", value.R, value.G, value.B));
            else writer.WriteString(String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", value.A, value.R, value.G, value.B));
        }

        public void WriteGuid(Guid uid)
        {
            writer.WriteValue(uid.ToString("D").ToUpper());
        }

        public void WriteTimeSpan(string name, TimeSpan value)
        {
            writer.WriteStartElement(name);
            writer.WriteValue(value.ToString());
            writer.WriteEndElement();
        }
        
        public void WriteList(string name, List<string> values)
        {
            writer.WriteStartElement(name);
            foreach (string value in values)
            {
                WriteString("Item", value);
            }
            writer.WriteEndElement();
        }

        public void WriteList(string name, List<Vector2> values)
        {
            writer.WriteStartElement(name);
            foreach (Vector2 value in values)
            {
                WriteVector2("Item", value);
            }
            writer.WriteEndElement();
        }

        public void WriteList(string name, List<Vector3> values)
        {
            writer.WriteStartElement(name);
            foreach (Vector3 value in values)
            {
                WriteVector3("Item", value);
            }
            writer.WriteEndElement();
        }

        public void WriteList<T>(string name, List<T> values) where T : IAetherSerialization
        {
            writer.WriteStartElement(name);
            foreach (IAetherSerialization value in values)
            {
                Write("Item", value);
            }
            writer.WriteEndElement();
        }
        
        public void WriteBoundingBox(string name, BoundingBox value)
        {
            writer.WriteStartElement(name);
            writer.WriteStartAttribute("MinX"); writer.WriteValue(value.Min.X); writer.WriteEndAttribute();
            writer.WriteStartAttribute("MinY"); writer.WriteValue(value.Min.Y); writer.WriteEndAttribute();
            writer.WriteStartAttribute("MinZ"); writer.WriteValue(value.Min.Z); writer.WriteEndAttribute();
            writer.WriteStartAttribute("MaxX"); writer.WriteValue(value.Max.X); writer.WriteEndAttribute();
            writer.WriteStartAttribute("MaxY"); writer.WriteValue(value.Max.Y); writer.WriteEndAttribute();
            writer.WriteStartAttribute("MaxZ"); writer.WriteValue(value.Max.Z); writer.WriteEndAttribute();
            writer.WriteEndElement();
        }
    }
    #endif
}
