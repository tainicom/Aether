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
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace tainicom.Aether.Design.Converters
{
    public class Vector4ColorWPGConverter : TypeConverter
    {
            public Vector4ColorWPGConverter()
            {
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string)) return true;
                else return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if(value is String)
                {
                    TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Vector4));
                    Vector4 vec = (Vector4)typeConverter.ConvertFromString(context, culture, (String)value);
                    vec = vec / 255f;
                    return vec;
                }
                
                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (value is Vector4 && destinationType == typeof(String))
                {
                    Vector4 vec = (Vector4)value;
                    vec.X = (int)Math.Round(vec.X * 255);
                    vec.Y = (int)Math.Round(vec.Y * 255);
                    vec.Z = (int)Math.Round(vec.Z * 255);
                    vec.W = (int)Math.Round(vec.W * 255);

                    TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Vector4));
                    string result = typeConverter.ConvertToString(context, culture, vec);
                    return result;
                }    
            
                return base.ConvertTo(context, culture, value, destinationType);
            }
    }
}

