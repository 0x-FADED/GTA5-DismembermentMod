using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Drawing;

namespace Dismemberment
{
    public static class Utils
    {
        public static bool IsDLCinstalled()
        {
            return Function.Call<bool>(Hash.IS_DLC_PRESENT, rage.atHashString.GetHashKey("dismemberment"));
        }

        public static int GetPedType(this Ped ped)
        {
            return Function.Call<int>(Hash.GET_PED_TYPE, ped);
        }

        public static int GetLastDamageBone(this Ped ped)
        {
            var outputArgument = new OutputArgument();
            Function.Call<int>(Hash.GET_PED_LAST_DAMAGE_BONE, ped, outputArgument);
            return outputArgument.GetResult<int>();
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
