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
using System;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;

namespace tainicom.Aether.Core.Materials.Data
{
    [Obsolete("This is used just for serialising to/from DataContractSerialisation.")]
    /// <summary>
    /// BasicEffect.DirectionalLightX cannot serialize because it needs
    /// a GraphicDevice reference. So we have to keep our own copy of data
    /// and copy them to Effect.
    /// </summary>
    #if (WINDOWS)
    [TypeConverter(typeof(ExpandableObjectConverter))]
    #endif
    public class DirectionalLightData
    {              
        #if (WINDOWS)
        [TypeConverter(typeof(Vector3EditAsColorConverter))]
        #endif
        public Vector3 DiffuseColor { get; set; }
        #if (WINDOWS)
        [TypeConverter(typeof(Vector3EditAsColorConverter))]
        #endif
        public Vector3 SpecularColor { get; set; }
        #if (WINDOWS)
        [TypeConverter(typeof(Vector3EditConverter))]
        #endif
        public Vector3 Direction { get; set; }
        public bool Enabled { get; set; }

        public DirectionalLightData()
        {
            DiffuseColor = Vector3.One;
            SpecularColor = Vector3.Zero;
            Direction = Vector3.Down;
            Enabled = false;
        }

        public void CopyTo(Microsoft.Xna.Framework.Graphics.DirectionalLight directionalLight)
        {
            directionalLight.DiffuseColor = DiffuseColor;
            directionalLight.SpecularColor = SpecularColor;
            directionalLight.Direction = Direction;
            directionalLight.Enabled = Enabled;
        }        
    }
}
