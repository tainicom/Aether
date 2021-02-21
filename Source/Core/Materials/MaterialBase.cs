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
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Elementary.Photons;
using tainicom.Aether.Engine;
using System.ComponentModel;
using tainicom.Aether.Elementary;
using tainicom.Aether.Elementary.Serialization;
using tainicom.Aether.MonoGame;

namespace tainicom.Aether.Core.Materials
{
    abstract public class MaterialBase : IMaterial, IInitializable, IShaderMatrices, IAetherSerialization
    {   
        #region Implement IShaderMatrices Properties
        #if(WINDOWS)
        [Browsable(false)]
        #endif
        public Matrix Projection { get; set; }
        #if(WINDOWS)
        [Browsable(false)]
        #endif
        public Matrix View { get; set; }
        #if(WINDOWS)
        [Browsable(false)]
        #endif
        public Matrix World { get; set; }
        #endregion

        protected GraphicsDevice GraphicsDevice { get; private set; }
        IDeviceContext DeviceContext;
        protected Effect _effect;

        #if(WINDOWS)
        [Category("States")]
        #endif
        public BlendState BlendState { get; set; }
        #if(WINDOWS)
        [Category("States")]
        #endif
        public DepthStencilState DepthStencilState { get; set; }
        #if(WINDOWS)
        [Category("States")]
        #endif
        public RasterizerState RasterizerState { get; set; }
        #if(WINDOWS)
        [Category("States")]
        #endif
        public SamplerState[] SamplerStates { get; set; }
        public PrimitiveType PrimitiveType { get; set; }

        
        protected const int NumberOfTextures = 4;

        public MaterialBase()
        {
            this.BlendState = BlendState.Opaque;
            this.DepthStencilState = DepthStencilState.Default;
            this.RasterizerState = RasterizerState.CullCounterClockwise;
            this.SamplerStates = new SamplerState[NumberOfTextures];
            this.SamplerStates[0] = SamplerState.LinearClamp;
            this.PrimitiveType = PrimitiveType.TriangleList;
        }

        public virtual void Initialize(AetherEngine engine)
        {
            this.GraphicsDevice = AetherContextMG.GetDevice(engine);
            this.DeviceContext = engine.Context.DeviceContext;
            CreateEffect();
        }

        protected virtual void CreateEffect() { }

        public virtual void Apply()
        {
            GraphicsDevice.BlendState = this.BlendState;
            GraphicsDevice.DepthStencilState = this.DepthStencilState;
            GraphicsDevice.RasterizerState = this.RasterizerState;
            for (int i = 0; i < NumberOfTextures; i++)
            {
                if (this.SamplerStates[i] != null) GraphicsDevice.SamplerStates[i] = this.SamplerStates[i];
            }

            IEffectMatrices effectMatrices = _effect as IEffectMatrices;
            if (effectMatrices != null)
            {
                effectMatrices.World = this.World;
                effectMatrices.View = this.View;
                effectMatrices.Projection = this.Projection;
            }

            _effect.CurrentTechnique.Passes[0].Apply();
        }
        
        public void ApplyTextures(ITexture[] textures)
        {
            for (int i = 0; i < NumberOfTextures; i++)
            {
                if (textures == null) GraphicsDevice.Textures[i] = null;
                else if (i >= textures.Length) GraphicsDevice.Textures[i] = null;
                else if (textures[i] == null) GraphicsDevice.Textures[i] = null;
                else GraphicsDevice.Textures[i] = textures[i].Texture;
            }
        }

        public void SetVertices<T>(IPhoton photon, T[] data) where T : struct
        {
            ((DeviceContextMG)DeviceContext).PrimitiveType = this.PrimitiveType;
            DeviceContext.SetVertices(photon, data);
        }

        public void SetVertices<T>(IPhoton photon, T[] vertexData, int vertexOffset, int primitiveCount, VertexDeclaration vertexDeclaration) where T : struct
        {
            DeviceContext.PrimitiveType = this.PrimitiveType;            
            DeviceContext.SetVertices(photon, vertexData, vertexOffset, primitiveCount, vertexDeclaration);            
        }

        public void SetVertices<T>(IPhoton photon, int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride) where T : struct
        {
            DeviceContext.PrimitiveType = this.PrimitiveType;
            DeviceContext.SetVertices(photon, offsetInBytes, data, startIndex, elementCount, vertexStride);
        }

