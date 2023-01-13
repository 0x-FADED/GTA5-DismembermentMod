using GTA;

namespace Dismemberment
{
	public class Settings : Script
	{
		public Settings()
		{
			iniFile = ScriptSettings.Load("scripts\\Dismemberment.toml");
            dismemberTorso = iniFile.GetValue("Settings", "bDismemberTorso", true);
            pedPainSound = iniFile.GetValue("Settings", "bPedPainSound", true);
		}

		private ScriptSettings iniFile;

		public static bool dismemberTorso;

		public static bool pedPainSound;
	}
}
