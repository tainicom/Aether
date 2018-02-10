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
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary.Cameras;
using System.ComponentModel;
using tainicom.Aether.Elementary.Serialization;

namespace tainicom.Aether.Core.Cameras
{
    abstract public class Camera: ICamera, IAetherSerialization
    {
        #region Fields
        
        //inner view & projection matrix 
        protected Matrix _view;
        protected Matrix _projection;
        //view related
        
        protected Vector3 _position = Vector3.Zero;
        
        protected Vector3 _lookAt = Vector3.Forward;
        
        protected Vector3 _up = Vector3.Up;
        //projection related
        
        protected float _nearPlane = 10.0f;
        
        protected float _farPlane = 10000.0f;

        //updates
        protected int updateLock = 0;
        #endregion fields

        public Camera()
        {         
        }
        
        #region ICamera Members

        #if(WINDOWS)
        [Browsable(false)]
        #endif
        public Matrix View { get { return _view; } }
        #if(WINDOWS)
        [Browsable(false)]
        #endif
        public Matrix Projection { get { return _projection; } }
        
        #if(WINDOWS)
        [Category("Positioning")]
        #endif
        public virtual Vector3 Position
        {
            get { return _position; }
            set { _position = value; if (updateLock == 0) UpdateView(); }
        }

        #if(WINDOWS)
        [Category("Positioning")]
        #endif
        public Vector3 LookAt
        {
            get { return _lookAt; }
            set { _lookAt = value; if (updateLock == 0) UpdateView(); }
        }

        #if(WINDOWS)
        [Category("Positioning")]
        #endif
        public Vector3 Up
        {
            get { return _up; }
            set { _up = value; if (updateLock == 0) UpdateView(); }
        }

        #if(WINDOWS)
        [Category("Positioning")]
        #endif
        public Vector3 Forward
        {
            get
            {
                Vector3 direction = LookAt - Position;
                direction.Normalize();
                return direction;
            }
            set { LookAt = (Position + value); if (updateLock == 0) UpdateView(); }
        }

        public Vector3 Left
        {
            get
            {
                Vector3 left = Vector3.Cross(_up, Forward);
                left.Normalize();
                return left;
            }
        }

        #if(WINDOWS)
        [Category("Projection")]
        #endif
        public float NearPlane
        {
            get { return _nearPlane; }
            set
            {
                if (value >= _farPlane || value <=0) return;
                _nearPlane = value; if (updateLock == 0) UpdateProjection(); 
            }
        }

        #if(WINDOWS)
        [Category("Projection")]
        #endif
        public float FarPlane
        {
            get { return _farPlane; }
            set
            {
                if (value <= _nearPlane) return;
                _farPlane = value; if (updateLock == 0) UpdateProjection();
            }
        }

        public void UpdateView()
        {
            _view = Matrix.CreateLookAt(_position, _lookAt, _up);
            return;
        }

        abstract public void UpdateProjection();

        public void SuspendUpdates()
        {
            updateLock++;
        }

        public void ResumeUpdates()
        {
            if (updateLock == 0) throw new Exception("Updates are not suspended");
            updateLock--;
            if (updateLock == 0)
            {
                UpdateView();
                UpdateProjection();
            }
            return;
        }

        #endregion


        #if(WINDOWS)
        public virtual void Save(IAetherWriter writer)
        {
            writer.WriteVector3("Position", _position);
            writer.WriteVector3("LookAt", _lookAt);
            writer.WriteVector3("Up", _up);
            writer.WriteFloat("NearPlane", _nearPlane);
            writer.WriteFloat("FarPlane", _farPlane);
        }
        #endif
        public virtual void Load(IAetherReader reader)
        {
            reader.ReadVector3("Position", out _position);
            reader.ReadVector3("LookAt", out _lookAt);
            reader.ReadVector3("Up", out _up);
            reader.ReadFloat("NearPlane", out _nearPlane);
            reader.ReadFloat("FarPlane", out _farPlane);
        }

        public override string ToString()
        {
            return string.Format("Camera, {0}, Position={1}", this.GetType().Name, _position);
        }
               
    }
}
