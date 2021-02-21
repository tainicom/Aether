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

#if (WINDOWS)
using tainicom.Aether.Design.Converters;
#endif
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using tainicom.Aether.Core.Materials.Data;
using tainicom.Aether.Elementary.Serialization;

namespace tainicom.Aether.Core.Materials
{
    public class BasicMaterial : MaterialBase
    {
        //TODO: we must hide Effect class completly.
        public BasicEffect Effect { get { return (BasicEffect)_effect; } } 
               
        #if (WINDOWS)
        [Category("Lighting")]
        [TypeConverter(typeof(Vector3EditAsColorConverter))]
        #endif
        public Vector3 DiffuseColor { get; set; }
        #if (WINDOWS)
        [Category("Lighting")]
        [TypeConverter(typeof(Vector3EditAsColorConverter))]
        #endif
        public Vector3 AmbientLightColor { get; set; }
        #if (WINDOWS)
        [Category("Lighting")]
        [TypeConverter(typeof(Vector3EditAsColorConverter))]
        #endif
        public Vector3 EmissiveColor { get; set; }

        #if (WINDOWS)
        [Category("Lighting")]
        #endif
        public DirectionalLightData DirectionalLight0 { get; set; }
        #if (WINDOWS)
        [Category("Lighting")]
        #endif
        public DirectionalLightData DirectionalLight1 { get; set; }
        #if (WINDOWS)
        [Category("Lighting")]
        #endif
        public DirectionalLightData DirectionalLight2 { get; set; }

        #if (WINDOWS)
        [Category("Lighting")]
        #endif
        public float Alpha { get; set; }
        
        #if (WINDOWS)
        [Category("Basic")]
        #endif
        public bool TextureEnabled { get; set; }
        #if (WINDOWS)
        [Category("Basic")]
        #endif
        public bool VertexColorEnabled { get; set; }
        #if (WINDOWS)
        [Category("Basic")]
        #endif
        public bool LightingEnabled { get; set; }

        public BasicMaterial():base()
        {
            DiffuseColor = Vector3.One;
            AmbientLightColor = Vector3.Zero;
            EmissiveColor = Vector3.Zero;
            DirectionalLight0 = new DirectionalLightData() { Enabled=true };
            DirectionalLight1 = new DirectionalLightData();
            DirectionalLight2 = new DirectionalLightData();

            //default lighting
            DirectionalLight0.Direction = new Vector3(-0.5265408f, -0.5735765f, -0.6275069f);
            DirectionalLight0.DiffuseColor = new Vector3(1, 0.9607844f, 0.8078432f);
            DirectionalLight0.SpecularColor = new Vector3(1, 0.9607844f, 0.8078432f);
            //DirectionalLight0.Enabled = true;
            DirectionalLight1.Direction = new Vector3(0.7198464f, 0.3420201f, 0.6040227f);
            DirectionalLight1.DiffuseColor = new Vector3(0.9647059f, 0.7607844f, 0.4078432f);
            DirectionalLight1.SpecularColor = Vector3.Zero;
            //DirectionalLight1.Enabled = true;
            DirectionalLight2.Direction = new Vector3(0.4545195f, -0.7660444f, 0.4545195f);
            DirectionalLight2.DiffuseColor = new Vector3(0.3231373f, 0.3607844f, 0.3937255f);
            DirectionalLight2.SpecularColor = new Vector3(0.3231373f, 0.3607844f, 0.3937255f);
            //DirectionalLight2.Enabled = true;
            AmbientLightColor = new Vector3(0.05333332f, 0.09882354f, 0.1819608f);

            Alpha = 1f;
        }
      
        protected override void CreateEffect()
        {
            this._effect = new BasicEffect(this.GraphicsDevice);
        }

        public override void Apply()
        {
            Effect.DiffuseColor = DiffuseColor;
            Effect.AmbientLightColor = AmbientLightColor;
            Effect.EmissiveColor = EmissiveColor;
            DirectionalLight0.CopyTo(Effect.DirectionalLight0);
            DirectionalLight1.CopyTo(Effect.DirectionalLight1);
            DirectionalLight2.CopyTo(Effect.DirectionalLight2);
            Effect.Alpha = Alpha;

            Effect.TextureEnabled = TextureEnabled;
            Effect.VertexColorEnabled = VertexColorEnabled;
            Effect.LightingEnabled = LightingEnabled;
            
            base.Apply();
        }
        
        #region Aether.Elementary.Serialization.IAetherSerialization Members

        public override void Save(IAetherWriter writer)
        {
            base.Save(writer);
            writer.WriteVector3("DiffuseColor", DiffuseColor);
            writer.WriteVector3("AmbientLightColor", AmbientLightColor); 
            writer.WriteVector3("EmissiveColor", EmissiveColor);
            WriteDirectionalLight(writer, "DirectionalLight0", DirectionalLight0);
            WriteDirectionalLight(writer, "DirectionalLight1", DirectionalLight1);
            WriteDirectionalLight(writer, "DirectionalLight2", DirectionalLight2);
            writer.WriteFloat("Alpha", Alpha);
            writer.WriteBoolean("TextureEnabled", TextureEnabled);
            writer.WriteBoolean("VertexColorEnabled", VertexColorEnabled);
            writer.WriteBoolean("LightingEnabled", LightingEnabled);
        }
        private void WriteDirectionalLight(IAetherWriter writer, string name, DirectionalLightData directionalLight)
        {
            writer.WriteVector3(name + "DiffuseColor", directionalLight.DiffuseColor);
            writer.WriteVector3(name + "SpecularColor", directionalLight.SpecularColor);
            writer.WriteVector3(name + "Direction", directionalLight.Direction);
            writer.WriteBoolean(name + "Enabled", directionalLight.Enabled);
        }

        public override void Load(IAetherReader reader)
        {
            base.Load(reader);
            Vector3 vctr3; float flt; bool bl;
            reader.ReadVector3("DiffuseColor", out vctr3); DiffuseColor = vctr3;
            reader.ReadVector3("AmbientLightColor", out vctr3); AmbientLightColor = vctr3;
            reader.ReadVector3("EmissiveColor", out vctr3); EmissiveColor = vctr3;
            DirectionalLight0 = ReadDirectionalLight(reader, "DirectionalLight0");
            DirectionalLight1 = ReadDirectionalLight(reader, "DirectionalLight1");
            DirectionalLight2 = ReadDirectionalLight(reader, "DirectionalLight2");
            reader.ReadFloat("Alpha", out flt); Alpha = flt;
            reader.ReadBoolean("TextureEnabled", out bl); TextureEnabled = bl;
            reader.ReadBoolean("VertexColorEnabled", out bl); VertexColorEnabled = bl;
            reader.ReadBoolean("LightingEnabled", out bl); LightingEnabled = bl;
        }
        private DirectionalLightData ReadDirectionalLight(IAetherReader reader, string name)
        {
            DirectionalLightData directionalLight = new DirectionalLightData();
            Vector3 vctr3; bool bl;
            reader.ReadVector3(name + "DiffuseColor", out vctr3); directionalLight.DiffuseColor = vctr3;
            reader.ReadVector3(name + "SpecularColor", out vctr3); directionalLight.SpecularColor = vctr3;
            reader.ReadVector3(name + "Direction", out vctr3); directionalLight.Direction = vctr3;
            reader.ReadBoolean(name + "Enabled", out bl); directionalLight.Enabled = bl;
            return directionalLight;
        }

        #endregion

    }
}
