using JiufenGames.Board.Logic;
using JiufenGames.MineSweeperAlike.Gameplay.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Board.Logic
{
    public class BoardController : BoardControllerSquaredTilesBase<MineSweeperTile>
    {
        [SerializeField, Range(0, 15)] private int m_rows = 0;
        [SerializeField, Range(0, 10)] private int m_columns = 0;
        [SerializeField, Range(1, 40)] private int m_numberOfBombs = 10;
        public override void Init()
        {
            //CreateBoard(m_rows, m_columns,0.1f);
            CreateBoard(new SquareTilesBoardPayload() { _squareSize = 1 });
        }

        public override void CreateBoard(object _payload, Action<int, int> _createdTile = null)
        {
            base.CreateBoard(_payload, _createdTile);
            //Create Mines
            for (int i = 0; i < m_numberOfBombs; i++)
            {
                int randomRow = UnityEngine.Random.Range(0, m_rows);
                int randomColumn = UnityEngine.Random.Range(0, m_columns);

                m_board[randomRow, randomColumn].m_isMine = true;
            }
        }
    }
}
