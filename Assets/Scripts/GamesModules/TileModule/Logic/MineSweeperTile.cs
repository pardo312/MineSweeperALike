using JiufenGames.MineSweeperAlike.Gameplay.Model;
using JiufenGames.TetrisAlike.Logic;
using JiufenModules.InterfaceReferenceValidator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class MineSweeperTile : TileBase, IPointerClickHandler
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
        public int m_numberOfMinesAround = 0;
        #endregion Class Fields
        #region Actions
        public event Action OnClearTileSweep;
        public event Action<bool> OnFlaggedTile;
        public event Action<bool> OnDeFlagMine;
        #endregion Actions
        #endregion ----Fields----

        #region ----Methods----
        public override void Awake()
        {
            foreach (ITileState tileState in m_tileStates)
            {
                m_tileStateDictionary.Add(tileState.m_stateName, tileState);
            }
            GetDefaultTileData();
            ChangeTileData(new MineDataPayload() { StateToChange = "NormalTileState" });
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                m_currentState.Sweep();
            else if (eventData.button == PointerEventData.InputButton.Right)
                m_currentState.Flag();
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

                if (!String.IsNullOrEmpty(mineDataPayload.StateToChange))
                {
                    m_currentState = m_tileStateDictionary[mineDataPayload.StateToChange];
                    m_currentState.InitState(this);
                    m_tileImage.sprite = m_currentState.m_stateSprite;
                }

                if (mineDataPayload.Sweeping)
                {
                    if (!m_isMine)
                    {
                        if (m_numberOfMinesAround != 0)
                        {
                            m_numberOfMinesTextField.text = m_numberOfMinesAround.ToString();
                        }
                        else
                        {
                            OnClearTileSweep?.Invoke();
                        }
                    }
                }
                else if (mineDataPayload.FlaggingTile)
                {
                    OnFlaggedTile?.Invoke(m_isMine);
                }
                else if (mineDataPayload.DeFlagMine)
                {
                    OnDeFlagMine?.Invoke(m_isMine);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public override object GetDefaultTileData()
        {
            return null;
        }
        public void EndGame()
        {
            Debug.Log("YOU LOSE");
            Debug.Break();
        }
        #endregion ----Methods----
    }
}
