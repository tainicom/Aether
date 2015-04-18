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

using Microsoft.Xna.Framework;

namespace tainicom.Aether.Maths
{
    public static class Extensions
    {       
        public static float Distance(this Plane plane, ref Vector3 point)
        {
            float dot = Vector3.Dot(plane.Normal, point);
            float value = dot - plane.D;
            value = value / Vector3.Dot(plane.Normal, plane.Normal); // for plane normals that are not normalized
            return value;
        }

        public static bool Intersects(this Plane planeA, ref Plane planeB, out Line3 line)
        {
            line = new Line3(Vector3.Zero, Vector3.Zero);
            Vector3.Cross(ref planeA.Normal, ref planeB.Normal, out line.Direction);
            
            float demon = Vector3.Dot(line.Direction, line.Direction);
            if(demon < float.Epsilon) return false; // parallel?

            // point on intersection line
            line.Position = Vector3.Cross(planeA.D * planeB.Normal - planeB.D * planeA.Normal, line.Direction);
            line.Position = line.Position / demon; // for plane normals that are not normalized
            
            return true;
        }
            
    }
}
