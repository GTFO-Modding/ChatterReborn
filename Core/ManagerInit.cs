using ChatterReborn.Components;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ChatterReborn
{
    public class ManagerInit
    {
        public static void SetupAllMangers()
        {
            if (_Setup) return;
            _Setup = true;


            ManagerHandler = new GameObject().AddComponent<ManagerHandler>();
            GameObject.DontDestroyOnLoad(ManagerHandler);
            Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where(type =>
            {
                Type ichatterManagerType = typeof(IChatterManager);
                return type.GetInterface(ichatterManagerType.Name) == ichatterManagerType & type.BaseType?.IsGenericType & type.BaseType?.GenericTypeArguments.Length > 0 ?? type.BaseType?.GenericTypeArguments.ElementAtOrDefault(0) == type;
            }).ToArray();
            bool firstSetup = true;
        BeginSetup:
            for (int i = 0; i < types.Length; i++)
            {
                Type manager_type = types[i];
                PropertyInfo propertyInfo = manager_type.BaseType.GetProperty("Current", BindingFlags.Static | BindingFlags.Public);
                IChatterManager manager = propertyInfo.GetValue(null) as IChatterManager;
                if (manager != null)
                {
                    manager.Initialize(firstSetup);
                }
                else
                {
                    ChatterDebug.LogError(manager_type + " is not a manager!");
                }
            }

            if (firstSetup)
            {
                firstSetup = false;
                goto BeginSetup;
            }

        }

        public static ManagerHandler ManagerHandler { get; private set; }

        private static bool _Setup = false;
    }
}
