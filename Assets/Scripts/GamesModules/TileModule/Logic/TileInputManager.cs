using JiufenGames.MineSweeperAlike.Gameplay.Model;
using JiufenGames.TetrisAlike.Logic;
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

    public class TileInputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Fields
        private bool m_isPressing = false;
        private float m_timePressing = 0f;

        private MineSweeperTile m_mineSweeperTile;

        private const float c_timesNeededForFlag = 3f;

        public Action a_PressedInputFlag;
        public Action a_PressedInputSweep;
        #endregion Fields

        #region Methods
        public void Start()
        {
            m_mineSweeperTile = GetComponent<MineSweeperTile>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                m_isPressing = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && m_isPressing)
            {
                m_isPressing = false;
                m_timePressing = 0;
                InputManager.m_Instance.SweepTile(m_mineSweeperTile);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                InputManager.m_Instance.FlagTile(m_mineSweeperTile);
            }
        }

        public void Update()
        {
            if (m_isPressing)
            {
                m_timePressing += Time.deltaTime * 10;
                if (m_timePressing > c_timesNeededForFlag)
                {
                    m_timePressing = 0;
                    m_isPressing = false;
                    InputManager.m_Instance.FlagTile(m_mineSweeperTile);
                }
            }
        }
        #endregion Methods
    }
}
