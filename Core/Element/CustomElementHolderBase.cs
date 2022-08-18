using ChatterReborn.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace ChatterReborn.Element
{
    public class CustomElementHolderBase<T> : IElementHolder where T : CustomElementBase
    {

        private void VerifyEnvolope()
        {
            uint highestID = 0;

            foreach (var element in Envelope.Elements)
            {
                highestID = Math.Max(element.ID, highestID);
            }

            Envelope.LastElementID = highestID;
        }

        private void CheckElementID(T element, bool force = false)
        {
            if (force || element.ID <= 0)
            {
                VerifyEnvolope();
                element.ID = ++Envelope.LastElementID;
                m_dirtyFile = true;
            }
        }

        private void EnsureUniqueElementName(T element, List<string> existingNames)
        {
            uint uniqueID = 1;
            string originalName = element.name;
            string newName = originalName;
            bool first = true;
            do
            {
                if (!first)
                {
                    newName = originalName + "_" + uniqueID;
                    uniqueID++;
                }
                first = false;
            } while (existingNames.Contains(newName));
            element.name = newName;
        }

        private void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private string GetFileContents()
        {
            if (File.Exists(elementsPath))
            {
                return File.ReadAllText(elementsPath);
            }
            return null;
        }



        private void LoadElements()
        {
            elementsDirectory = Path.Combine(BepInEx.Paths.ConfigPath, "ChatterElements");
            ChatterDebug.LogMessage("Directory : " + elementsDirectory);
            EnsureDirectoryExists(elementsDirectory);
            elementsPath = Path.Combine(elementsDirectory, typeof(T).Name + ".json");

            string fileContents = GetFileContents();
            /*if (!string.IsNullOrEmpty(fileContents))
            {
                var serializer = new MTFO.Utilities.JsonSerializer();
                Envelope = serializer.Deserialize<ElementEnvolope<T>>(fileContents);
                VerifyEnvolope();
                m_elementByID = new Dictionary<uint, T>();
                m_elementByName = new Dictionary<string, T>();
                foreach (var element in Envelope.Elements)
                {
                    if (m_elementByID.ContainsKey(element.ID))
                    {
                        CheckElementID(element);
                    }
                    if (m_elementByName.ContainsKey(element.name))
                    {
                        EnsureNonDuplicateName(element);
                    }
                    m_elementByID.Add(element.ID, element);
                    m_elementByName.Add(element.name, element);
                }

                if (m_dirtyFile)
                {
                    m_dirtyFile = false;
                    SaveToDisk();
                }

                ChatterDebug.LogMessage("LoadElements, elements found! " + typeof(T).ToString());
                return;
            }*/

            ChatterDebug.LogWarning("LoadElements, no elements for Type " + typeof(T).ToString() + " found! starting out fresh!");
            m_elementByID = new Dictionary<uint, T>();
            m_elementByName = new Dictionary<string, T>();
            Envelope = new ElementEnvolope<T>();
            Envelope.Elements = new List<T>();
        }

        public void Setup()
        {
            Current = this;
            this.m_logger = new DebugLoggerObject(typeof(T).Name + " Holder");
            LoadElements();
        }

        private DebugLoggerObject m_logger;

        private Dictionary<uint, T> m_elementByID;

        private Dictionary<string, T> m_elementByName;

        private ElementEnvolope<T> Envelope { get; set; }
        public static CustomElementHolderBase<T> Current { get; set; }

        public static T AddNewElement()
        {
            T t = Activator.CreateInstance<T>();

            t.name = "New_Element";
            AddElement(t);

            return t;
        }

        public static T[] GetAllElements()
        {
            List<T> elements = new List<T>();
            foreach (var element in Current.Envelope.Elements)
            {
                if (element.enabled)
                {
                    elements.Add(element);
                }                
            }
            return elements.ToArray();
        }
        public static void SaveToDisk()
        {
            /*
            string contents = JsonSerializer.Serialize(Current.Envelope);
            File.WriteAllText(Current.elementsPath, contents);
            Current.m_logger.DebugPrint("Saving To Disk : " + typeof(T).ToString());
            */
        }

        public static uint AddElement(T element, int addIndex = -1)
        {
            Current.CheckElementID(element, false);
            EnsureNonDuplicateName(element);
            if (addIndex >= 0)
            {
                Current.Envelope.Elements.Insert(addIndex + 1, element);
            }
            else
            {
                Current.Envelope.Elements.Add(element);
            }
            Current.m_elementByName.Add(element.name, element);
            Current.m_elementByID.Add(element.ID, element);

            return element.ID;
        }

        public static void EnsureNonDuplicateName(T element)
        {
            Current.EnsureUniqueElementName(element, new List<string>(Current.m_elementByName.Keys));
        }

        public static void RenameElement(T element, string newName)
        {
            Current.m_elementByName.Remove(element.name);
            element.name = newName;
            EnsureNonDuplicateName(element);
            if (Current.m_elementByName.ContainsKey(element.name))
            {
                ChatterDebug.LogError("Renaming a block has failed!!");
                return;
            }
            Current.m_elementByName.Add(element.name, element);
        }
        public static T GetElement(uint id)
        {
            if (id == 0)
            {
                return null;
            }

            if (Current.m_elementByID.TryGetValue(id, out T value))
            {
                return value;
            }
            return null;
        }
        
        

        public static T GetElement(string name)
        {
            if (Current.m_elementByName.TryGetValue(name, out T value))
            {
                return value;
            }
            return null;
        }

        public static bool HasElement(uint id)
        {
            if (id == 0)
            {
                return false;
            }
            return Current.m_elementByID.TryGetValue(id, out T value) && value.enabled;
        }
        public static bool HasElement(string name)
        {
            return Current.m_elementByName.TryGetValue(name, out T value) && value.enabled;
        }

        public void PostSetup()
        {
            foreach (var element in Envelope.Elements)
            {
                element.OnPostSetup();
            }
        }

        private string elementsDirectory;

        private string elementsPath;
        private bool m_dirtyFile;
    }
}
