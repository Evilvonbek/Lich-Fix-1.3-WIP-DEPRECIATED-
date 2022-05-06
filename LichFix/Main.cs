using System;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;
using UnityEngine.UI;
using HarmonyLib;
using Kingmaker.UnitLogic.Buffs.Blueprints;


namespace LichFix
{
    static class Main
    {
        public static Settings Settings;
        public static bool Enabled;


        static bool Load(UnityModManager.ModEntry modEntry)
        {
            Settings = Settings.Load<Settings>(modEntry);
            Settings.ModEntry = modEntry;

            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Range of explosion of Corrupted Blood", GUILayout.ExpandWidth(false));
            GUILayout.Space(10);
            Settings.corruptedBloodRange = (int)GUILayout.HorizontalSlider(Settings.corruptedBloodRange, 5, 15, GUILayout.Width(100f));
            GUILayout.Label(Settings.corruptedBloodRange + " Feet", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();
            //GUILayout.Label("Allow Lord beyond the Grave affect living companions when buffed with Blessing of Unlife", GUILayout.ExpandWidth(false));
            //GUILayout.Space(10);
            //Settings.allowLichAuraAffectLivingWithBlessingOfUnlife = GUILayout.Toggle(Settings.allowLichAuraAffectLivingWithBlessingOfUnlife, $" {Settings.allowLichAuraAffectLivingWithBlessingOfUnlife}", GUILayout.ExpandWidth(false));
            //GUILayout.EndHorizontal();
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
        }

        public static void Log(string msg)
        {
            Settings.ModEntry.Logger.Log(msg);
        }

        // Token: 0x0600000D RID: 13 RVA: 0x000025B6 File Offset: 0x000007B6
        public static void LogDebug(string msg)
        {
            Settings.ModEntry.Logger.Log(msg);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x000025C8 File Offset: 0x000007C8

        public static void LogPatch(string action, BlueprintBuff bp)
        {
            Main.Log(string.Format("{0}: {1} - {2}", action, bp.AssetGuid, bp.name));
        }

        // Token: 0x0600000F RID: 15 RVA: 0x000025EB File Offset: 0x000007EB
        public static void LogHeader(string msg)
        {
            Main.Log("--" + msg.ToUpper() + "--");
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00002607 File Offset: 0x00000807
        public static Exception Error(string message)
        {
            Main.Log(message);
            return new InvalidOperationException(message);
        }
    }
}