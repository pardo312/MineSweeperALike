﻿using JiufenGames.MineSweeperAlike.Gameplay.Model;
using JiufenGames.TetrisAlike.Logic;
using JiufenModules.InterfaceReferenceValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class MineSweeperTile : TileBase
    {
        #region ----Fields----
        #region References
        [SerializeField] private Image m_tileImage;
        [SerializeField] private TMP_Text m_numberOfMinesTextField;
        [SerializeField] private UnityEngine.Object[] m_tileStatesField;
        #endregion References

        #region Properties
        public List<ITileState> m_tileStates => InterfaceUnityRefereceValidator.ValidateIfUnityObjectArrayIsOfType<ITileState>(m_tileStatesField, "JiufenGames.MineSweeperAlike.Gameplay.Logic.", Assembly.GetExecutingAssembly().FullName);
        #endregion Properties

        #region Class Fields
        public Dictionary<string, ITileState> m_tileStateDictionary = new Dictionary<string, ITileState>();
        public ITileState m_currentState;
        public bool m_isMine = false;
        public bool m_isSwiping = false;
        public int m_numberOfMinesAround = 0;

        #endregion Class Fields

        #region Actions
        public event Action a_OnNormalTileSweep;
        public event Action a_OnClearTileSweep;
        public event Action<bool> a_OnFlaggedTile;
        public event Action<bool> a_OnDeFlagMine;
        public event Action a_OnExplodeMine;
        #endregion Actions
        #endregion ----Fields----

        #region ----Methods----
        public override void Awake()
        {
            foreach (ITileState tileState in m_tileStates)
                m_tileStateDictionary.Add(tileState.m_stateName, tileState);

            GetDefaultTileData();
            ChangeTileData(new MineDataPayload() { StateToChange = TileStatesConstants.NORMAL_TILE_STATE });
        }

        public void ExecuteCurrentStateAction(string state, bool canFlag)
        {
            if (state.CompareTo(TileStatesConstants.SWEEP_ACTION) == 0)
                m_currentState.Sweep(this);
            else if (state.CompareTo(TileStatesConstants.FLAG_ACTION) == 0)
                if (canFlag || (!canFlag && m_currentState.m_stateName.CompareTo(TileStatesConstants.FLAGGED_TILE_STATE) == 0))
                    m_currentState.Flag(this);
        }

        /// <summary>
        /// Change Tile Data  
        /// </summary>
        /// <param name="_methodParams[0](string)">State to change</param>
        /// <param name="_methodParams[1](bool)">Sweeping: is sweeping</param>
        /// <param name="_methodParams[2](bool)">Flagged: is bomb or not</param>
        /// <returns></returns>
        public override object ChangeTileData(object _payload = null)
        {
            base.ChangeTileData(_payload);
            if (_payload != null && _payload.GetType() == typeof(MineDataPayload))
            {
                MineDataPayload mineDataPayload = (MineDataPayload)_payload;

                if (!string.IsNullOrEmpty(mineDataPayload.StateToChange))
                {
                    m_currentState = m_tileStateDictionary[mineDataPayload.StateToChange];
                    m_currentState.InitState(this);
                    m_tileImage.sprite = m_currentState.m_stateSprite;
                }

                if (mineDataPayload.Sweeping)
                {
                    if (m_isMine)
                        return true;

                    if (m_numberOfMinesAround != 0)
                    {
                        m_numberOfMinesTextField.text = m_numberOfMinesAround.ToString();
                        // Reduce the number of tiles not sweeped
                        a_OnNormalTileSweep?.Invoke();
                    }
                    else
                    {
                        //Clear all adayacent tiles
                        a_OnClearTileSweep?.Invoke();
                    }
                }
                else if (mineDataPayload.FlaggingTile)
                    a_OnFlaggedTile?.Invoke(m_isMine);
                else if (mineDataPayload.DeFlagMine)
                    a_OnDeFlagMine?.Invoke(m_isMine);

                return true;
            }
            else
                return false;
        }

        public override object GetDefaultTileData()
        {
            return null;
        }

        public void ExplodeMine()
        {
            a_OnExplodeMine?.Invoke();
        }

        #endregion ----Methods----
    }
}
