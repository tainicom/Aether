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
using tainicom.Aether.Core.Materials;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Engine;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;
using System.ComponentModel;
using tainicom.Aether.Core.Materials.Data;
using tainicom.Aether.Elementary.Serialization;
#if (WINDOWS)
using tainicom.Aether.Design.Converters;
#endif

namespace tainicom.Aether.Core.Materials
{
    public class SkinnedMaterial : MaterialBase
    {
        private SkinnedEffect Effect { get { return (SkinnedEffect)_effect; } }
        public int BoneTransformsCount { get; protected set; }

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
        [Category("Skinning")]
        #endif
        public int WeightsPerVertex { get; set; }


        public SkinnedMaterial():base()
        {
            DiffuseColor = Vector3.One;
            AmbientLightColor = Vector3.Zero;
            EmissiveColor = Vector3.Zero;
            DirectionalLight0 = new DirectionalLightData() { Enabled = true };
            DirectionalLight1 = new DirectionalLightData();
            DirectionalLight2 = new DirectionalLightData();
            Alpha = 1f;
            WeightsPerVertex = 4;
            BoneTransformsCount = 0;
        }

        protected override void CreateEffect()
        {
            this._effect = new SkinnedEffect(this.GraphicsDevice);            
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

            Effect.WeightsPerVertex = WeightsPerVertex;

            base.Apply();
        }

        public void ApplySkinned()
        {
            Effect.WeightsPerVertex = WeightsPerVertex;
            _effect.CurrentTechnique.Passes[0].Apply();
        }
                
        public virtual void SetBoneTransforms (Matrix[] boneTransforms)
        {
            Effect.SetBoneTransforms(boneTransforms);
            BoneTransformsCount = boneTransforms.Length;
        }

        public Matrix[] GetBoneTransforms(int count)
        {
            return Effect.GetBoneTransforms(count);
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
            writer.WriteInt64("LightingEnabled", WeightsPerVertex);
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
            Vector3 vctr3; float flt; Int64 i64;
            reader.ReadVector3("DiffuseColor", out vctr3); DiffuseColor = vctr3;
            reader.ReadVector3("AmbientLightColor", out vctr3); AmbientLightColor = vctr3;
            reader.ReadVector3("EmissiveColor", out vctr3); EmissiveColor = vctr3;
            DirectionalLight0 = ReadDirectionalLight(reader, "DirectionalLight0");
            DirectionalLight1 = ReadDirectionalLight(reader, "DirectionalLight1");
            DirectionalLight2 = ReadDirectionalLight(reader, "DirectionalLight2");
            reader.ReadFloat("Alpha", out flt); Alpha = flt;
            reader.ReadInt64("LightingEnabled", out i64); WeightsPerVertex = (int)i64;
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
