﻿#region License
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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Chronons;
using tainicom.Aether.Elementary.Photons;
using tainicom.Aether.Elementary.Serialization;
using tainicom.Aether.Engine.Data;

namespace tainicom.Aether.Core
{
    public class PhotonPlasma: BasePlasma<IPhotonNode>, IPhotonPlasma
    {
        HashSet<IPhotonNode> _visibleParticles;

        public virtual IEnumerator<IPhotonNode> VisibleParticles { get { return _visibleParticles.GetEnumerator(); } }

        public PhotonPlasma()
        {
            _visibleParticles = new HashSet<IPhotonNode>();
        }
        
        protected override void InsertItem(int index, IPhotonNode item)
        {
            base.InsertItem(index, item);
            _visibleParticles.Add(item);
            return;
        }

        protected override void RemoveItem(int index)
        {
            IAether item = this[index];
            _visibleParticles.Remove((IPhotonNode)item);
            base.RemoveItem(index);
        }
        
        public virtual void Enable(IPhoton item)
        {
            _visibleParticles.Add(item);
        }

        public virtual void Disable(IPhoton item)
        {
            _visibleParticles.Remove(item);
        }

        public bool IsEnabled(IPhoton item)
        {
            return _visibleParticles.Contains(item);
        }


        #region Implement IAetherSerialization
        public override void Save(IAetherWriter writer)
        {
            writer.WriteInt32("Version", 1);

            base.Save(writer);
            // TODO: add IAetherWriter.WriteParticles(string, new ISet<T>)
            writer.WriteParticles("VisibleParticles", new List<IPhotonNode>(_visibleParticles));
        }

        public override void Load(IAetherReader reader)
        {
            int version;
            reader.ReadInt32("Version", out version);

            switch (version)
            {
                case 1:
                base.Load(reader);
                    _visibleParticles.Clear();
                    var visibleParticles = new List<IPhotonNode>();
                    // TODO: add IAetherWriter.WriteParticles(string, new ISet<T>)
                    reader.ReadParticles("VisibleParticles", visibleParticles); _visibleParticles = new HashSet<IPhotonNode>(visibleParticles);
                    break;
                default:
                    throw new InvalidOperationException("unknown version " + version);
            }
        }
        #endregion


    }
}
