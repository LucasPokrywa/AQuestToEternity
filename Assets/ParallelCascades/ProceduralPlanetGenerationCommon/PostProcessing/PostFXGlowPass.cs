using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.PostProcessing
{
    public class PostFXGlowPass : ScriptableRenderPass
    {
        Material m_BlitMaterial;

        public void Setup(Material material)
        {
            m_BlitMaterial = material;

            // The pass will read the current color texture. That needs to be an intermediate texture. It's not supported to use the BackBuffer as input texture. 
            // By setting this property, URP will automatically create an intermediate texture. 
            requiresIntermediateTexture = true;
        }
        
        class PassData
        {
            public TextureHandle sourceTexture;
            public TextureHandle destinationTexture;
            public Material material;
        }
        
        private class CopyPassData
        {
            internal TextureHandle inputTexture;
        }
        
        private static MaterialPropertyBlock s_SharedPropertyBlock = new MaterialPropertyBlock();

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            // UniversalResourceData contains all the texture handles used by the renderer, including the active color and depth textures
            // The active color and depth textures are the main color and depth buffers that the camera renders into
            var resourceData = frameData.Get<UniversalResourceData>();

            // This should never happen since we set m_Pass.requiresIntermediateTexture = true;
            // Unless you set the render event to AfterRendering, where we only have the BackBuffer. 
            if (resourceData.isActiveTargetBackBuffer)
            {
                Debug.LogWarning(
                    $"Skipping render pass. PostFXGlowPass requires an intermediate ColorTexture, we can't use the BackBuffer as a texture input.");
                return;
            }
            
            // The destination texture is created here, 
            // the texture is created with the same dimensions as the active color texture
            TextureHandle source = resourceData.activeColorTexture;
            
            var destinationDesc = renderGraph.GetTextureDesc(resourceData.cameraColor);
            destinationDesc.name = "_CameraColorFullScreenPass";
            destinationDesc.clearBuffer = false;
            
            TextureHandle destination = renderGraph.CreateTexture(destinationDesc);

            using (var builder = renderGraph.AddRasterRenderPass<CopyPassData>("Copy Color Full Screen", out var passData, profilingSampler))
            {
                passData.inputTexture = source;
                builder.UseTexture(passData.inputTexture, AccessFlags.Read);

                builder.SetRenderAttachment(destination, 0, AccessFlags.Write);

                builder.SetRenderFunc((CopyPassData data, RasterGraphContext rgContext) =>
                {
                 ExecuteCopyColorPass(rgContext.cmd, data.inputTexture);
                });
            }
            
            //Swap for next pass;
            source = destination;
            destination = resourceData.activeColorTexture;
            
            using (var builder =
                   renderGraph.AddRasterRenderPass<PassData>(passName, out var passData, profilingSampler))
            {
                passData.material = m_BlitMaterial;
                passData.sourceTexture = source;
                
                if (passData.sourceTexture.IsValid())
                {
                    builder.UseTexture(passData.sourceTexture);
                }

                Debug.Assert(resourceData.cameraOpaqueTexture.IsValid());
                builder.UseTexture(resourceData.cameraOpaqueTexture);

                Debug.Assert(resourceData.cameraDepthTexture.IsValid());
                builder.UseTexture(resourceData.cameraDepthTexture);

                builder.SetRenderAttachment(destination, 0);

                builder.SetRenderAttachmentDepth(resourceData.activeDepthTexture);
                builder.SetRenderFunc((PassData data, RasterGraphContext rgContext) =>
                {
                    ExecuteMainPass(rgContext.cmd, data.sourceTexture, data.material);
                });            
            }
        }
        
        private static void ExecuteCopyColorPass(RasterCommandBuffer cmd, RTHandle sourceTexture)
        {
            Blitter.BlitTexture(cmd, sourceTexture, new Vector4(1, 1, 0, 0), 0.0f, false);
        }
        
        private static void ExecuteMainPass(RasterCommandBuffer cmd, RTHandle sourceTexture, Material material)
        {
            s_SharedPropertyBlock.Clear();
            if (sourceTexture != null)
                s_SharedPropertyBlock.SetTexture(Shader.PropertyToID("_BlitTexture"), sourceTexture);

            // We need to set the "_BlitScaleBias" uniform for user materials with shaders relying on core Blit.hlsl to work
            s_SharedPropertyBlock.SetVector(Shader.PropertyToID("_BlitScaleBias"), new Vector4(1, 1, 0, 0));

            cmd.DrawProcedural(Matrix4x4.identity, material, 0, MeshTopology.Triangles, 3, 1, s_SharedPropertyBlock);
        }
    }
}