using JiufenGames.Board.Logic;
using JiufenGames.MineSweeperAlike.Gameplay.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.Board.Logic
{
    public class BoardController : BoardControllerSquaredTilesBase<MineSweeperTile>
    {
        [SerializeField] private RectTransform m_boardBackground;

        public int m_numberOfTiles = 0;
        [Range(1, 40)] public int m_numberOfBombs = 10;
        [HideInInspector] public List<MineSweeperTile> minesPositions = new List<MineSweeperTile>();

        [SerializeField, Range(0, 2)] private float m_sizeOfSquare = 1;

        public event Action a_OnNormalTileSweep;
        public event Action<int, int> a_OnClearTileSweep;

        public event Action a_PressedInputFlag;
        public event Action a_PressedInputSweep;

        public event Action<bool, int, int> a_OnFlag;
        public event Action<bool, int, int> a_OnDeFlag;

        public event Action a_OnExplodeMine;

        public event Action<string, MineSweeperTile, Action<Sprite>> a_OnChangeState;
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
                m_board[row, column].transform.localScale = Vector2.one * .1f;
                LeanTween.scale(m_board[row, column].gameObject, Vector2.one * 1f, .5f).setEase(LeanTweenType.easeInBack);
                numberOfRows = row;
                numberOfColumns = column;

                m_board[row, column].a_OnNormalTileSweep -= () => a_OnNormalTileSweep?.Invoke();
                m_board[row, column].a_OnNormalTileSweep += () => a_OnNormalTileSweep?.Invoke();

                m_board[row, column].a_OnClearTileSweep -= () => a_OnClearTileSweep?.Invoke(row, column);
                m_board[row, column].a_OnClearTileSweep += () => a_OnClearTileSweep?.Invoke(row, column);

                m_board[row, column].a_OnFlaggedTile -= (isMine) => a_OnFlag?.Invoke(isMine, row, column);
                m_board[row, column].a_OnFlaggedTile += (isMine) => a_OnFlag?.Invoke(isMine, row, column);

                m_board[row, column].a_OnDeFlagMine -= (isMine) => a_OnDeFlag?.Invoke(isMine, row, column);
                m_board[row, column].a_OnDeFlagMine += (isMine) => a_OnDeFlag?.Invoke(isMine, row, column);

                m_board[row, column].a_OnExplodeMine -= () => a_OnExplodeMine?.Invoke();
                m_board[row, column].a_OnExplodeMine += () => a_OnExplodeMine?.Invoke();

            });
            m_numberOfTiles = ((numberOfRows + 1) * (numberOfColumns + 1)) - 1;
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

                MineSweeperTile tile = m_board[randomRow, randomColumn];
                tile.m_isMine = true;
                minesPositions.Add(tile);
            }
            SetNumberBaseOnMinesAroundIt(numberOfRows, numberOfColumns);
        }

        private void SetNumberBaseOnMinesAroundIt(int rows, int columns)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int numberOfMines = 0;
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            if (k == 0 && l == 0)
                                continue;
                            CheckMineOnAdjacentTile(i + k, j + l, ref numberOfMines);
                        }
                    }
                    m_board[i, j].m_numberOfMinesAround = numberOfMines;
                }
            }
        }

        private void CheckMineOnAdjacentTile(int row, int column, ref int numberOfMines)
        {
            if (m_board.GetLength(0) <= row || row < 0)
                return;
            if (m_board.GetLength(1) <= column || column < 0)
                return;
            if (!m_board[row, column].m_isMine)
                return;
            numberOfMines++;
        }
    }
}
