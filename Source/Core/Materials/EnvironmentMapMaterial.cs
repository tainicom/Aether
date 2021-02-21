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

#if(WINDOWS)
using tainicom.Aether.Design.Converters;
#endif
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using tainicom.Aether.Core.Materials.Data;
using tainicom.Aether.Elementary.Serialization;

namespace tainicom.Aether.Core.Materials
{
    public class EnvironmentMapMaterial : MaterialBase
    {
        //TODO: we must hide Effect class completly.
        public EnvironmentMapEffect Effect { get { return (EnvironmentMapEffect)_effect; } }
        
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
        [Category("EnviromentalMap")]
        #endif
        public float EnvironmentMapAmount { get; set; }
        #if (WINDOWS)
        [Category("EnviromentalMap")]
        [TypeConverter(typeof(Vector3EditConverter))]
        #endif
        public Vector3 EnvironmentMapSpecular { get; set; }
        #if (WINDOWS)
        [Category("EnviromentalMap")]
        #endif
        public float FresnelFactor { get; set; }
        
        public EnvironmentMapMaterial():base()
        {
            DiffuseColor = Vector3.One;
            AmbientLightColor = Vector3.Zero;
            EmissiveColor = Vector3.Zero;
            DirectionalLight0 = new DirectionalLightData() { Enabled = true };
            DirectionalLight1 = new DirectionalLightData();
            DirectionalLight2 = new DirectionalLightData();
            Alpha = 1f;
            EnvironmentMapAmount = 1f;
            EnvironmentMapSpecular = Vector3.Zero;
            FresnelFactor = 1f;
        }
        
        protected override void CreateEffect()
        {
            this._effect = new EnvironmentMapEffect(this.GraphicsDevice);
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

            Effect.EnvironmentMapAmount = EnvironmentMapAmount;
            Effect.EnvironmentMapSpecular = EnvironmentMapSpecular;
            Effect.FresnelFactor = FresnelFactor;

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
            writer.WriteFloat("EnvironmentMapAmount", EnvironmentMapAmount);
            writer.WriteVector3("EnvironmentMapSpecular", EnvironmentMapSpecular);
            writer.WriteFloat("FresnelFactor", FresnelFactor);     
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

            reader.ReadFloat("EnvironmentMapAmount", out flt); EnvironmentMapAmount = flt;
            reader.ReadVector3("EnvironmentMapSpecular", out vctr3); EnvironmentMapSpecular = vctr3;
            reader.ReadFloat("FresnelFactor", out flt); FresnelFactor = flt;
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
