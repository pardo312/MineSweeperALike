using JiufenGames.MineSweeperAlike.Board.Logic;
using System;
using System.Collections;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class SweepController : MonoBehaviour
    {
        #region ----Fields----
        private BoardController m_boardController;
        private  FlagsController m_flagsController;

        [SerializeField, Range(.01f, .1f)] private float m_sweepClearTileAnimTime = .05f;
        public int m_notClearedTiles = 0;

        public Action<bool> a_endGame;
        public Action<string, MineSweeperTile> a_executeInput;
        #endregion ----Fields----

        #region ----Method----
        public void Init(BoardController _boardController, FlagsController _flagsController, int _initNotClearedTiles)
        {
            m_boardController = _boardController;
            m_flagsController = _flagsController;
            m_notClearedTiles = _initNotClearedTiles;
        }

        public void SweepClearTile(int _row, int _column)
        {
            ReduceNotSweepedTiles();
            StartCoroutine(SweepAllClearTilesAround(_row, _column));
        }

        public void ReduceNotSweepedTiles()
        {
            m_notClearedTiles--;
            //All tiles swept, no matter the flags
            if (m_notClearedTiles == 0)
            {
                m_boardController.minesPositions.ForEach((mine) =>
                {
                    if (mine.m_currentState.m_stateName.CompareTo(TileStatesConstants.FLAGGED_TILE_STATE) != 0)
                        a_executeInput("Flag", mine);
                });
                a_endGame(true);
            }
        }

        private IEnumerator SweepAllClearTilesAround(int _row, int _column)
        {
            for (int k = -1; k <= 1; k++)
            {
                for (int l = -1; l <= 1; l++)
                {
                    //if tile = tileChecking(originaltile)
                    if (k == 0 && l == 0)
                        continue;
                    //if tile outsideBounds
                    if (!CheckIfInsideBoundsOfBoard(_row + k, _column + l))
                        continue;
                    MineSweeperTile currentTile = m_boardController.m_board[_row + k, _column + l];
                    //if tile hasMine
                    if (currentTile.m_isMine)
                        continue;
                    //if tile isn't in normalState
                    if (currentTile.m_currentState.m_stateName.CompareTo(TileStatesConstants.NORMAL_TILE_STATE) != 0)
                        continue;
                    if (currentTile.m_isSwiping)
                        continue;

                    currentTile.m_isSwiping = true;

                    //Sweep tile and reduce the not sweeped tiles
                    yield return new WaitForSeconds(m_sweepClearTileAnimTime);
                    currentTile.ExecuteCurrentStateAction("Sweep", m_flagsController.m_numberOfFlagsLeft > 0);

                    currentTile.m_isSwiping = false;
                }
            }
            yield return null;
        }

        private bool CheckIfInsideBoundsOfBoard(int row, int column)
        {
            if (m_boardController.m_board.GetLength(0) <= row || row < 0)
                return false;
            if (m_boardController.m_board.GetLength(1) <= column || column < 0)
                return false;

            return true;
        }
        #endregion ----Method----
    }
}
