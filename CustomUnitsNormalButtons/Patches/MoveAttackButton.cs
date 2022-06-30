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
    [HarmonyPatch(typeof(CombatHUD), nameof(CombatHUD.Init), new Type[] { typeof(CombatGameState)})]
    public static class MoveAttackButton
    {

        //Debug
        public static bool Prepare() => false;

        public static void Postfix(CombatHUD __instance)
        {
            RectTransform transform = __instance.AttackModeSelector.GetComponent<RectTransform>();

            Vector3 location = transform.localPosition;
            location.y -= 100;
            transform.localPosition = location; 

        }
        
    }
}
