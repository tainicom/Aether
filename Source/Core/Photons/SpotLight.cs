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
using tainicom.Aether.Elementary.Photons;
using tainicom.Aether.Elementary.Serialization;
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary.Leptons;
using tainicom.Aether.Maths;

namespace tainicom.Aether.Core.Photons
{
    public class SpotLight : PointLight, ILightCone
    {
        public float InnerAngle { get; set; }
        public float OuterAngle { get; set; }
        public Vector3 Direction { get; set; }

        public SpotLight(): base()
        {
            Direction = Vector3.Forward;
            InnerAngle = OuterAngle = Tau.HALFQUARTER;
        }

        public override void Save(IAetherWriter writer)
        {
            base.Save(writer);
            writer.WriteFloat("InnerAngle", InnerAngle);
            writer.WriteFloat("OuterAngle", OuterAngle);
            writer.WriteVector3("Direction", Direction);
        }

        public override void Load(IAetherReader reader)
        {
            base.Load(reader);
            Vector3 v3; float f;
            reader.ReadFloat("InnerAngle", out f); InnerAngle = f;
            reader.ReadFloat("OuterAngle", out f); OuterAngle = f;
            reader.ReadVector3("Direction", out v3); Direction = v3;
        }

    }
}
