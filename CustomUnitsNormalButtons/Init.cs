using Harmony;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomUnitsNormalButtons
{
    public static class HarmonyInit
    {
        public static void Init(string directory, string settingsJSON)
        {
            Core.ModSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<ModSettings>(settingsJSON);

            Logger.Log("Init called with new");

            var harmony = HarmonyInstance.Create("io.github.nbk_redspy.CustomUnitsNormalButtons");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
