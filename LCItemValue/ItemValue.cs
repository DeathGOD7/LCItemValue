using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LCItemValue.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LCItemValue
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class ItemValue : BaseUnityPlugin
    {
        // Main Variables
        private const string modGUID = "com.github.DeathGOD7.LCItemValue";
        private const string modName = "ItemValue";
        private const string modVersion = "1.0.0";

        // Logger
        static ManualLogSource logger;
        internal static ManualLogSource getLogger()
        {
            return logger;
        }

        void Awake()
        {
            Harmony harmony = new Harmony(modGUID);

            logger = BepInEx.Logging.Logger.CreateLogSource(modName);
            logger.LogInfo("Item Value is loaded successfully.");

            harmony.PatchAll(typeof(ItemValue));
            harmony.PatchAll(typeof(ValueCounterPatcher));
        }
    }
}
