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
using tainicom.Aether.Core.Materials;
using tainicom.Aether.Core.Materials.Data;
using tainicom.Aether.Elementary.Serialization;
using System;

namespace tainicom.Aether.Core.Materials
{
    public class AlphaTestMaterial : MaterialBase
    {
        //TODO: we must hide Effect class completly.
        public AlphaTestEffect Effect { get { return (AlphaTestEffect)_effect; } } 

        #if (WINDOWS)
        [Category("Lighting")]
        [TypeConverter(typeof(Vector3EditAsColorConverter))] 
        #endif
        public Vector3 DiffuseColor { get; set; }

        #if (WINDOWS)
        [Category("Lighting")]
        #endif
        public int ReferenceAlpha { get; set; }
        
        #if (WINDOWS)
        [Category("AlphaTest")]
        #endif
        public float Alpha { get; set; }

        #if (WINDOWS)
        [Category("AlphaTest")]
        #endif
        public bool VertexColorEnabled { get; set; }

        public AlphaTestMaterial():base()
        {
            DiffuseColor = Vector3.One;
            ReferenceAlpha = 0;
            Alpha = 1f;
        }

        protected override void CreateEffect()
        {
            this._effect = new AlphaTestEffect(this.GraphicsDevice);
        }

        public override void Apply()
        {
            Effect.DiffuseColor = DiffuseColor;
            Effect.ReferenceAlpha = ReferenceAlpha;
            Effect.Alpha = Alpha;

            Effect.VertexColorEnabled = VertexColorEnabled;
            
            base.Apply();
        }

        
        #region Aether.Elementary.Serialization.IAetherSerialization Members

        public override void Save(IAetherWriter writer)
        {
            base.Save(writer);
            writer.WriteVector3("DiffuseColor", DiffuseColor);
            writer.WriteFloat("Alpha", Alpha);
            writer.WriteInt64("ReferenceAlpha", ReferenceAlpha);
            writer.WriteBoolean("VertexColorEnabled", VertexColorEnabled);
        }

        public override void Load(IAetherReader reader)
        {
            base.Load(reader);
            Vector3 vctr3; float flt; Int64 i64;  bool bl;
            reader.ReadVector3("DiffuseColor", out vctr3); DiffuseColor = vctr3;
            reader.ReadFloat("Alpha", out flt); Alpha = flt;
            reader.ReadInt64("ReferenceAlpha", out i64); ReferenceAlpha = (int)i64;
            reader.ReadBoolean("VertexColorEnabled", out bl); VertexColorEnabled = bl;
        }

        #endregion

    }
}
