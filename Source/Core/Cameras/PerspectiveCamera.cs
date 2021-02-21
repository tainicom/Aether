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

using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Cameras;
using tainicom.Aether.Elementary.Serialization;
using tainicom.Aether.Engine;
using tainicom.Aether.Maths;
using tainicom.Aether.MonoGame;

namespace tainicom.Aether.Core.Cameras
{
    public class PerspectiveCamera : Camera, IPerspectiveCamera, IInitializable
    {
        #region Fields
        //projection (perspective)
        protected float _aspectRatio;
        protected float _fieldOfView = Tau.HALFQUARTER;
        #endregion fields
        
        public float AspectRatio
        {
            get { return _aspectRatio; }
            set { _aspectRatio = value; if (updateLock == 0) UpdateProjection(); }
        }

        #if(WINDOWS)
        [Category("Projection")]
        #endif
        public float FieldOfView
        {
            get { return _fieldOfView; }
            set { _fieldOfView = value; if (updateLock == 0) UpdateProjection(); }
        }

        public PerspectiveCamera()
        {
        }

        public PerspectiveCamera(Viewport viewport)
            : base()
        {
            float width = (float)viewport.Width;
            float height = (float)viewport.Height;
            _aspectRatio = width / height;
            UpdateView();
            UpdateProjection();
        }
        
        public PerspectiveCamera(Viewport viewport, ICamera fromCamera):base()
        {
            float width = (float)viewport.Width;
            float height = (float)viewport.Height;
            _aspectRatio = width / height;
            
            this.Position = fromCamera.Position;
            this.LookAt = fromCamera.LookAt;
            this.Up = fromCamera.Up;
            this.NearPlane = fromCamera.NearPlane;
            this.FarPlane = fromCamera.FarPlane;

            UpdateView();
            UpdateProjection();

            return;
        }

        public void CopyFrom(IPerspectiveCamera fromCamera)
        {
            this._position = fromCamera.Position;
            this._lookAt = fromCamera.LookAt;
            this._up = fromCamera.Up;
            this._nearPlane = fromCamera.NearPlane;
            this._farPlane = fromCamera.FarPlane;
            this._fieldOfView = fromCamera.FieldOfView;
            this._aspectRatio = fromCamera.AspectRatio;
            UpdateView();
            UpdateProjection();
        }

        public virtual void Initialize(AetherEngine engine)
        {
            var device = AetherContextMG.GetDevice(engine);
            float width = (float)device.Viewport.Width;
            float height = (float)device.Viewport.Height;
            _aspectRatio = width / height;
            UpdateView();
            UpdateProjection();
        }

        /*
        public PerspectiveCamera(GraphicsDevice graphicsDevice, PerspectiveCamera fromCamera): base(graphicsDevice)
        {
            //copy properties from another camera
            _position = fromCamera.Position;
            _lookAt = fromCamera.LookAt;
            _up = fromCamera.Up;
            _nearPlane = fromCamera.NearPlane;
            _farPlane = fromCamera.FarPlane;
            _fieldfView = fromCamera.FieldView;

            //update view and projection
            UpdateView();
            UpdateProjection();
        }
        */
        
        #region Camera Members

        public override void UpdateProjection()
        {
            if (_fieldOfView == 0)
            {
                _projection = Matrix.Identity;
                return;
            }
            
            _projection = Matrix.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, _nearPlane, _farPlane);
        }

        #endregion

        
        public override void Save(IAetherWriter writer)
        {
            base.Save(writer);
            writer.WriteFloat("FieldOfView", _fieldOfView);
        }        

        public override void Load(IAetherReader reader)
        {
            base.Load(reader);
            reader.ReadFloat("FieldOfView", out _fieldOfView);
        }
    }
}
