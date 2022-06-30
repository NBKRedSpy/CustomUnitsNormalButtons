using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomUnitsNormalButtons.Patches
{

    [HarmonyPatch()]
    public static class CombatHUDMechwarriorTrayEx_Buttons
    {

        //Debug
        public static bool Prepare() => false;


        public static void Postfix()
        {
            try
            {
                AccessTools.TypeByName("CustomUnits.CombatHUDMechwarriorTrayEx").GetField("BUTTONS_COUNT").SetValue(null, 5);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }

        }

        public static MethodBase TargetMethod()
        {
            try
            {
                return AccessTools.TypeByName("CustomUnits.CombatHUDMechwarriorTrayEx").GetConstructor(Array.Empty<Type>());
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }        
        }

    }
}
