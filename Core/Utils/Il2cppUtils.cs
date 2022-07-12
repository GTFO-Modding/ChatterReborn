using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Collections.Generic;

namespace ChatterReborn.Utils
{


    public static class Il2cppUtils
    {
        public static Il2CppSystem.Collections.Generic.List<T> ToCppList<T>(List<T> list)
        {
            Il2CppSystem.Collections.Generic.List<T> cpplist = new Il2CppSystem.Collections.Generic.List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                T item = list[i];
                cpplist.Add(item);
            }
            return cpplist;
        }
        public static List<T> ToSystemList<T>(this Il2CppSystem.Collections.Generic.List<T> cpplist)
        {
            List<T> list = new List<T>();

            for (int i = 0; i < cpplist.Count; i++)
            {
                T item = cpplist[i];
                list.Add(item);
            }

            return list;
        }

        public static bool Convert<T, C>(this T from, out C isthis) where T : Il2CppObjectBase where C : Il2CppObjectBase
        {
            isthis = from.TryCast<C>();
            return isthis != null;
        }

        public static bool IsCppType<C>(this Il2CppSystem.Object from) where C : Il2CppObjectBase
        {
            return Il2CppType.Of<C>().Equals(from.GetIl2CppType());
        }

        public static Dictionary<TKey, TValue> ToSystemDictionary<TKey, TValue>(Il2CppSystem.Collections.Generic.Dictionary<TKey, TValue> CppDictionary)
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

            foreach (TKey tkey in CppDictionary.Keys)
            {
                TValue value;
                if (CppDictionary.TryGetValue(tkey, out value))
                {
                    dictionary.Add(tkey, value);
                }
            }
            return dictionary;
        }

        public static T[] ToArray<T>(Il2CppArrayBase<T> cppArray)
        {
            T[] t = new T[cppArray.Length];
            for (int i = 0; i < cppArray.Length; i++)
            {
                t[i] = cppArray[i];
            }
            return t;
        }

    }

}
