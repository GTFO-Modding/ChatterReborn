using ChatterReborn.Attributes;
using ChatterReborn.Data;
using HarmonyLib;
using System;
using System.Reflection;

namespace ChatterReborn.Utils
{
    public abstract class ChatterPatcherBase
    {

        public ChatterPatcherBase(string id)
        {
            m_harmonyInstance = new Harmony("Harmony_" + id);
            m_logger = new DebugLoggerObject(id);
        }



        protected void Patch(Type myclass, Type classType, MethodTokenName methodName, HarmonyPatchType patchType, MethodType methodType = MethodType.Normal ,BindingFlags binding = BindingFlags.Default, Type[] types = null)
        {

            MethodInfo methodInfo = null;

            string methodNameLog = "[" + classType + "." + methodName + "]";
            try
            {
                if (types == null)
                {
                    if (methodType == MethodType.Setter)
                    {
                        methodInfo = classType.GetProperty(methodName, binding).GetSetMethod();
                    }
                    else if (methodType == MethodType.Getter)
                    {
                        methodInfo = classType.GetProperty(methodName, binding).GetGetMethod();
                    }
                    else
                    {
                        methodInfo = classType.GetMethod(methodName, binding);
                    }

                }
                else
                {
                    methodInfo = classType.GetMethod(methodName, binding, null, types, null);
                }
            }
            catch(Exception e)
            {
                this.m_logger.DebugPrint("ERROR: Fatal error has occurred when patching " + methodNameLog + "\nERROR MESSAGE : " + e.Message, eLogType.Error);
                return;
            }
            



            if (methodInfo == null)
            {
                m_logger.DebugPrint("FAILED: Patching " + methodNameLog + " MethodInfo has returned null!", eLogType.Error);
                return;
            }



            string myFullMethodName = string.Empty;
            if (patchType == HarmonyPatchType.Postfix)
            {
                myFullMethodName = classType.Name + "__" + methodName + "__Postfix";
            }
            else
            {
                myFullMethodName = classType.Name + "__" + methodName + "__Prefix";
            }

            MethodInfo myMethodInfo = myclass.GetMethod(myFullMethodName, BindingFlags.NonPublic | BindingFlags.Static);

            

            if (methodName.HasToken)
            {
                if (MethodTokenUtils.TryToGetMethodInfoByTokenID(methodName.m_tokenID, out var tokenedMethodInfo))
                {
                    this.m_logger.DebugPrint("myMethodName " + myFullMethodName + " Has gotten overriden Token methodInfo instead with id -> " + methodName.m_tokenID, eLogType.Message);
                    myMethodInfo = tokenedMethodInfo;
                }
                else
                {
                    this.m_logger.DebugPrint("myMethodName " + myFullMethodName + " has a tokenID but does not exist!??", eLogType.Error);
                }
                
            }
            else
            {
                this.m_logger.DebugPrint("MissingTokenID: myMethodName " + myFullMethodName + ", consider adding a tokenID!", eLogType.Warning);
            }

            if (myMethodInfo == null)
            {
                m_logger.DebugPrint("FAILED: Patching " + methodNameLog + " Could not get my method [" + myFullMethodName + "]", eLogType.Error);
                return;
            }


            if (myMethodInfo.GetCustomAttribute<MethodDecoderTokenAttribute>() == null)
            {
                m_logger.DebugPrint("MethodDecoderTokenAttribute missing: [" + myFullMethodName + "]", eLogType.Warning);

            }

            HarmonyMethod harmonyInfo = new HarmonyMethod(myMethodInfo);
            harmonyInfo.methodType = methodType;
            if (patchType == HarmonyPatchType.Postfix)
            {

                try
                {
                    m_harmonyInstance.Patch(methodInfo, null, harmonyInfo);
                    m_logger.DebugPrint("SUCCESS : PostFix patching method -> " + myFullMethodName, eLogType.Debug);
                }
                catch
                {
                    m_logger.DebugPrint("FAILED : PostFix patching method -> " + myFullMethodName, eLogType.Error);
                }

            }
            else if  (patchType == HarmonyPatchType.Prefix)
            {
                try
                {
                    m_harmonyInstance.Patch(methodInfo, harmonyInfo, null);
                    m_logger.DebugPrint("SUCCESS : Prefix patching method -> " + myFullMethodName, eLogType.Debug);
                }
                catch
                {
                    m_logger.DebugPrint("FAILED : Prefix patching method -> " + myFullMethodName, eLogType.Error);
                }
            }

        }

        private Harmony m_harmonyInstance;


        private DebugLoggerObject m_logger;
    }

    

}
