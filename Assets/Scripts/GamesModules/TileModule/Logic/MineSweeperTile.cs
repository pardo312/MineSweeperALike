using JiufenGames.TetrisAlike.Logic;
using JiufenModules.ScoreModule.Example;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class MineSweeperTile : TileBase
    {
        #region ----Fields----
        #region References
        [SerializeField] private Image m_tileImage;
        [SerializeField] private UnityEngine.Object[] m_tileStatesField;
        #endregion References

        #region Class Fields
        public List<ITileState> m_tileStates => ValidateIfUnityObjectArrayIsOfType<ITileState>(m_tileStatesField, "JiufenGames.MineSweeperAlike.Gameplay.Logic.");
        public ITileState testState;
        public Dictionary<string, ITileState> m_tileStateDictionary = new Dictionary<string, ITileState>();
        public ITileState m_currentState;
        public bool m_isMine = false;
        #endregion Class Fields
        #endregion ----Fields----

        public List<T> ValidateIfUnityObjectArrayIsOfType<T>(UnityEngine.Object[] unityObjects, string classNamespace = null)
        {
            List<T> returnList = new List<T>();
            for (int i = 0; i < unityObjects.Length; i++)
            {
                UnityEngine.Object item = unityObjects[i];

                if (item == null)
                    continue;


                if ((object)item is T)
                {
                    returnList.Add((T)(object)item);
                }
                else if (item is GameObject && (item as GameObject).GetComponent<T>() != null)
                {
                    returnList.Add((item as GameObject).GetComponent<T>());
                }
                else
                {
                    Type classType = Type.GetType(classNamespace + item.name);
                    if (classType != null)
                    {
                        returnList.Add((T)Activator.CreateInstance(classType));
                        continue;
                    }
                    Debug.LogError($"<color=red>ValidateInterface:</color>  The item: [{item.name}] is not a {typeof(T).Name} subclass. Please check the reference");
                }
            }

            //Return List
            return returnList;
        }

        #region ----Methods----
        public override void Awake()
        {
            foreach (ITileState tileState in m_tileStates)
            {
                m_tileStateDictionary.Add(tileState.m_stateName, tileState);
            }
            GetDefaultTileData();
            m_currentState = m_tileStateDictionary["NormalTileState"];
            m_currentState.InitState();
        }

        public override object[] ChangeTileData(object[] _methodParams = null)
        {
            base.ChangeTileData(_methodParams);
            if (_methodParams != null && _methodParams.Length > 0 && _methodParams.GetType() == typeof(string))
            {
                m_currentState = m_tileStateDictionary[(string)_methodParams[0]];
                m_currentState.InitState();
                m_tileImage.sprite = m_currentState.m_stateSprite;
                return new object[1] { true };
            }
            else
            {
                return new object[1] { false };
            }
        }

        public override object[] GetDefaultTileData()
        {
            return null;
        }


        #endregion ----Methods----
    }

    public class StateFactory
    {

    }
}
