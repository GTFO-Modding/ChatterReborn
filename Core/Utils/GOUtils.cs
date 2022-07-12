using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace ChatterReborn.Utils
{
    public static class GOUtils
    {
        public static void SetChildToGO(GameObject child, GameObject parent)
        {
            child.transform.parent = parent.transform;
        }

        public static T SafeAddComponent<T>(bool dontDestroyOnLoad = true) where T : MonoBehaviour
        {
            if (!ClassInjector.IsTypeRegisteredInIl2Cpp<T>())
            {
                ChatterDebug.LogError("Cannot Add Component " + typeof(T).Name + " is hasn't been registered in IL2CPP!!");
                return null;
            }

            T t = new GameObject().AddComponent<T>();

            if (dontDestroyOnLoad)
            {
                GameObject.DontDestroyOnLoad(t);
            }

            return t;
        }

        public static T AddAbsoluteComponent<T>(this GameObject gameobject, bool skipDuplication = false) where T : MonoBehaviour
        {
            if (!ClassInjector.IsTypeRegisteredInIl2Cpp<T>())
            {
                ChatterDebug.LogError("Cannot Add Component " + typeof(T).Name + " is hasn't been registered in IL2CPP!!");
                return null;
            }

            if (!skipDuplication)
            {
                if (gameobject.GetComponent<T>() != null)
                {
                    return null;
                }
            }
            

            return gameobject.AddComponent<T>();
        }

        public static T GetAbsoluteComponent<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            T comp = gameObject.GetComponent<T>();
            if (comp == null)
            {
                comp = gameObject.GetComponentInParent<T>();
            }
            return comp;
        }

        public static bool HasComponent<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            return gameObject.GetComponent<T>() != null;
        }

        public static bool HasComponentInParent<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            return gameObject.GetComponentInParent<T>() != null;
        }

        public static bool HasComponentInChildren<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            return gameObject.GetComponentInChildren<T>() != null;
        }
    }
}
