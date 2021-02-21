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
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Leptons;
using tainicom.Aether.Elementary.Serialization;
using tainicom.Aether.Engine.Data;

namespace tainicom.Aether.Core
{
    public class LeptonPlasma : BasePlasma<ILeptonNode>, ILeptonPlasma, ILepton, IPosition, ILocalTransform, IWorldTransform, IWorldTransformUpdateable
    {
        Vector3 _position;
        Vector3 _scale = Vector3.One;
        Quaternion _rotation = Quaternion.Identity;

        Matrix _localTransform = Matrix.Identity;
        Matrix _parentWorldTransform = Matrix.Identity;
        Matrix _worldTransform = Matrix.Identity;

        public Vector3 Position { get { return _position; } set { _position = value; UpdateLocalTransform(); } }
        public Vector3 Scale { get { return _scale; } set { _scale = value; UpdateLocalTransform(); } }
        public Quaternion Rotation { get { return _rotation; } set { _rotation = value; UpdateLocalTransform(); } }

        public LeptonPlasma()
        {
        }
        
        protected override void InsertItem(int index, ILeptonNode item)
        {   
            base.InsertItem(index, item);

            var updatetable = item as IWorldTransformUpdateable;
            if (updatetable != null)
                updatetable.UpdateWorldTransform(this);
        }

        public Matrix LocalTransform
        {
            get { return _localTransform; }
        }

        public Matrix WorldTransform
        {
            get { return _worldTransform; }
        }

        protected void UpdateLocalTransform()
        {
            _localTransform = Matrix.CreateScale(_scale)
                            * Matrix.CreateFromQuaternion(_rotation)
                            * Matrix.CreateTranslation(_position);
            
            _worldTransform = _localTransform * _parentWorldTransform;
            UpdateChildrenTransform();
        }

        public void UpdateWorldTransform(IWorldTransform parentWorldTransform)
        {
            _parentWorldTransform = parentWorldTransform.WorldTransform;
            _worldTransform = _localTransform * _parentWorldTransform;
            UpdateChildrenTransform();
        }

        private void UpdateChildrenTransform()
        {
            foreach (var child in this)
            {
                var updatetable = child as IWorldTransformUpdateable;
                if (updatetable != null)
                    updatetable.UpdateWorldTransform(this);
            }
        }

        
        #region Implement IAetherSerialization
        public override void Save(IAetherWriter writer)
        {
            writer.WriteInt32("Version", 1);

            base.Save(writer);
        }

        public override void Load(IAetherReader reader)
        {
            int version;
            reader.ReadInt32("Version", out version);

            switch (version)
            {
                case 1:
                    base.Load(reader);
                  break;
                default:
                  throw new InvalidOperationException("unknown version " + version);
            }
        }
        #endregion


    }
}
