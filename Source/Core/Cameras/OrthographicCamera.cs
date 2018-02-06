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

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary.Cameras;

namespace tainicom.Aether.Core.Cameras
{
    public class OrthographicCamera : Camera, IOrthographicCamera
    {
        #region Fields
        //projection (orthographic)
        float _width = 1280;
        float _height = 720;
        //updates
        //int updateLock = 0;
        #endregion fields
        
        public float Width
        {
            get { return _width; }
            set { _width = value; if (updateLock == 0) UpdateProjection(); }
        }

        public float Height
        {
            get { return _height; }
            set { _height = value; if (updateLock == 0) UpdateProjection(); }
        }

        public OrthographicCamera()
        {
        }

        public OrthographicCamera(float width, float height): base()
        {
            _width = width;
            _height = height;
            UpdateView();
            UpdateProjection();
        }
        
        #region Camera Members
        
        public override void UpdateProjection()        
        {
            _projection = Matrix.CreateOrthographic(_width, _height, _nearPlane, _farPlane);
        }

        #endregion
    }
}
