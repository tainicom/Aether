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

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Elementary.Visual;
using tainicom.Aether.Engine;

namespace tainicom.Aether.MonoGame
{
    public class AetherContextMG: AetherContext
    {
        private GraphicsDevice _graphicsDevice;
        private ContentManager _contentManager;
        private IDeviceContext _deviceContext;

        public GraphicsDevice Device { get { return _graphicsDevice; } }
        public ContentManager Content { get { return _contentManager; } }
        
        public override IDeviceContext DeviceContext { get { return _deviceContext; } }

        public AetherContextMG(GraphicsDevice graphicsDevice, ContentManager content):base()
        {
            this._graphicsDevice = graphicsDevice;
            this._contentManager = content;

            this._deviceContext = new DeviceContextMG(graphicsDevice);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    _contentManager.Dispose();
                    _graphicsDevice.Dispose();
                }

                this._graphicsDevice = null;
                this._contentManager = null;
            }

            base.Dispose(disposing);
        }
        
        public static GraphicsDevice GetDevice(AetherEngine engine)
        {
            return ((AetherContextMG)engine.Context).Device;
        }

        public static ContentManager GetContent(AetherEngine engine)
        {
            return ((AetherContextMG)engine.Context).Content;
        }

    }
}
