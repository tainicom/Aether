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
using tainicom.Aether.Maths;

namespace tainicom.Aether.Design.Converters
{
    public class QuaternionEditAsYawPitchRollConverter : TypeConverter
    {
        public QuaternionEditAsYawPitchRollConverter()
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
            if (value is String)
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Vector3));
                Vector3 eulerAngles = (Vector3)typeConverter.ConvertFromString(context, culture, (String)value);
                eulerAngles *= (Tau.FULL/360);
                Quaternion result = YawPitchRollToQuaternion(eulerAngles);
                return result;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value is Quaternion && destinationType == typeof(String))
            {
                Quaternion quaternion = (Quaternion)value;
                Vector3 eulerAngles = QuaternionToYawPitchRoll(quaternion);
                eulerAngles *= (360/Tau.FULL);
                eulerAngles.X = (float)Math.Round(eulerAngles.X, 3);
                eulerAngles.Y = (float)Math.Round(eulerAngles.Y, 3);
                eulerAngles.Z = (float)Math.Round(eulerAngles.Z, 3); 
                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Vector3));
                string result = typeConverter.ConvertToString(context, culture, eulerAngles);
                return result;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        private Quaternion YawPitchRollToQuaternion(Vector3 yawPitchRoll)
        {
            Quaternion result = Quaternion.CreateFromYawPitchRoll(yawPitchRoll.X, yawPitchRoll.Y, yawPitchRoll.Z); 
            return result;
        }

        private Vector3 QuaternionToYawPitchRoll(Quaternion quaternion)
        {
            Vector3 result = Vector3.Zero;

            result.X = (float)Math.Atan2(
                2 * quaternion.Y * quaternion.W - 2 * quaternion.X * quaternion.Z,
                   1 - 2 * Math.Pow(quaternion.Y, 2) - 2 * Math.Pow(quaternion.Z, 2)
            );

            result.Z = (float)Math.Asin(
                2 * quaternion.X * quaternion.Y + 2 * quaternion.Z * quaternion.W
            );

            result.Y = (float)Math.Atan2(
                2 * quaternion.X * quaternion.W - 2 * quaternion.Y * quaternion.Z,
                1 - 2 * Math.Pow(quaternion.X, 2) - 2 * Math.Pow(quaternion.Z, 2)
            );

            if (quaternion.X * quaternion.Y + quaternion.Z * quaternion.W == 0.5)
            {
                result.X = (float)(2 * Math.Atan2(quaternion.X, quaternion.W));
                result.Y = 0;
            }
            
            else if (quaternion.X * quaternion.Y + quaternion.Z * quaternion.W == -0.5)
            {
                result.X = (float)(-2 * Math.Atan2(quaternion.X, quaternion.W));
                result.Y = 0;
            }

            return result;
        }



    }
}