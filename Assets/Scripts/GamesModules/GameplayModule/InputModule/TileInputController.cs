using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class TileInputController : MonoBehaviour
    {
        #region Singleton
        public static TileInputController m_Instance;
        public void Awake()
        {
            if(m_Instance == null)
            {
                m_Instance = this;
            }
            else
            {
                DestroyImmediate(this);
            }
        }
        #endregion Singleton

        public Action<MineSweeperTile> a_PressedInputFlag;
        public Action<MineSweeperTile> a_PressedInputSweep;

        public void FlagTile(MineSweeperTile _tile)
        {
            a_PressedInputFlag?.Invoke(_tile);
        }
        
        public void SweepTile(MineSweeperTile _tile)
        {
            a_PressedInputSweep?.Invoke(_tile);
        }
    }
}