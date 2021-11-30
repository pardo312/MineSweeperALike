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
            ChangeTileData(new object[1] { "NormalTileState" });
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                m_currentState.Sweep();
            else if (eventData.button == PointerEventData.InputButton.Right)
                m_currentState.Flag();
        }

        public override object[] ChangeTileData(object[] _methodParams = null)
        {
            base.ChangeTileData(_methodParams);
            if (_methodParams != null)
            {
                if (_methodParams.Length > 0 && _methodParams[0].GetType() == typeof(string))
                {
                    m_currentState = m_tileStateDictionary[(string)_methodParams[0]];
                    m_currentState.InitState(this);
                    m_tileImage.sprite = m_currentState.m_stateSprite;
                }
                if (_methodParams.Length > 1 && _methodParams[1].GetType() == typeof(bool) && (bool)_methodParams[1])
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
}
