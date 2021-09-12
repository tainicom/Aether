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
using tainicom.Aether.Elementary.Spatial;
using tainicom.Aether.Elementary.Serialization;
using tainicom.Aether.Engine.Data;
using tainicom.Aether.Core.Spatial;

namespace tainicom.Aether.Core
{
    public class SpatialPlasma : BasePlasma<ISpatialNode>, ISpatialPlasma, ISpatial, IPosition, ILocalTransform, IWorldTransform, IWorldTransformUpdateable
    {
        SpatialBase _spatialImpl = new SpatialBase();

        public Vector3 Position { get { return _spatialImpl.Position; } set { _spatialImpl.Position = value; UpdateLocalTransform(); } }
        public Vector3 Scale { get { return _spatialImpl.Scale; } set { _spatialImpl.Scale = value; UpdateLocalTransform(); } }
        public Quaternion Rotation { get { return _spatialImpl.Rotation; } set { _spatialImpl.Rotation = value; UpdateLocalTransform(); } }

        public SpatialPlasma()
        {
        }
        
        protected override void InsertItem(int index, ISpatialNode item)
        {   
            base.InsertItem(index, item);

            var updatetable = item as IWorldTransformUpdateable;
            if (updatetable != null)
                updatetable.UpdateWorldTransform(this);
        }

        public Matrix LocalTransform { get { return _spatialImpl.LocalTransform; } }

        public Matrix WorldTransform { get { return _spatialImpl.WorldTransform; } }

        protected void UpdateLocalTransform()
        {
            UpdateChildrenTransform();
        }

        public void UpdateWorldTransform(IWorldTransform parentWorldTransform)
        {
            _spatialImpl.UpdateWorldTransform(parentWorldTransform);
            UpdateChildrenTransform();
        }

        private void UpdateChildrenTransform()
        {
            var spatialNodes = (IPlasma<ISpatialNode>)this;
            foreach (var spatialNode in spatialNodes)
            {
                var updatetable = spatialNode as IWorldTransformUpdateable;
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