        public void SetVertices<T>(IPhoton photon, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount, VertexDeclaration vertexDeclaration) where T : struct
        {
            DeviceContext.PrimitiveType = this.PrimitiveType;
            DeviceContext.SetVertices(photon, vertexData, vertexOffset, numVertices, indexData, indexOffset, primitiveCount, vertexDeclaration);            
        }

        public void SetVertices(IPhoton photon, VertexBuffer vertexBuffer, int baseVertex, int minVertexIndex, int numVertices, IndexBuffer indexBuffer, int startIndex, int primitiveCount)
        {
            DeviceContext.PrimitiveType = this.PrimitiveType;
            DeviceContext.SetVertices(photon, vertexBuffer, baseVertex, minVertexIndex, numVertices, indexBuffer, startIndex, primitiveCount);            
        }

        public override string ToString()
        {
            return string.Format("Material {0}", this.GetType().Name);
        }
        
        #region Aether.Elementary.Serialization.IAetherSerialization Members

        public virtual void Save(IAetherWriter writer)
        {
            writer.WriteInt64("PrimitiveType", (int)PrimitiveType);
            SaveBlendState(writer, this.BlendState);
            SaveDepthStencilState(writer, this.DepthStencilState);
            SaveRasterizerState(writer, this.RasterizerState);
            SaveSamplerStates(writer, this.SamplerStates);
        }
        private void SaveBlendState(IAetherWriter writer, BlendState blendState)
        {
            writer.WriteString("Name", blendState.Name);
            writer.WriteString("BlendStateName", blendState.Name);
            writer.WriteInt64("AlphaBlendFunction", (int)blendState.AlphaBlendFunction);
            writer.WriteInt64("AlphaDestinationBlend", (int)blendState.AlphaDestinationBlend);
            writer.WriteInt64("AlphaSourceBlend", (int)blendState.AlphaSourceBlend);
            writer.WriteColor("BlendFactor", blendState.BlendFactor);
            writer.WriteInt64("ColorBlendFunction", (int)blendState.ColorBlendFunction);
            writer.WriteInt64("ColorDestinationBlend", (int)blendState.ColorDestinationBlend);
            writer.WriteInt64("ColorSourceBlend", (int)blendState.ColorSourceBlend);
            writer.WriteInt64("ColorWriteChannels", (int)blendState.ColorWriteChannels);
            writer.WriteInt64("ColorWriteChannels1", (int)blendState.ColorWriteChannels1);
            writer.WriteInt64("ColorWriteChannels2", (int)blendState.ColorWriteChannels2);
            writer.WriteInt64("ColorWriteChannels3", (int)blendState.ColorWriteChannels3);
            writer.WriteInt64("MultiSampleMask", (int)blendState.MultiSampleMask);
        }
        private void SaveDepthStencilState(IAetherWriter writer, DepthStencilState depthStencilState)
        {
            writer.WriteString("Name", depthStencilState.Name);
            writer.WriteInt64("CounterClockwiseStencilDepthBufferFail", (int)depthStencilState.CounterClockwiseStencilDepthBufferFail);
            writer.WriteInt64("CounterClockwiseStencilFail", (int)depthStencilState.CounterClockwiseStencilFail);
            writer.WriteInt64("CounterClockwiseStencilFunction", (int)depthStencilState.CounterClockwiseStencilFunction);
            writer.WriteInt64("CounterClockwiseStencilPass", (int)depthStencilState.CounterClockwiseStencilPass);
            writer.WriteBoolean("DepthBufferEnable", depthStencilState.DepthBufferEnable);
            writer.WriteInt64("DepthBufferFunction", (int)depthStencilState.DepthBufferFunction);
            writer.WriteBoolean("DepthBufferWriteEnable", depthStencilState.DepthBufferWriteEnable);
            writer.WriteInt64("ReferenceStencil", depthStencilState.ReferenceStencil);
            writer.WriteInt64("StencilDepthBufferFail", (int)depthStencilState.StencilDepthBufferFail);
            writer.WriteBoolean("StencilEnable", depthStencilState.StencilEnable);
            writer.WriteInt64("StencilFail", (int)depthStencilState.StencilFail);
            writer.WriteInt64("StencilFunction", (int)depthStencilState.StencilFunction);
            writer.WriteInt64("StencilMask", depthStencilState.StencilMask);
            writer.WriteInt64("StencilPass", (int)depthStencilState.StencilPass);
            writer.WriteInt64("StencilWriteMask", depthStencilState.StencilWriteMask);
            writer.WriteBoolean("TwoSidedStencilMode", depthStencilState.TwoSidedStencilMode);
        }
        private void SaveRasterizerState(IAetherWriter writer, RasterizerState rasterizerState)
        {
            writer.WriteString("Name", rasterizerState.Name);
            writer.WriteInt64("CullMode", (int)rasterizerState.CullMode);
            writer.WriteFloat("DepthBias", rasterizerState.DepthBias);
            writer.WriteInt64("FillMode", (int)rasterizerState.FillMode);
            writer.WriteBoolean("MultiSampleAntiAlias", rasterizerState.MultiSampleAntiAlias);
            writer.WriteBoolean("ScissorTestEnable", rasterizerState.ScissorTestEnable);
            writer.WriteFloat("SlopeScaleDepthBias", rasterizerState.SlopeScaleDepthBias);
        }
        private void SaveSamplerStates(IAetherWriter writer, SamplerState[] samplerStates)
        {
            int samplerStateCount = samplerStates.Length;
            for (int i = 0; i < samplerStateCount; i++)
            {
                if (samplerStates[i] == null)  
                {
                    samplerStateCount = i; 
                    break;
                }
            }
            writer.WriteInt32("SamplerStateCount", samplerStateCount);
            for (int i = 0; i < samplerStateCount; i++)
            {
                SamplerState samplerState = samplerStates[i];
                writer.WriteString("Name", samplerState.Name);
                writer.WriteInt64("AddressU", (int)samplerState.AddressU);
                writer.WriteInt64("AddressV", (int)samplerState.AddressV);
                writer.WriteInt64("AddressW", (int)samplerState.AddressW);
                writer.WriteInt64("Filter", (int)samplerState.Filter);
                writer.WriteInt64("MaxAnisotropy", samplerState.MaxAnisotropy);
                writer.WriteInt64("MaxMipLevel", samplerState.MaxMipLevel);
                writer.WriteFloat("MipMapLevelOfDetailBias", samplerState.MipMapLevelOfDetailBias);
            }
        }

