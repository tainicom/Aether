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
        private Dictionary<Assembly, ContentManager> _contentMgrs;

        public GraphicsDevice Device { get { return _graphicsDevice; } }
        public ContentManager Content { get { return _contentManager; } }
        
        public AetherContextMG(GraphicsDevice graphicsDevice, ContentManager content):base()
        {
            this._graphicsDevice = graphicsDevice;
            this._contentManager = content;
            this._contentMgrs = new Dictionary<Assembly, ContentManager>();
        }
        
        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                _contentManager.Dispose();
                _graphicsDevice.Dispose();
                foreach (var contentMgr in _contentMgrs.Values)
                    contentMgr.Dispose();
            }

            this._graphicsDevice = null;
            this._contentManager = null;
            this._contentMgrs = null;

            return;
        }
        
        public ContentManager GetContent(Assembly library)
        {
            ContentManager result;
            _contentMgrs.TryGetValue(library, out result);

            if (result == null)
            {
                //cache lib contentManagers
            string hstLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\";
            string asmLocation = Path.GetDirectoryName(library.Location) + "\\";
            String relModuleLocation = new Uri(hstLocation).MakeRelativeUri(new Uri(asmLocation)).ToString();
            String rootDirectory = relModuleLocation + "/Content";
                result = new ContentManager(Content.ServiceProvider, rootDirectory);
                _contentMgrs.Add(library, result);
            }
            else return result;

            return result;
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
