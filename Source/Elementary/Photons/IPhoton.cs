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
using System.Text;
using tainicom.Aether.Elementary;
using Microsoft.Xna.Framework.Graphics;

namespace tainicom.Aether.Elementary.Photons
{
    /// <summary>
    /// Particle that can be visualised.
    /// This means it has a geometry and a material.
    /// </summary>
    /// <remarks>
    /// Instead of just exposing the geometry as a form of Vertices/indices
    /// we use something similar to a visitor patterns in order to work around
    /// some limitations of C#. 
    /// Specifically, MonoGame/XNA only accepts vertices through generics accepting the IVertexType.
    /// With a direct aproach, we would have to define a function for each IVertexType
    /// and this pattern would propagate all the way throughout the engine. 
    /// Now we confine functions like VertexBuffer.SetData<IVertexType>(...) and 
    /// GraphicsDevice.DrawUserPrimitives<IVertexType>(...) once inside the visitor.
    /// </remarks>
    public interface IPhoton : IAether, IPhotonNode
    {
        void Accept(IGeometryVisitor geometryVisitor);
        IMaterial Material { get; }
        ITexture[] Textures { get; }
    }
}
