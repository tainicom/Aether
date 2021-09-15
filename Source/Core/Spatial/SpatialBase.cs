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
using Microsoft.Xna.Framework;
using tainicom.Aether.Core.Components;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Components;
using tainicom.Aether.Elementary.Serialization;
using tainicom.Aether.Elementary.Spatial;

namespace tainicom.Aether.Core.Spatial
{
    public class SpatialBase : 
        ISpatial, IWorldTransform, IWorldTransformUpdateable,
        IAetherSerialization, IAether
    {
        private Vector3 _scale = Vector3.One;
        private Quaternion _rotation = Quaternion.Identity;
        private Vector3 _position;

        private Matrix _localTransform = Matrix.Identity;
        private Matrix _parentWorldTransform = Matrix.Identity;
        private Matrix _worldTransform = Matrix.Identity;


        public SpatialBase()
        {
        }


        #region Implement ISpatial Properties

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; UpdateLocalTransform(); }
        }

        public Quaternion Rotation
        {
            get { return _rotation; }
            set { _rotation = value; UpdateLocalTransform(); }
        }

        public Vector3 Scale
        {
            get { return _scale; }
            set { _scale = value; UpdateLocalTransform(); }
        }

        public Matrix LocalTransform { get { return _localTransform; } }

        #endregion ISpatial Properties

        #region Implement IWorldTransform, IWorldTransformUpdateable
        public void UpdateWorldTransform(IWorldTransform parentWorldTransform)
        {
            _parentWorldTransform = parentWorldTransform.WorldTransform;
            UpdateWorldTransform();
        }

        public Matrix WorldTransform { get { return _worldTransform; } }
        #endregion


        protected virtual void UpdateLocalTransform()
        {
            _localTransform = Matrix.CreateScale(_scale)
                            * Matrix.CreateFromQuaternion(_rotation)
                            * Matrix.CreateTranslation(_position);

            UpdateWorldTransform();
        }

        private void UpdateWorldTransform()
        {
            _worldTransform = _localTransform * _parentWorldTransform;
        }

        #region Implement IAetherSerialization
        public void Save(IAetherWriter writer)
        {
            writer.WriteVector3("Position", _position);
            writer.WriteVector3("Scale", _scale);
            writer.WriteQuaternion("Rotation", _rotation);
        }
        public void Load(IAetherReader reader)
        {
            reader.ReadVector3("Position", out _position);
            reader.ReadVector3("Scale", out _scale);
            reader.ReadQuaternion("Rotation", out _rotation);
            UpdateLocalTransform();
        }
        #endregion
    }
}
