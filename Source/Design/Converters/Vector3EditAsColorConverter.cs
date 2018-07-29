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
using System.Globalization;
using Microsoft.Xna.Framework;

namespace tainicom.Aether.Design.Converters
{
    public class Vector3EditAsColorConverter : TypeConverter
    {
            public Vector3EditAsColorConverter()
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
                    return FromHexColor((string)value);
  
                    TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Vector3));
                    Vector3 vec = (Vector3)typeConverter.ConvertFromString(context, culture, (String)value);
                    vec = vec / 255f;
                    return vec;
                }
                
                return base.ConvertFrom(context, culture, value);
            }

            
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (value is Vector3 && destinationType == typeof(String))
                {
                    return ToHexColor(value);

                    Vector3 vec = (Vector3)value;
                    vec.X = (int)Math.Round(vec.X * 255);
                    vec.Y = (int)Math.Round(vec.Y * 255);
                    vec.Z = (int)Math.Round(vec.Z * 255);
                    
                    TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Vector3));
                    string result = typeConverter.ConvertToString(context, culture, vec);
                    return result;
                } 
               
                return base.ConvertTo(context, culture, value, destinationType);
            }

            private object ToHexColor(object value)
            {           
                Vector3 vec = (Vector3)value;
                vec = vec * 255;
                return String.Format("#{0:X2}{1:X2}{2:X2}", (int)vec.X, (int)vec.Y, (int)vec.Z);
            }

            private object FromHexColor(string value)
            {   
                string val = value;
                val = val.Trim().Replace("#", "");
                int r = byte.Parse(val.Substring(0,2), NumberStyles.HexNumber);
                int g = byte.Parse(val.Substring(2,2), NumberStyles.HexNumber);
                int b = byte.Parse(val.Substring(4,2), NumberStyles.HexNumber);

                return (object)(new Vector3(r,g,b)/255f);
            }

    }
}

/*
 * 
 * ArrayList piers = new ArrayList();
            public PierListConverter()
            {

            }
            public override bool
            GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
            public override StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
            {
                // This method returns me the list that will use to fill combo at property grid.
                piers.Clear();
                foreach (var item in GullsEyeModel.GetInstance().GetPiers())
                {
                    piers.Add(item.Id);
                }
                StandardValuesCollection cols = new  StandardValuesCollection(piers);
                return cols;
            }
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
               // If this method returns true, ConvertFrom method will invoke
                if (sourceType == typeof(string))
                {
                    return true;
                }
                else
                return base.CanConvertFrom(context, sourceType);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                // In this method i am getting selected text and querying; after that i retrieve proparete Guid value and then returning back to my object that binded property grid.
                if (value != null)
                {
                    if (value.ToString() == "Seçiniz")
                    {
                        return Guid.Empty;
                    }
                    else if (!string.IsNullOrEmpty(value.ToString()))
                    {
                        GuidConverter g = new GuidConverter();
                        PierItem[] pierArray = GullsEyeModel.GetInstance().GetPiers();
                        PierItem selectedPier = pierArray.Where(item => item.Info.Name == value.ToString()).FirstOrDefault();
                        if (selectedPier != null)
                        {
                            return selectedPier.Id;
                        }
                        else
                            return base.ConvertFrom(context, culture, value);
                    }
                    else
                        return base.ConvertFrom(context, culture, value);
                }
                else
                return base.ConvertFrom(context, culture, value);
            }
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
// In this method i am converting ID to string (Name) to display in Property Grid
                if (value != null)
                {
                    GuidConverter g = new GuidConverter();
                    PierItem[] piers = GullsEyeModel.GetInstance().GetPiers();
                    PierItem selectedPier = piers.Where(item => item.Id== (Guid)g.ConvertFromString(value.ToString())).FirstOrDefault();
                    if (selectedPier != null)
                    {
                        return selectedPier.Info.Name;
                    }
                    else
                        return "Seçiniz";
                }
                else
                return base.ConvertTo(context, culture, value, destinationType);
            }

 * 
*/
