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

namespace tainicom.Aether.Core.Temporal
{
    public class AetherPhasor : AetherTimer
    {
        public float A;
        public float Frequency;
        public float Period;
        public float Phase; // φ
        public float AngularFrequency; // ω (Angular frequency)
        public float Value 
        { 
            get {return A * (float)Math.Sin((double)(elapsedTime.TotalSeconds * AngularFrequency + Phase));}
        }

        public AetherPhasor(): base()
        {
            Reset();
        }

        public static AetherPhasor CreateFromFreq(float frequency)
        {
            return CreateFromFreq(frequency, 1f, 0f);
        }

        public static AetherPhasor CreateFromFreq(float frequency, float A)
        {
            return CreateFromFreq(frequency, A, 0f);
        }

        public static AetherPhasor CreateFromFreq(float frequency, float A, float phase)
        {
            AetherPhasor _this = new AetherPhasor();

            _this.A = A;
            _this.Frequency = frequency;
            _this.Phase = phase;

            _this.Period = 1 / frequency;
            _this.AngularFrequency = Aether.Maths.Tau.FULL * frequency;
            return _this;
        }

        public virtual void Reset()
        {
            elapsedTime = TimeSpan.Zero;
        }

        public override void Tick(GameTime gameTime)
        {
            base.Tick(gameTime);
        }

    }    
}
