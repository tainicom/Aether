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
using tainicom.Aether.Elementary.Temporal;
using Microsoft.Xna.Framework;

namespace tainicom.Aether.Core.Chronons
{
    public class AetherTimer : IChronon
    {
        protected TimeSpan elapsedTime;

        public TimeSpan ElapsedTime { get { return elapsedTime; } }


        public AetherTimer()
        {
            Reset();
        }

        public virtual void Reset()
        {
            elapsedTime = TimeSpan.Zero;
        }

        public virtual void Tick(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;
        }

    }    
}
