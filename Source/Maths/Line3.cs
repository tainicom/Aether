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
    public struct Line3
    {
        public Vector3 Direction;
        public Vector3 Position;
        
        public Line3(Vector3 position, Vector3 direction)
        {
            this.Position = position;
            this.Direction = direction;
        }

        public static void ClosestPoints(ref Line3 L1, ref Line3 L2, out Vector3 Q1, out Vector3 Q2)
        {
            Vector3 P1 = L1.Position;
            Vector3 P2 = L2.Position;
            Vector3 d1 = L1.Direction;
            Vector3 d2 = L2.Direction;

            var r = P1 - P2;

            var a = Vector3.Dot(d1, d1);
            var b = Vector3.Dot(d1, d2);
            var c = Vector3.Dot(d1, r);
            var e = Vector3.Dot(d2, d2);
            var f = Vector3.Dot(d2, r);


            var d = a*e - b*b;
            if (d == 0)
            {
                // parallel lines
                //var s = 0;
                //var t = ??
            }

            var s = (b*f - c*e)/d;
            var t = (a*f - b*c)/d;

            Q1 = P1 + s * d1;
            Q2 = P2 + t * d2;
            return;
        }
    }

}
