#region License
//   Copyright 2015-2018 Kastellanos Nikolaos
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
using tainicom.Aether.Elementary.Photons;

namespace tainicom.Aether.Engine
{
    public abstract class AetherContext : IDisposable
    {
        public abstract IDeviceContext DeviceContext { get; }

        private bool _isDisposed = false;
        protected bool IsDisposed { get { return _isDisposed; } }

        public AetherContext()
        {
        }

        ~AetherContext()
        {
            Dispose(false);
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_isDisposed) return;
            OnDispose(disposing);
            _isDisposed = true;
        }
        
        protected abstract void OnDispose(bool disposing);

        #endregion
        
    }
}
