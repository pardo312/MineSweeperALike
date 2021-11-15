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
        [SerializeField] private RectTransform m_boardBackground;

        [SerializeField, Range(1, 40)] private int m_numberOfBombs = 10;
        [SerializeField, Range(0, 2)] private float m_sizeOfSquare = 1;
        public override void Init()
        {
            CreateBoard(new SquareTilesBoardPayload() { _squareSize = m_sizeOfSquare });
        }

        public override void CreateBoard(object _payload, Action<int, int> _createdTile = null, Action<object> callback = null)
        {
            //----Create Board----
            int numberOfRows = 0;
            int numberOfColumns = 0;
            base.CreateBoard(_payload, (row, column) =>
            {
                numberOfRows = row;
                numberOfColumns = column;
            });
            numberOfRows += 1;
            numberOfColumns += 1;

            //----Set Background----
            RectTransform parentRectTransform = m_tileParent.GetComponent<RectTransform>();
            m_boardBackground.anchoredPosition = parentRectTransform.rect.center;
            float widthBackground = numberOfColumns * m_sizeOfSquare * 100;
            float heightBackground = numberOfRows * m_sizeOfSquare * 100;

            float offsetWidth = 0;
            float offsetHeight = 0;

            if (widthBackground > parentRectTransform.rect.width)
            {
                offsetHeight = widthBackground - parentRectTransform.rect.width;
                widthBackground = parentRectTransform.rect.width;
            }
            if (heightBackground > parentRectTransform.rect.height)
            {
                offsetWidth = heightBackground - parentRectTransform.rect.height;
                heightBackground = parentRectTransform.rect.height;
            }
            m_boardBackground.sizeDelta = new Vector2(widthBackground - offsetWidth, heightBackground - offsetHeight);

            //-----Create Mines----
            for (int i = 0; i < m_numberOfBombs; i++)
            {
                int randomRow = UnityEngine.Random.Range(0, numberOfRows);
                int randomColumn = UnityEngine.Random.Range(0, numberOfColumns);

                m_board[randomRow, randomColumn].m_isMine = true;
            }
        }
    }
}
