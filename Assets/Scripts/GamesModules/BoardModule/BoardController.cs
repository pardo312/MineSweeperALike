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
            CreateBoard2(new SquareTilesBoardPayload() { _squareSize = m_sizeOfSquare });
        }

        public override void CreateBoard(object _payload, Action<int, int> _createdTile = null)
        {
            int numberOfRows = 0;
            int numberOfColumns = 0;
            base.CreateBoard(_payload, (row, column) =>
            {
                numberOfRows = row;
                numberOfColumns = column;
            });
            numberOfRows += 1;
            numberOfColumns += 1;

            m_boardBackground.anchoredPosition = m_tileParent.GetComponent<RectTransform>().rect.center;
            m_boardBackground.sizeDelta = new Vector2(numberOfColumns * m_sizeOfSquare * 100, numberOfRows * m_sizeOfSquare * 100);

            //Create Mines
            for (int i = 0; i < m_numberOfBombs; i++)
            {
                int randomRow = UnityEngine.Random.Range(0, numberOfRows);
                int randomColumn = UnityEngine.Random.Range(0, numberOfColumns);

                m_board[randomRow, randomColumn].m_isMine = true;
            }
        }

        public void CreateBoard2(object payload, Action<int, int> _createdTile = null)
        {
            //Init Payload
            SquareTilesBoardPayload boardPayload;
            if (payload.GetType() != typeof(SquareTilesBoardPayload))
                return;
            boardPayload = payload as SquareTilesBoardPayload;

            //Set variables
            RectTransform parentRectTransform = m_tileParent.GetComponent<RectTransform>();

            //Get parent sizeDelta
            float parentWidth = parentRectTransform.rect.width;
            float parentHeight = parentRectTransform.rect.height;

            //Get column and rows
            int rows = (int)(parentHeight / (boardPayload._squareSize * 100));
            int columns = (int)(parentWidth / (boardPayload._squareSize * 100));

            //Set sizes for childs
            float sizeForChilds = (boardPayload._squareSize * 100) - 10;

            //SetEnRow & endColumn if even or odd 
            int endRow = rows % 2 == 0 ? rows / 2 : (rows / 2) + 1;
            int endColumn = columns % 2 == 0 ? columns / 2 : (columns / 2) + 1;

            //CreateBoard
            m_board = new MineSweeperTile[endRow + (rows / 2), endColumn + (columns / 2)];
            for (int i = -endRow + 1; i < endRow; i++)
                for (int j = -endColumn + 1; j < endColumn; j++)
                {
                    //Instantiate
                    GameObject instancedGO = Instantiate(m_tilePrefab, Vector3.zero, Quaternion.identity, m_tileParent);

                    //Set Position and scale
                    RectTransform instanceRectTransform = instancedGO.GetComponent<RectTransform>();
                    instanceRectTransform.anchoredPosition = parentRectTransform.rect.center + new Vector2(j * boardPayload._squareSize * 100, i * boardPayload._squareSize * 100);
                    instanceRectTransform.sizeDelta = new Vector2(sizeForChilds, sizeForChilds);

                    //Set Board and tile
                    m_board[i + (endRow-1), j + (endColumn-1)] = instancedGO.GetComponent<MineSweeperTile>();

                    //Callback
                    _createdTile?.Invoke(i + endRow, j + endColumn);
                }

            m_boardBackground.anchoredPosition = parentRectTransform.rect.center;
            m_boardBackground.sizeDelta = new Vector2(columns * m_sizeOfSquare * 100, rows * m_sizeOfSquare * 100);
        }
    }
}
