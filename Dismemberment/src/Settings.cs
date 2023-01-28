using GTA;

namespace Dismemberment
{
    internal static class ModSettings
    {
        internal static void LoadSettings()
        {
            toml = ScriptSettings.Load("scripts\\Dismemberment.toml");
            dismemberTorso = toml.GetValue("Settings", "bDismemberTorso", true);
            pedPainSound = toml.GetValue("Settings", "bPedPainSound", true);
        }

        private static ScriptSettings toml;

        internal static bool dismemberTorso;

        internal static bool pedPainSound;
    }
}
