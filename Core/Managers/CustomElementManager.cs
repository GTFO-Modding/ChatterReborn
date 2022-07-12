using ChatterReborn.Attributes;
using ChatterReborn.Data;
using ChatterReborn.Element;
using ChatterReborn.Utils;
using ChatterRebornSettings;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ChatterReborn.Managers
{
    public class CustomElementManager : ChatterManager<CustomElementManager>
    {

        public bool Loaded { get; set; }
        protected override void Setup()
        {
            //PrepareCustomElements();
        }

        private void PrepareCustomElements()
        {
            if (MiscSettings.ElementsEnabled)
            {
                if (AssemblyUtils.DoesAssemblyExist("MTFO"))
                {
                    JsonUtils.JsonExist = true;
                    SetupElementHolders();
                    Loaded = true;
                }
                else
                {
                    this.DebugPrint("Elements are enabled but MTFO does not exist!!!", eLogType.Error);
                }
            }
        }

        private void SetupElementHolders()
        {
            List<IElementHolder> elementHolders = new List<IElementHolder>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var attribute = type.GetCustomAttribute<CustomElementTypeAttribute>();
                if (attribute != null)
                {
                    var holder = typeof(CustomElementHolderBase<>);
                    var genericHolder = holder.MakeGenericType(type);
                    if (genericHolder != null)
                    {
                        IElementHolder elementHolder = Activator.CreateInstance(genericHolder) as IElementHolder;
                        if (elementHolder != null)
                        {
                            elementHolder.Setup();
                            elementHolders.Add(elementHolder);
                            this.DebugPrint("Setting up Elements : " + attribute.ElementType, eLogType.Message);
                        }
                        else
                        {
                            this.DebugPrint("ERROR : element type : " + attribute.ElementType + " could not be set up!", eLogType.Error);
                        }
                    }
                    else
                    {
                        this.DebugPrint("Could not make a generic method for element type " + attribute.ElementType, eLogType.Error);
                    }
                }
            }

            foreach (var elementHolder in elementHolders)
            {
                elementHolder.PostSetup();
            }
        }
          




    }
}
