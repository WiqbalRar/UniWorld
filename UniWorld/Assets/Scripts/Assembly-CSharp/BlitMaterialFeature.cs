using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlitMaterialFeature : ScriptableRendererFeature
{
	private class RenderPass : ScriptableRenderPass
	{
		private string profilingName;

		private Material material;

		private int materialPassIndex;

		private RenderTargetIdentifier sourceID;

		private RenderTargetHandle tempTextureHandle;

		public RenderPass(string profilingName, Material material, int passIndex)
		{
			this.profilingName = profilingName;
			this.material = material;
			materialPassIndex = passIndex;
			tempTextureHandle.Init("_TempBlitMaterialTexture");
		}

		public void SetSource(RenderTargetIdentifier source)
		{
			sourceID = source;
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = CommandBufferPool.Get(profilingName);
			RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
			cameraTargetDescriptor.depthBufferBits = 0;
			commandBuffer.GetTemporaryRT(tempTextureHandle.id, cameraTargetDescriptor, FilterMode.Bilinear);
			Blit(commandBuffer, sourceID, tempTextureHandle.Identifier(), material, materialPassIndex);
			Blit(commandBuffer, tempTextureHandle.Identifier(), sourceID);
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}

		public override void FrameCleanup(CommandBuffer cmd)
		{
			cmd.ReleaseTemporaryRT(tempTextureHandle.id);
		}
	}

	[Serializable]
	public class Settings
	{
		public Material material;

		public int materialPassIndex = -1;

		public RenderPassEvent renderEvent = RenderPassEvent.AfterRenderingOpaques;

		public bool mmainCameraOnly;
	}

	[SerializeField]
	private Settings settings = new Settings();

	private RenderPass renderPass;

	public Material Material => settings.material;

	public override void Create()
	{
		renderPass = new RenderPass(base.name, settings.material, settings.materialPassIndex);
		renderPass.renderPassEvent = settings.renderEvent;
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (!settings.mmainCameraOnly || renderingData.cameraData.camera.CompareTag("MainCamera"))
		{
			renderPass.SetSource(renderer.cameraColorTarget);
			renderer.EnqueuePass(renderPass);
		}
	}
}
