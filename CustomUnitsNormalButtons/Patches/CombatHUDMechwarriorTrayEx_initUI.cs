using BattleTech;
using BattleTech.UI;
using Harmony;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace CustomUnitsNormalButtons.Patches
{

 

    


    [HarmonyPatch]
    public static class CombatHUDMechwarriorTrayEx_initUI
    {

        public static bool Logged { get; set; } = true;  //Debug to avoid logging for the alpha test.
        public static bool Inited { get; set; }

        //Debug
        public static bool Prepare() => true;

        public static void Postfix(Object __instance)
        {

            try
            {

                //Debug
                //if (Inited) return;

                Type instanceType = __instance.GetType();

                CombatHUDMechwarriorTray mechWarriorTray = (CombatHUDMechwarriorTray)AccessTools.Field(instanceType, "mechWarriorTray").GetValue(__instance);
                GameObject activeButtonsLayoutGameObject = (GameObject)AccessTools.Field(instanceType, "ActiveButtonsLayout").GetValue(__instance);
                GameObject passiveButtonsLayoutGameObject = (GameObject)AccessTools.Field(instanceType, "PassiveButtonsLayout").GetValue(__instance);

                float trayRightPosition = mechWarriorTray.gameObject.GetComponent<RectTransform>().sizeDelta.x +
                    mechWarriorTray.gameObject.transform.position.x;


                RectTransform mwPortfolioLayoutGroupRect = ((Component)mechWarriorTray).gameObject.transform.FindRecursive("mwPortraitLayoutGroup") as RectTransform;

                Transform mwPortfolioLayoutGroupRectTransform = (Transform)mwPortfolioLayoutGroupRect;
                RectTransform activeButtonsLayoutRect = activeButtonsLayoutGameObject.GetComponent<RectTransform>();

                if (!Logged) Logger.LogJson(new
                {
                    title = "mwPortfolio",
                    mwPortfolioLayoutGroupRectTransform.position.x,
                    mwPortfolioLayoutGroupRectTransform.position.y,
                    deltaX = activeButtonsLayoutRect.sizeDelta.x,
                    deltaY = activeButtonsLayoutRect.sizeDelta.y,
                }, "mwPortfolio.json");

                Vector3 scale = new Vector3(.75f, .75f, 1);

                //Debug
                if (!Logged) LogObject("passive button layout before", passiveButtonsLayoutGameObject, null);

                //----Passive
                passiveButtonsLayoutGameObject.transform.localScale = scale;

                //--Y assign

                //NOTE - Size Delta doesn't seem to change.
                Vector3 yPosition = Vector3.zero;
                yPosition.y = mwPortfolioLayoutGroupRectTransform.position.y + mwPortfolioLayoutGroupRect.sizeDelta.y / 2f + activeButtonsLayoutRect.sizeDelta.y;

                Vector3 passiveBarPosition = Vector3.zero;
                passiveBarPosition.y = yPosition.y;
                passiveButtonsLayoutGameObject.transform.localPosition = passiveBarPosition;

                //--X assign

                //float middleX = component.sizeDelta.x / 2 + transform.position.x;
                //Debug - testing non scaled
                //float scaledWidth = activeButtonsLayoutRect.sizeDelta.x * .75f;
                float scaledWidth = activeButtonsLayoutRect.sizeDelta.x;

                float positionX;
                Vector3 existingPosition;

                positionX = trayRightPosition - scaledWidth;

                existingPosition = passiveButtonsLayoutGameObject.transform.position;
                passiveButtonsLayoutGameObject.transform.position = new Vector3(positionX, existingPosition.y, existingPosition.z);

                //Debug
                if (!Logged) LogObject("passive button layout after", passiveButtonsLayoutGameObject, null);

                //Debug
                if (!Logged) LogObject("active button layout before", activeButtonsLayoutGameObject, activeButtonsLayoutRect);
                
                //----Active
                activeButtonsLayoutGameObject.transform.localScale = scale;
                
                Vector3 activeBarPosition = Vector3.zero;
                activeButtonsLayoutGameObject.transform.localPosition = yPosition;


                //Debug
                //positionX = positionX - scaledWidth;
                positionX = positionX - scaledWidth *.75f;
                existingPosition = activeButtonsLayoutGameObject.transform.position;
                activeButtonsLayoutGameObject.transform.position = new Vector3(positionX, existingPosition.y, existingPosition.z);


                //Debug
                if (!Logged) LogObject("active button layout after", activeButtonsLayoutGameObject, null);

            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }
            finally
            {
                Logged = true;
                //Debug
                Inited = true;
            }

        }


        private static void LogObject(string title, GameObject activeButtonsLayout, RectTransform component)
        {

            string fileName = $"{title.Replace(" ", "")}.json";

            Logger.Log(JsonConvert.SerializeObject(new
                {

                    title,
                    activeButtonsLayout?.transform?.position,
                    activeButtonsLayout?.transform?.localPosition,
                    GameObject_position = activeButtonsLayout?.gameObject?.transform?.position,
                    component?.sizeDelta,
                    Component_localPositon = component?.localPosition,
                    Component_sizeDelta = component?.sizeDelta,
                    Component_localPosiion = component?.localPosition,
                    Component_position = component?.position
                }, Formatting.Indented, new Vector2Converter(), new Vector3Converter()),
                fileName
            );
        }

        public static MethodBase TargetMethod()
        {
            return AccessTools.TypeByName("CustomUnits.CombatHUDMechwarriorTrayEx").GetMethod("initUI");
        }


    }


    public static class CustomUnitsExtensions
    {
        /// <summary>
        /// From the custom units extensions
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="checkName"></param>
        /// <returns></returns>
        public static Transform FindRecursive(this Transform transform, string checkName)
        {
            foreach (Transform transform1 in transform)
            {
                if (transform1.name == checkName)
                    return transform1;
                Transform recursive = transform1.FindRecursive(checkName);
                if (recursive != null)
                    return recursive;
            }
            return (Transform)null;
        }
    }




}