        public virtual void Load(IAetherReader reader)
        {
            string str; Int64 i64;
            reader.ReadInt64("PrimitiveType", out i64); PrimitiveType = (PrimitiveType)i64;
            this.BlendState = LoadBlendState(reader);
            this.DepthStencilState = LoadDepthStencilState(reader);
            this.RasterizerState = LoadRasterizerState(reader);
            LoadSamplerStates(reader, this.SamplerStates);
        } 
        private BlendState LoadBlendState(IAetherReader reader)
        {
            BlendState blendState = new BlendState();
            string str; Int64 i64; Color col;
            reader.ReadString("Name", out str); blendState.Name = str;
            reader.ReadString("BlendStateName", out str); blendState.Name = str;
            reader.ReadInt64("AlphaBlendFunction", out i64); blendState.AlphaBlendFunction = (BlendFunction)i64;
            reader.ReadInt64("AlphaDestinationBlend", out i64); blendState.AlphaDestinationBlend = (Blend)i64;
            reader.ReadInt64("AlphaSourceBlend", out i64); blendState.AlphaSourceBlend = (Blend)i64;
            reader.ReadColor("BlendFactor", out col); blendState.BlendFactor = col;
            reader.ReadInt64("ColorBlendFunction", out i64); blendState.ColorBlendFunction = (BlendFunction)i64;
            reader.ReadInt64("ColorDestinationBlend", out i64); blendState.ColorDestinationBlend = (Blend)i64;
            reader.ReadInt64("ColorSourceBlend", out i64); blendState.ColorSourceBlend = (Blend)i64;
            reader.ReadInt64("ColorWriteChannels", out i64); blendState.ColorWriteChannels = (ColorWriteChannels)i64;
            reader.ReadInt64("ColorWriteChannels1", out i64); blendState.ColorWriteChannels1 = (ColorWriteChannels)i64;
            reader.ReadInt64("ColorWriteChannels2", out i64); blendState.ColorWriteChannels2 = (ColorWriteChannels)i64;
            reader.ReadInt64("ColorWriteChannels3", out i64); blendState.ColorWriteChannels3 = (ColorWriteChannels)i64;
            reader.ReadInt64("MultiSampleMask", out i64); blendState.MultiSampleMask = (int)i64;
            return blendState;
        }
        private DepthStencilState LoadDepthStencilState(IAetherReader reader)
        {
            DepthStencilState depthStencilState = new DepthStencilState();
            string str; Int64 i64; bool bl;
            reader.ReadString("Name", out str); depthStencilState.Name = str;
            reader.ReadInt64("CounterClockwiseStencilDepthBufferFail", out i64); depthStencilState.CounterClockwiseStencilDepthBufferFail = (StencilOperation)i64;
            reader.ReadInt64("CounterClockwiseStencilFail", out i64); depthStencilState.CounterClockwiseStencilFail = (StencilOperation)i64;
            reader.ReadInt64("CounterClockwiseStencilFunction", out i64); depthStencilState.CounterClockwiseStencilFunction = (CompareFunction)i64;
            reader.ReadInt64("CounterClockwiseStencilPass", out i64); depthStencilState.CounterClockwiseStencilPass = (StencilOperation)i64;
            reader.ReadBoolean("DepthBufferEnable", out bl); depthStencilState.DepthBufferEnable = bl;
            reader.ReadInt64("DepthBufferFunction", out i64); depthStencilState.DepthBufferFunction = (CompareFunction)i64;
            reader.ReadBoolean("DepthBufferWriteEnable", out bl); depthStencilState.DepthBufferWriteEnable = bl;
            reader.ReadInt64("ReferenceStencil", out i64); depthStencilState.ReferenceStencil = (int)i64;
            reader.ReadInt64("StencilDepthBufferFail", out i64); depthStencilState.StencilDepthBufferFail = (StencilOperation)i64;
            reader.ReadBoolean("StencilEnable", out bl); depthStencilState.StencilEnable = bl;
            reader.ReadInt64("StencilFail", out i64); depthStencilState.StencilFail = (StencilOperation)i64;
            reader.ReadInt64("StencilFunction", out i64); depthStencilState.StencilFunction = (CompareFunction)i64;
            reader.ReadInt64("StencilMask", out i64); depthStencilState.StencilMask = (int)i64;
            reader.ReadInt64("StencilPass", out i64); depthStencilState.StencilPass = (StencilOperation)i64;
            reader.ReadInt64("StencilWriteMask", out i64); depthStencilState.StencilWriteMask = (int)i64;
            reader.ReadBoolean("TwoSidedStencilMode", out bl); depthStencilState.TwoSidedStencilMode = bl;
            return depthStencilState;
        }
        private RasterizerState LoadRasterizerState(IAetherReader reader)
        {
            RasterizerState rasterizerState = new RasterizerState();
            string str; Int64 i64; float flt; bool bl;
            reader.ReadString("Name", out str); rasterizerState.Name = str;
            reader.ReadInt64("CullMode", out i64); rasterizerState.CullMode = (CullMode)i64;
            reader.ReadFloat("DepthBias", out flt); rasterizerState.DepthBias = flt;
            reader.ReadInt64("FillMode", out i64); rasterizerState.FillMode = (FillMode)i64;
            reader.ReadBoolean("MultiSampleAntiAlias", out bl); rasterizerState.MultiSampleAntiAlias = bl;
            reader.ReadBoolean("ScissorTestEnable", out bl); rasterizerState.ScissorTestEnable = bl;
            reader.ReadFloat("SlopeScaleDepthBias", out flt); rasterizerState.SlopeScaleDepthBias = flt;
            return rasterizerState;
        }
        private void LoadSamplerStates(IAetherReader reader, SamplerState[] samplerStates)
        {
            int samplerStateCount;
            reader.ReadInt32("SamplerStateCount", out samplerStateCount);
            for (int i = 0; i < samplerStateCount; i++)
            {
                SamplerState samplerState = new SamplerState();
                string str; Int64 i64; float flt;
                reader.ReadString("Name", out str); samplerState.Name = str;
                reader.ReadInt64("AddressU", out i64); samplerState.AddressU = (TextureAddressMode)i64;
                reader.ReadInt64("AddressV", out i64); samplerState.AddressV = (TextureAddressMode)i64;
                reader.ReadInt64("AddressW", out i64); samplerState.AddressW = (TextureAddressMode)i64;
                reader.ReadInt64("Filter", out i64); samplerState.Filter = (TextureFilter)i64;
                reader.ReadInt64("MaxAnisotropy", out i64); samplerState.MaxAnisotropy = (int)i64;
                reader.ReadInt64("MaxMipLevel", out i64); samplerState.MaxMipLevel = (int)i64;
                reader.ReadFloat("MipMapLevelOfDetailBias", out flt); samplerState.MipMapLevelOfDetailBias = flt;
                samplerStates[i] = samplerState;
            }
        }

        #endregion

    }
}
