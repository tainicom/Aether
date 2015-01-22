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

namespace tainicom.Aether.Maths
{
    // Summary:
    //     Represents the ratio of the circumference of a circle to its radious, 
    //     specified by the constant τ.
    public class Tau
    {   
        public const float DOUBLE       = (float)Math.PI * 4;

        public const float FULL         = (float)Math.PI * 2;
        public const float HALF         = (float)Math.PI * 1;
        public const float QUARTER      = (float)Math.PI / 2;
        public const float HALFQUARTER  = (float)Math.PI / 4;
    }

    // Summary:
    //     Represents the ratio of the circumference of a circle to its radious, 
    //     specified by the constant τ.
    public class TauD
    {
        public const double DOUBLE      = Math.PI * 4;

        public const double FULL        = Math.PI * 2;
        public const double HALF        = Math.PI * 1;
        public const double QUARTER     = Math.PI / 2;
        public const double HALFQUARTER = Math.PI / 4;
    }

}
