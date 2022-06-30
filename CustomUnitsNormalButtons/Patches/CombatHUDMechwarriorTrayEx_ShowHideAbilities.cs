using BattleTech;
using BattleTech.UI;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace CustomUnitsNormalButtons.Patches
{
    [HarmonyPatch]
    internal class CombatHUDMechwarriorTrayEx_ShowHideAbilities
    {
        //Debug
        public static bool Prepare() => true;

        public static void Postfix(Object __instance)
        {
            try
            {

                Type instanceType = __instance.GetType();

                CombatHUD combatHUD = (CombatHUD)AccessTools.Field(instanceType, "HUD").GetValue(__instance);

                if (combatHUD?.SelectedActor == null) return;

                GameObject passiveLayout = (GameObject)AccessTools.Field(instanceType, "PassiveButtonsLayout")
                    .GetValue(__instance);

                GameObject activeLayout = (GameObject)AccessTools.Field(instanceType, "ActiveButtonsLayout")
                    .GetValue(__instance);

                //todo:  probably cache.  I'm assuming the ui will be rebuilt with new objects on some init.

                passiveLayout.SetActive(true);
                activeLayout.SetActive(true);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }


        public static IEnumerable<MethodBase> TargetMethods()
        {
            try
            {
                List<string> targetMethodNames = new List<string>()
                {
                    "ShowActiveAbilities",
                    "ShowPassiveAbilities",
                    "HideActiveAbilities",
                    "HidePassiveAbilities",
                    "Update",
                };

                IEnumerable<MethodBase> matchedMethods = AccessTools.TypeByName("CustomUnits.CombatHUDMechwarriorTrayEx")
                    .GetMethods(AccessTools.all)
                    .Where(x => targetMethodNames.Contains(x.Name)).ToList();

                return matchedMethods;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }

    }
}
