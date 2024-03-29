﻿using JiufenGames.MineSweeperAlike.Board.Logic;
using JiufenGames.MineSweeperAlike.HomeModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class BoardSaveDataController : MonoBehaviour
    {
        #region ----Fields-----
        private BoardController boardController;
        #endregion  ----Fields-----

        #region ----Methods-----
        public void Init(BoardController _boardController)
        {
            boardController = _boardController;
        }

        [ContextMenu("Do it!")]
        public BoardSaveData SaveMatch(BoardDifficultyEnum difficulty)
        {
            List<Vector2Int> minesPos = new List<Vector2Int>();
            boardController.minesPositions.ForEach((mine) => minesPos.Add(new Vector2Int(mine.m_tileRow, mine.m_tileColumn)));

            List<Vector2Int> sweepPos = new List<Vector2Int>();
            List<Vector2Int> flaggedPos = new List<Vector2Int>();

            for (int i = 0; i < boardController.m_board.GetLength(0); i++)
            {
                for (int j = 0; j < boardController.m_board.GetLength(1); j++)
                {
                    if (boardController.m_board[i, j].m_currentState.m_stateName.Equals(TileStatesConstants.FLAGGED_TILE_STATE))
                        flaggedPos.Add(new Vector2Int(i, j));
                    else if (boardController.m_board[i, j].m_currentState.m_stateName.Equals(TileStatesConstants.SWEPT_TILE_STATE))
                        sweepPos.Add(new Vector2Int(i, j));
                }
            }

            bool isCustom = boardController.m_numberOfColumns > 32 || boardController.m_numberOfRows > 32;
            BoardSaveData boardData = new BoardSaveData()
            {
                difficulty = difficulty,
                flaggedPositions = flaggedPos,
                minesPositions = minesPos,
                sweepedTilePositions = sweepPos
            };
            return boardData;
        }

        public void LoadMatch(BoardSaveData boardData)
        {
            boardData.minesPositions.ForEach((minePos) =>
            {
                boardController.m_board[minePos.x, minePos.y].m_isMine = true;
                boardController.minesPositions.Add(boardController.m_board[minePos.x, minePos.y]);
            });

            boardData.flaggedPositions.ForEach((flaggedPos) => boardController.m_board[flaggedPos.x, flaggedPos.y].ExecuteCurrentStateAction(TileStatesConstants.FLAG_ACTION, true));
            StartCoroutine(LoadSweeptTiles(boardData.sweepedTilePositions));
        }

        IEnumerator LoadSweeptTiles(List<Vector2Int> sweepedTilePositions)
        {
            //yield return new WaitForSeconds(.7f);
            yield return new WaitForEndOfFrame();
            sweepedTilePositions.ForEach((sweeptPos) => boardController.m_board[sweeptPos.x, sweeptPos.y].ExecuteCurrentStateAction(TileStatesConstants.SWEEP_ACTION, true));
        }
        #endregion ----Methods-----
    }
}
