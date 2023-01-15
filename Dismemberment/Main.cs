using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;

namespace Dismemberment
{
    public class Main : Script
    {
        [DllImport("DismembermentASI.asi", CharSet = CharSet.Auto)]
        private static extern void AddBoneDraw(int handle, int start, int end);

        [DllImport("DismembermentASI.asi", CharSet = CharSet.Auto)]
        private static extern void RemoveBoneDraw(int handle);

        public Main()
        {
            if (Utils.IsDLCinstalled())
            {
                caps = new List<Prop>();
                chunks = new List<Prop>();
                rand = new Random();
                Dismemberment.Settings.LoadSettings();
                dismembermentWpns = File.ReadAllLines("scripts/DismembermentWeapons.cfg");
                Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_solomon3");
                Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_fbi1");
                Tick += OnTick;
                Aborted += OnAborted;
            }
            else
            {
                Screen.ShowHelpText("~b~dismemberment.rpf ~r~not found! ~w~please re-install the mod correctly", -1, false, true);
                Notification.Show("~b~dismemberment.rpf ~r~not found! ~w~please re-install the mod correctly",false);
            }
        }

        private void OnAborted(object sender, EventArgs e)
        {
            if(Screen.IsHelpTextDisplayed)
            {
                Screen.ClearHelpText();
            }
            foreach (Prop prop in caps)
            {
                prop.Delete();
            }
            foreach (Prop prop2 in chunks)
            {
                prop2.Delete();
            }
        }
        // gotta figure out why ped.Bones.LastDamaged from SHVDN3 doesnt work here
        private void OnTick(object sender, EventArgs e)
        {
            foreach (Ped ped in World.GetNearbyPeds(Game.Player.Character, 150f))
            {
                if (ped.GetLastDamageBone() != 0 && !ped.WasKilledByStealth && !ped.WasKilledByTakedown && Function.Call<int>(Hash.GET_PED_TYPE, ped) != 28)
                {
                    foreach (string text in dismembermentWpns)
                    {
                        if (ped.HasBeenDamagedBy((WeaponHash)rage.atHashString.GetHashKey(text)))
                        {
                            Dismember(ped, ped.GetLastDamageBone(), -1);
                        }
                    }
                    ped.ClearLastWeaponDamage();
                    ped.Bones.ClearLastDamaged();
                }
            }
            foreach (Prop prop in caps)
            {
                if (!prop.IsAttached())
                {
                    prop.Delete();
                }
                if (prop == null && !prop.Exists())
                {
                    caps.Remove(prop);
                }
            }
            if (chunkTimer > 0 && Game.GameTime >= chunkTimer)
            {
                for (int k = 0; k < chunks.Count; k++)
                {
                    chunks[k].MarkAsNoLongerNeeded();
                }
                chunkTimer = 0;
            }
            if (chunks.Count > 50)
            {
                chunks[0].Delete();
            }
            for (int l = 0; l < chunks.Count; l++)
            {
                if (!chunks[l].Exists())
                {
                    chunks.RemoveAt(l);
                }
            }
        }

