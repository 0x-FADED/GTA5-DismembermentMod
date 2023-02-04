using GTA;
using GTA.Math;
using GTA.Native;

namespace Dismemberment
{
    internal static class Utils
    {      
        internal static bool ExcludedPeds(this Ped ped)
        {
            var pedType = Function.Call<int>(Hash.GET_PED_TYPE, ped);
            if(pedType == 0 || pedType == 1 || pedType == 2 || pedType == 3 || pedType == 28)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static void RequestPTFXLibrary(string lib)
        {
            if (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, lib))
            {
                Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, lib);
            }
        }
        internal static void RemovePTFXLibrary(string lib)
        {
            if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, lib))
            {
                Function.Call(Hash.REMOVE_NAMED_PTFX_ASSET, lib);
            }
        }
        internal static bool IsDLCInstalled()
        {
            return Function.Call<bool>(Hash.IS_DLC_PRESENT, 0x83E0D0E0);
        }

        internal static Ped CloneMe(this Ped ped, Vector3 coords, float heading)
        {
            Ped ped2 = Function.Call<Ped>(Hash.CLONE_PED, ped, heading, 0, 1);
            ped2.Position = coords;
            ped2.Heading = heading;
            return ped2;
        }
        internal static float GetPhysicsHeading(this Entity entity)
        {
            return Function.Call<float>(Hash.GET_ENTITY_HEADING_FROM_EULERS, entity);
        }
    }
}
