#region License
//   Copyright 2018 Kastellanos Nikolaos
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
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Elementary.Photons;

namespace tainicom.Aether.MonoGame
{
    public class DeviceContextMG : IDeviceContext, IGeometryVisitor
    {
        private GraphicsDevice GraphicsDevice;
        public PrimitiveType PrimitiveType { get; set; }
        

        public DeviceContextMG(GraphicsDevice graphicsDevice)
        {            
            this.GraphicsDevice = graphicsDevice;
        }
        
        public void SetVertices<T>(IPhoton photon, T[] vertexData) where T : struct
        {
            throw new NotImplementedException();
        }

        public void SetVertices<T>(IPhoton photon, T[] vertexData, int vertexOffset, int primitiveCount, VertexDeclaration vertexDeclaration) where T : struct
        {
            GraphicsDevice.DrawUserPrimitives<T>(this.PrimitiveType,
                vertexData, vertexOffset, primitiveCount,
                vertexDeclaration);
        }

        public void SetVertices<T>(IPhoton photon, int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride) where T : struct
        {
            throw new NotImplementedException();
        }

        public void SetVertices<T>(IPhoton photon, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount, VertexDeclaration vertexDeclaration) where T : struct
        {
            GraphicsDevice.DrawUserIndexedPrimitives<T>(this.PrimitiveType,
                vertexData, vertexOffset, numVertices,
                indexData, indexOffset, primitiveCount,
                vertexDeclaration);
        }

        public void SetVertices(IPhoton photon, VertexBuffer vertexBuffer, int baseVertex, int minVertexIndex, int numVertices, IndexBuffer indexBuffer, int startIndex, int primitiveCount)
        {
            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            GraphicsDevice.Indices = indexBuffer;
            GraphicsDevice.DrawIndexedPrimitives(this.PrimitiveType, baseVertex, 0, numVertices, startIndex, primitiveCount);
        }
    }
}