        private void Dismember(Ped ped, int start, int end)
        {
            if (!ped.IsDead && !ped.HasBeenDamagedByAnyMeleeWeapon())
            {
                return;
            }
            if (Dismemberment.Settings.dismemberTorso || (start != 57597 && start != 23553 && start != 24816 && start != 24817 && start != 24818))
            {
                ped.IsPersistent = true;
                if (ped.IsAlive || !ped.IsRagdoll)
                {
                    ped.CanRagdoll = true;
                    ped.Kill();
                }
                Ped ped2 = null;
                if (start == 31086 || start == 39317)
                {
                    Function.Call(Hash.KNOCK_OFF_PED_PROP, ped, 0, 1, 1, 1);
                    Function.Call(Hash.KNOCK_OFF_PED_PROP, ped, 1, 1, 1, 1);
                    Function.Call(Hash.STOP_PED_SPEAKING, ped, true);
                    Function.Call(Hash.DISABLE_PED_PAIN_AUDIO, ped, true);
                    if (ped.HasBeenDamagedByAnyMeleeWeapon())
                    {
                        ped2 = ped.CloneMe(new Vector3(ped.Position.X, ped.Position.Y, ped.Position.Z - 0.5f), ped.GetPhysicsHeading());
                        Function.Call(Hash.STOP_PED_SPEAKING, ped2, true);
                        Function.Call(Hash.DISABLE_PED_PAIN_AUDIO, ped2, true);
                        ped2.Kill();
                        Dismember(ped2, 11816, 39317);
                        ped2.Heading = ped.GetPhysicsHeading();
                        ped2.IsPersistent = true;
                    }
                    else
                    {
                        Prop prop = World.CreateProp("cap_head", ped.Position, false, false);
                        prop.AttachTo(ped.Bones[Bone.SkelSpine3], new Vector3(0.294f, 0.01f, -0.025f), new Vector3(0f, -90f, 160f));
                        caps.Add(prop);
                    }
                }
                else if (Dismemberment.Settings.dismemberTorso && (start == 57597 || start == 23553 || start == 24816 || start == 24817 || start == 24818))
                {
                    Function.Call(Hash.KNOCK_OFF_PED_PROP, ped, 0, 1, 1, 1);
                    Function.Call(Hash.KNOCK_OFF_PED_PROP, ped, 1, 1, 1, 1);
                    ped2 = ped.CloneMe(new Vector3(ped.Position.X, ped.Position.Y, ped.Position.Z - 0.5f), ped.GetPhysicsHeading());
                    Function.Call(Hash.STOP_PED_SPEAKING, ped2, true);
                    Function.Call(Hash.DISABLE_PED_PAIN_AUDIO, ped2, true);
                    ped2.Kill();
                    Dismember(ped2, 11816, -1);
                    ped2.Heading = ped.GetPhysicsHeading();
                    ped2.IsPersistent = true;
                }
                else if (start == 63931)
                {
                    Prop prop2 = World.CreateProp("cap_calf", ped.Position, false, false);
                    prop2.AttachTo(ped.Bones[Bone.SkelLeftThigh], new Vector3(0.25f, -0.03f, 0f), new Vector3(-40f, 90f, 0f));
                    caps.Add(prop2);
                }
                else if (start == 36864)
                {
                    Prop prop3 = World.CreateProp("cap_calf", ped.Position, false, false);
                    prop3.AttachTo(ped.Bones[Bone.SkelRightThigh], new Vector3(0.25f, -0.03f, 0f), new Vector3(-40f, 90f, 0f));
                    caps.Add(prop3);
                }
                else if (start == 58271)
                {
                    Prop prop4 = World.CreateProp("cap_calf", ped.Position, false, false);
                    prop4.AttachTo(ped.Bones[Bone.SkelLeftThigh], new Vector3(0.05f, 0f, 0f), new Vector3(0f, 90f, 0f));
                    caps.Add(prop4);
                }
                else if (start == 51826)
                {
                    Prop prop5 = World.CreateProp("cap_calf", ped.Position, false, false);
                    prop5.AttachTo(ped.Bones[Bone.SkelRightThigh], new Vector3(0.05f, 0f, 0f), new Vector3(0f, 90f, 0f));
                    caps.Add(prop5);
                }
                else if (start == 14201)
                {
                    Prop prop6 = World.CreateProp("cap_upperarm", ped.Position, false, false);
                    prop6.AttachTo(ped.Bones[Bone.SkelLeftCalf], new Vector3(0.3f, 0.01f, 0f), new Vector3(120f, 20f, 90f));
                    caps.Add(prop6);
                }
                else if (start == 52301)
                {
                    Prop prop7 = World.CreateProp("cap_upperarm", ped.Position, false, false);
                    prop7.AttachTo(ped.Bones[Bone.SkelRightCalf], new Vector3(0.3f, 0.01f, 0f), new Vector3(120f, 20f, 90f));
                    caps.Add(prop7);
                }
                else if (start == 45509 || start == 64729 || start == 61163)
                {
                    Prop prop8 = World.CreateProp("cap_shoulder", ped.Position, false, false);
                    prop8.AttachTo(ped.Bones[Bone.SkelSpine3], new Vector3(0.2f, -0.03f, 0.15f), new Vector3(-170f, -180f, -50f));
                    caps.Add(prop8);
                }
                else if (start == 40269 || start == 10706 || start == 28252)
                {
                    Prop prop9 = World.CreateProp("cap_shoulder", ped.Position, false, false);
                    prop9.AttachTo(ped.Bones[Bone.SkelSpine3], new Vector3(0.18f, -0.03f, -0.15f), new Vector3(0f, 180f, 0f));
                    caps.Add(prop9);
                }
                else if (start == 18905)
                {
                    Prop prop10 = World.CreateProp("cap_shoulder", ped.Position, false, false);
                    prop10.AttachTo(ped.Bones[Bone.SkelLeftUpperArm], new Vector3(0.3f, 0.02f, 0.04f), new Vector3(0f, 80f, 0f));
                    caps.Add(prop10);
                }
                else if (start == 57005)
                {
                    Prop prop11 = World.CreateProp("cap_shoulder", ped.Position, false, false);
                    prop11.AttachTo(ped.Bones[Bone.SkelRightUpperArm], new Vector3(0.3f, 0.02f, 0.04f), new Vector3(0f, 80f, 0f));
                    caps.Add(prop11);
                }
                int num = rand.Next(5, 21);
                for (int i = 0; i < num; i++)
                {
                    Prop prop12 = World.CreateProp("p_brain_chunk_s", ped.Bones[(Bone)start].Position, false, false);
                    chunks.Add(prop12);
                }
                chunkTimer = Game.GameTime + 2000;
                if (start != 31086 && start != 39317 && Dismemberment.Settings.pedPainSound)
                {
                    Function.Call(Hash.STOP_CURRENT_PLAYING_SPEECH, ped);
                    Function.Call(Hash.PLAY_AMBIENT_SPEECH_FROM_POSITION_NATIVE,
                        "ON_FIRE",
                        "WAVELOAD_PAIN_" + ped.Gender.ToString().ToUpper(),
                        ped.Position.X,
                        ped.Position.Y,
                        ped.Position.Z,
                        "SPEECH_PARAMS_FORCE_SHOUTED"
                    );
                }
                else
                {
                    Function.Call(Hash.STOP_CURRENT_PLAYING_SPEECH, ped);
                }
                AddBoneDraw(ped.Handle, start, end);
                Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_solomon3");
                Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_ON_PED_BONE,
                    "scr_trev4_747_blood_impact", ped, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, start, 0.3f,
                    false, false, false
                );
                Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_fbi1");
                Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_ON_PED_BONE,
                    "scr_fbi_autopsy_blood", ped, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, start, 0.45f,
                    false, false, false
                );
                ped.MarkAsNoLongerNeeded();
                if (ped2 != null && ped2.Exists())
                {
                    ped2.MarkAsNoLongerNeeded();
                }
            }
        }

        private List<Prop> caps = null;

        private List<Prop> chunks = null;

        private readonly string[] dismembermentWpns;

        private int chunkTimer;

        private Random rand = null;
    }
}