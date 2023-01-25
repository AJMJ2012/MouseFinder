using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace MouseFinder {
	[Label("Client Config")]
	public class ClientConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ClientSide;
		public static ClientConfig Instance;

		[Label("Method")]
		[Range(0, 5)]
		[Increment(1)]
		[DefaultValue(1)]
		[Slider]
		public int Method = 1;

		[Label("Alpha (percent)")]
		[Range(0f, 100f)]
		[Increment(1f)]
		[DefaultValue(100f)]
		[Slider]
		public float Alpha = 100f;

		[Label("Scale (percent)")]
		[Range(50f, 100f)]
		[Increment(1f)]
		[DefaultValue(100f)]
		[Slider]
		public float Scale = 100f;

	}

	public static class Config {
		public static ClientConfig Client => ClientConfig.Instance;
	}
}