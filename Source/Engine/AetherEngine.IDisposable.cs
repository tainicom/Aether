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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Managers;

//namespace tainicom.Aether.Engine
namespace tainicom.Aether.Engine
{
    //public partial class AetherEngine: IDisposable
    public partial class AetherEngine : IDisposable
    {        
        protected bool isDisposed = false;
        
        //~AetherEngine()
        ~AetherEngine()
        {
            Dispose(false);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;
            if (disposing)
            {   
                //free unmanaged objects
                foreach (IAetherManager particleManager in _particleManagers)
                {
                    particleManager.Dispose();
                }
                //((IDisposable)_aetherContext).Dispose();
            }
            //clear managed objects
            _particleManagers.Clear();
            _particleManagers = null;
            //_moduleManager = null;
            //_aetherContext = null;

            isDisposed = true;
            return;
        }

        #endregion

    }
}
