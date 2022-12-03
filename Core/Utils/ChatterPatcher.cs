using HarmonyLib;
using System;
using System.Reflection;

namespace ChatterReborn.Utils
{
    public class ChatterPatcher<T> : ChatterPatcherBase
    {

        public ChatterPatcher(string id) : base(id)
        {

        }



        public void Patch<S>(MethodToken methodName, HarmonyPatchType patchType)
        {
            base.Patch(typeof(T), typeof(S), methodName, patchType);
        }

        public void Patch<S>(MethodToken methodName, HarmonyPatchType patchType, BindingFlags bindingFlags)
        {
            base.Patch(typeof(T), typeof(S), methodName, patchType, MethodType.Normal, bindingFlags);
        }

        public void Patch<S>(MethodToken methodName, HarmonyPatchType patchType, BindingFlags bindingFlags, Type[] types)
        {
            base.Patch(typeof(T), typeof(S), methodName, patchType, MethodType.Normal, bindingFlags, types);
        }

        public void Patch<S>(MethodToken methodName, HarmonyPatchType patchType, MethodType methodType)
        {
            base.Patch(typeof(T), typeof(S), methodName, patchType, methodType);
        }

        public void Patch<S>(MethodToken methodName, HarmonyPatchType patchType, MethodType methodType, Type[] types)
        {
            base.Patch(typeof(T), typeof(S), methodName, patchType, methodType, BindingFlags.Default, types);
        }

        public void Patch<S>(MethodToken methodName, HarmonyPatchType patchType, MethodType methodType, BindingFlags bindingFlags)
        {
            base.Patch(typeof(T), typeof(S), methodName, patchType, methodType, bindingFlags);
        }
        public void Patch<S>(MethodToken methodName, HarmonyPatchType patchType, MethodType methodType, BindingFlags bindingFlags, Type[] types)
        {
            base.Patch(typeof(T), typeof(S), methodName, patchType, methodType, bindingFlags, types);
        }

    }
}
