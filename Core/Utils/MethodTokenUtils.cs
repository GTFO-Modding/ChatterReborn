using ChatterReborn.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ChatterReborn.Utils
{
    internal static class MethodTokenUtils
    {


        private static Random m_rnd = new Random(1342134234);

        private static List<uint> m_codes = new List<uint>();


        private static void GenerateRandomTokenIDs()
        {
            for (uint i = 100000; i < 9000000; i++)
            {
                m_codes.Add(i);
            }
        }


        private static uint GetRandomTokenID()
        {
            uint randomTokenID = m_codes[m_rnd.Next(0, m_codes.Count - 1)];
            m_codes.Remove(randomTokenID);  
            return randomTokenID;
        }

        internal static void GenerateMethodTokenIDsFile()
        {
            GenerateRandomTokenIDs();

            foreach (var type in AssemblyUtils.CurrentAssemblies)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

                if (methods != null)
                {
                    foreach (var methodInfo in methods)
                    {
                        var methodToken = methodInfo.GetCustomAttribute<MethodDecoderTokenAttribute>();
                        if (methodToken != null)
                        {
                            AddTokenIDToMethodName(GetRandomTokenID(), methodInfo);
                        }
                    }                    
                }                
            }
        }

        internal static bool TryToGetMethodInfoByTokenID(uint tokenID, out MethodInfo methodInfo)
        {
            return s_methodInfoBytokenID.TryGetValue(tokenID, out methodInfo);
        }

        private static void AddTokenIDToMethodName(uint tokenID, MethodInfo methodInfo)
        {
            if (s_methodInfoBytokenID.ContainsKey(tokenID))
            {
                return;
            }

            ChatterDebug.LogMessage("MethodTokenUtils, Now adding a tokenID to method -> " + methodInfo.Name + " with the id -> " + tokenID);

            s_methodInfoBytokenID.Add(tokenID, methodInfo);
            s_tokenIDByMethodInfo.Add(methodInfo, tokenID); 
        }


        internal static void GenerateMethodTokenIDs()
        {
            ChatterDebug.LogMessage("MethodTokenUtils, now generating tokenIDs...");
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("namespace ChatterRebornTokens {");
            builder.AppendLine("\tpublic static class Tokens{");
            foreach (var pair in s_methodInfoBytokenID)
            {
                builder.AppendLine("\t\tpublic const uint " + pair.Value.Name + " = " + pair.Key + ";");
            }
            builder.AppendLine("\t}");
            builder.AppendLine("}");


            string directory = Path.Combine(BepInEx.Paths.GameRootPath, "ChatterRebornMethodTokens");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(Path.Combine(directory, "ChatterRebornTokens.cs"), builder.ToString());
        }

        internal static void AddMissingPatchName(string myMethodName)
        {
            if (!s_missingPatches.Contains(myMethodName))
            {
                s_missingPatches.Add(myMethodName);
            }
        }

        internal static Dictionary<uint, MethodInfo> s_methodInfoBytokenID = new Dictionary<uint, MethodInfo>();
        internal static Dictionary<MethodInfo, uint> s_tokenIDByMethodInfo = new Dictionary<MethodInfo, uint>();


        internal static List<string> s_missingPatches = new List<string>();
    }
}
