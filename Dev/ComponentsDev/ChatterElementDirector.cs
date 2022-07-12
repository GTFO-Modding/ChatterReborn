using ChatterReborn.Attributes;
using ChatterReborn.Data;
using ChatterReborn.Element;
using ChatterReborn.Utils;
using GameData;
using Player;
using System;
using System.Collections.Generic;
using UnhollowerBaseLib.Attributes;
using UnityEngine;

namespace ChatterReborn.ComponentsDev
{
    [IL2CPPType(AddComponentOnStart = false, DontDestroyOnLoad = false)]
    public class ChatterElementDirector : MonoBehaviour
    {
        public ChatterElementDirector()
        {
        }

        public ChatterElementDirector(IntPtr pointer) : base(pointer)
        {
        }

        void Awake()
        {
            Current = this;
            this.debugLoggerObject = new DebugLoggerObject("ChatterElementDirector");

        }


        private DebugLoggerObject debugLoggerObject;

        private Dictionary<KeyCode, ElementType> m_actionType = new Dictionary<KeyCode, ElementType>
        {
            {
                KeyCode.M,
                ElementType.TeammateComment
            },
            {
                KeyCode.N,
                ElementType.SecurityDoor
            },
            {
                KeyCode.J,
                ElementType.EnemyFilter
            }
        };

        void Update()
        {
            foreach (var actionType in m_actionType)
            {
                if (Input.GetKeyDown(actionType.Key))
                {
                    AddElementType(actionType);
                    return;
                }
            }
        }

        private void AddEnemyToEnemyFilters()
        {
            if (Globals.Global.RundownIdToLoad != 1)
            {
                this.debugLoggerObject.DebugPrint("This is not a custom rundown!!!", eLogType.Error);
                return;
            }

            string customRundownTitle = RundownDataBlock.GetBlock(1).StorytellingData.Title;
            ExtendedStringUtils.EliminateInvalidCharacters(ref customRundownTitle);
            string elementName = "FilterGroup_" + customRundownTitle;
            CustomRundownElement element = null;
            if (!CustomElementHolderBase<CustomRundownElement>.HasElement(elementName))
            {
                element = CustomElementHolderBase<CustomRundownElement>.AddNewElement();
                CustomElementHolderBase<CustomRundownElement>.RenameElement(element, elementName);
                element.enabled = true;
                element.CustomRundownKey = CustomRundown.Fatal_Experiment2;
            }

            if (element != null)
            {
                CustomElementHolderBase<CustomRundownElement>.SaveToDisk();
            }


            var filterElement = CustomElementHolderBase<EnemyFilterElement>.AddNewElement();
            filterElement.CustomRundownKey = CustomRundown.Fatal_Experiment2;
            filterElement.EnemyFilter = EnemyFilter.Big | EnemyFilter.Striker;

            if (filterElement.EnemyIDs == null)
            {
                filterElement.EnemyIDs = new List<uint>();
            }


            foreach (var block in EnemyDataBlock.GetAllBlocks())
            {
                bool isCharger = block.name.Contains("Charger");
                if (block.name.Contains("Striker") || isCharger)
                {
                    if (block.ModelDatas[0].SizeRange.x > 1.7f)
                    {
                        filterElement.EnemyIDs.Add(block.persistentID);
                    }
                }                
            }


            CustomElementHolderBase<EnemyFilterElement>.SaveToDisk();

            this.debugLoggerObject.DebugPrint("Created calloutTargets for the Fatal2!", eLogType.Message);
        }

        private void AddElementType(KeyValuePair<KeyCode, ElementType> actionType)
        {
            if (PlayerManager.TryGetLocalPlayerAgent(out PlayerAgent playerAgent))
            {
                if (JsonUtils.JsonExist)
                {
                    if (playerAgent.FPSCamera != null)
                    {
                        switch (actionType.Value)
                        {
                            case ElementType.None:
                                break;
                            case ElementType.TeammateComment:
                                AddDialogueTriggerPoint(playerAgent.FPSCamera.CameraRayPos);
                                break;
                            case ElementType.SecurityDoor:
                                AddSecurityDoorElementPoint(playerAgent.FPSCamera.CameraRayObject, (int)playerAgent.CourseNode.m_zone.LocalIndex);
                                break;
                            case ElementType.EnemyFilter:
                                PrepareEnemies();
                                break;
                            default:
                                break;
                        }
                    }
                }                
            }            
        }

        private void PrepareEnemies()
        {
            this.AddEnemyToEnemyFilters();
        }

        [HideFromIl2Cpp]
        private void AddDialogueTriggerPoint(FixedVector3 position)
        {
            var element = CustomElementHolderBase<TeammateCommentElement>.AddNewElement();
            element.Position = position;
            element.enabled = true;
            CustomElementHolderBase<TeammateCommentElement>.SaveToDisk();
            ChatterDebug.LogMessage("Adding a dialog trigger point at position : " + position.ToString());
        }

        [HideFromIl2Cpp]
        private void AddSecurityDoorElementPoint(GameObject go, int zoneIndex)
        {
            var element = CustomElementHolderBase<SecurityDoorElement>.AddNewElement();
            element.GOName = GOUtil.GetHiearchyName(go);
            CustomElementHolderBase<SecurityDoorElement>.RenameElement(element, go.name);
            element.ZoneIndex = zoneIndex;
            element.enabled = true;
            CustomElementHolderBase<SecurityDoorElement>.SaveToDisk();
        }

        public static ChatterElementDirector Current;
    }
}
