using ChatterRebornSettings;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Collections.Generic;
using UnityEngine;

namespace ChatterReborn.Managers
{
    public class DevToolManager : ChatterManager<DevToolManager>
    {

        protected override void Setup()
        {
            //GameDataBlockUtils<EnemyDataBlock>.WriteHeaderToDisk();
            base.Setup();
        }


        private bool allowLogComponents = MiscSettings.Debug_DevToolLogger;
        public static void LogComponents(GameObject targetGameObject)
        {
            if (!Current.allowLogComponents)
            {
                return;
            }
            List<Component> allComponents = new List<Component>();
            var components = targetGameObject.GetComponents<Component>();
            if (components != null)
            {
                LogComponents(targetGameObject.name + " GetComponents", components);
            }

            var componentschldren = targetGameObject.GetComponentsInChildren<Component>();
            if (componentschldren != null)
            {
                LogComponents(targetGameObject.name + " GetComponentsInChildren", componentschldren);
            }

            var componentsParent = targetGameObject.GetComponentsInParent<Component>(false);
            if (componentsParent != null)
            {
                LogComponents(targetGameObject.name + " GetComponentsInParent", componentsParent);
            }

            Current.DebugPrint("<<<<<<<Line Break>>>>>>>>");
        }

        private static void LogComponents(string componentType, Il2CppArrayBase<Component> components)
        {
            List<string> message1 = new List<string>();
            foreach (var item in components)
            {
                message1.Add(item.GetIl2CppType().Name + " ,");
            }

            Current.DebugPrint(componentType + " : " + string.Concat(message1));
        }
    }
}
