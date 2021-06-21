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

namespace tainicom.Aether.Elementary.Visual
{
    public interface IGeometryVisitor
    {
        //Vertex data
        void SetVertices<T>(IPhoton photon, T[] vertexData) where T : struct;
        void SetVertices<T>(IPhoton photon, T[] vertexData, int vertexOffset, int primitiveCount, VertexDeclaration vertexDeclaration) where T : struct;        
        void SetVertices<T>(IPhoton photon, int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride) where T : struct;
        void SetVertices<T>(IPhoton photon, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount, VertexDeclaration vertexDeclaration) where T : struct;

        //Vertex buffers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="photon"></param>
        /// <param name="vertexBuffer"></param>
        /// <param name="baseVertex">Offset to add to each vertex index in the index buffer.</param>
        /// <param name="minVertexIndex">Minimum vertex index for vertices used during the call. The minVertexIndex parameter and all of the indices in the index stream are relative to the baseVertex parameter.</param>
        /// <param name="numVertices">Number of vertices used during the call. The first vertex is located at index: baseVertex + minVertexIndex.</param>
        /// <param name="indexBuffer"></param>
        /// <param name="startIndex">Location in the index array at which to start reading vertices.</param>
        /// <param name="primitiveCount">Number of primitives to render. The number of vertices used is a function of primitiveCount and primitiveType.</param>
        void SetVertices(IPhoton photon, VertexBuffer vertexBuffer, int baseVertex, int minVertexIndex, int numVertices, IndexBuffer indexBuffer, int startIndex, int primitiveCount);
        
    }
}
