using JiufenGames.MineSweeperAlike.Board.Logic;
using JiufenGames.MineSweeperAlike.Gameplay.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class FlagsController : MonoBehaviour
    {
        [SerializeField] private FlagsView m_flagsView;
        [HideInInspector] public int m_numberOfFlagsLeft;
        private int m_flaggedMines = 0;

        public void Init(BoardController _boardController)
        {
            m_numberOfFlagsLeft = _boardController.m_numberOfBombs;
            m_flagsView.Init(m_numberOfFlagsLeft);
        }

        public void DeflagTile(bool _isMine, int _totalNumberOfMines)
        {
            if (_isMine)
                m_flaggedMines--;

            if (m_numberOfFlagsLeft < _totalNumberOfMines)
                m_numberOfFlagsLeft++;

            m_flagsView.UpdateFlagsCounter(m_numberOfFlagsLeft);
        }

        /// <summary>
        /// Flag tile 
        /// </summary>
        /// <returns>Is end of the game</returns>
        public bool FlagTile(bool _isMine, int _totalNumberOfMines)
        {
            if (_isMine)
                m_flaggedMines++;

            if (m_numberOfFlagsLeft > 0)
                m_numberOfFlagsLeft--;

            m_flagsView.UpdateFlagsCounter(m_numberOfFlagsLeft);

            if (m_flaggedMines == _totalNumberOfMines && m_numberOfFlagsLeft == 0)
                return true;

            return false;
        }
    }
}
