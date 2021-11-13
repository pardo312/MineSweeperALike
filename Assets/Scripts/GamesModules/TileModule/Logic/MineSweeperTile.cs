using JiufenGames.TetrisAlike.Logic;
using JiufenModules.ScoreModule.Example;
using System;
using System.Collections.Generic;
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
        public List<ITileState> m_tileStates => InterfaceUnityRefereceValidator.ValidateIfUnityObjectArrayIsOfType<ITileState>(m_tileStatesField);
        public Dictionary<string, ITileState> m_tileStateDictionary = new Dictionary<string, ITileState>();
        public ITileState m_currentState;
        public bool m_isMine = false;
        #endregion Class Fields
        #endregion ----Fields----

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
