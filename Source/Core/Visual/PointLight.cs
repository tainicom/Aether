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

#if WINDOWS
using tainicom.Aether.Design.Converters;
#endif
using System.ComponentModel;
using tainicom.Aether.Elementary.Visual;
using tainicom.Aether.Elementary.Serialization;
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary.Spatial;

namespace tainicom.Aether.Core.Visual
{
    public class PointLight : ILightSource, IPosition, IAetherSerialization
    {
        public Vector3 Position { get; set; }
        
        #if WINDOWS
        [Category("Light"), TypeConverter(typeof(Vector3EditAsColorConverter))]
        #endif
        public Vector3 LightSourceColor { get; set; }

        #if WINDOWS
        [Category("Light")]
        #endif
        public float Intensity { get; set; }

        #if WINDOWS
        [Category("Light")]
        #endif
        public float MaximumRadius { get; set; }

        public Vector3 PremultiplyColor { get { return this.LightSourceColor * this.Intensity; } }
        
        public PointLight(): base()
        {
            LightSourceColor = Vector3.One;
            Intensity = 1;
            MaximumRadius = float.MaxValue;
        }
        
        public virtual void Save(IAetherWriter writer)
        {
            writer.WriteVector3("Position", Position);
            writer.WriteVector3("LightSourceColor", LightSourceColor);
            writer.WriteFloat("Intensity", Intensity);
            writer.WriteFloat("MaximumRadius", MaximumRadius);
        }

        public virtual void Load(IAetherReader reader)
        {
            Vector3 v3; float f;
            reader.ReadVector3("Position", out v3); Position = v3;
            reader.ReadVector3("LightSourceColor", out v3); LightSourceColor = v3;
            reader.ReadFloat("Intensity", out f); Intensity = f;
            reader.ReadFloat("MaximumRadius", out f); MaximumRadius = f;
        }        
    }
}
