using GTA;
using GTA.Math;
using GTA.Native;
using System.Xml.Schema;

namespace Dismemberment
{
    public static class Utils
    {
        public static void RequestPTFXLibrary(string lib)
        {
            if (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, lib))
            {
                Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, lib);
            }
        }
        public static void RemovePTFXLibrary(string lib)
        {
            if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, lib))
            {
                Function.Call(Hash.REMOVE_NAMED_PTFX_ASSET, lib);
            }
        }

        public static bool IsDLCInstalled()
        {
            return Function.Call<bool>(Hash.IS_DLC_PRESENT, Game.GenerateHash("dismemberment"));
        }

        public static Ped CloneMe(this Ped ped, Vector3 coords, float heading)
        {
            Ped ped2 = Function.Call<Ped>(Hash.CLONE_PED, ped, heading, 0, 1);
            ped2.Position = coords;
            ped2.Heading = heading;
            return ped2;
        }
        public static float GetPhysicsHeading(this Entity entity)
        {
            return Function.Call<float>(Hash.GET_ENTITY_HEADING_FROM_EULERS, entity);
        }
    }
}
