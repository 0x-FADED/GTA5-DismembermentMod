using GTA;
using GTA.Math;
using GTA.Native;

namespace Dismemberment
{
    public static class Utils
    {
        public static int GetLastDamageBone(this Ped ped)
        {
            unsafe
            {
                int boneID;
                Function.Call<bool>(Hash.GET_PED_LAST_DAMAGE_BONE, ped, &boneID);
                return (int)(Bone)boneID;
            }
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
