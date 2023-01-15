using GTA;

namespace Dismemberment
{
	public static class Settings
	{
		public static void LoadSettings()
		{
			toml = ScriptSettings.Load("scripts\\Dismemberment.toml");
            dismemberTorso = toml.GetValue("Settings", "bDismemberTorso", true);
            pedPainSound = toml.GetValue("Settings", "bPedPainSound", true);
		}

		private static ScriptSettings toml;

		public static bool dismemberTorso;

		public static bool pedPainSound;
	}
}
