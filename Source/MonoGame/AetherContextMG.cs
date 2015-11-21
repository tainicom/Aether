using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using tainicom.Aether.Engine;
using System.IO;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace tainicom.Aether.MonoGame
{
    public class AetherContextMG: AetherContext
    {
        private GraphicsDevice _graphicsDevice;
        private ContentManager _contentManager;

        public GraphicsDevice Device { get { return _graphicsDevice; } }
        public ContentManager Content { get { return _contentManager; } }
        
        public AetherContextMG(GraphicsDevice graphicsDevice, ContentManager content):base()
        {
            this._graphicsDevice = graphicsDevice;
            this._contentManager = content;
        }
        
        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                _contentManager.Dispose();
                _graphicsDevice.Dispose();
            }

            this._graphicsDevice = null;
            this._contentManager = null;

            return;
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
