using JiufenGames.Board.Logic;
using JiufenGames.MineSweeperAlike.Gameplay.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Board.Logic
{
    public struct BoardData
    {
        public List<Vector2Int> minesPositions;
        public List<Vector2Int> sweepedTilePositions;
        public List<Vector2Int> flaggedPositions;
    }

    public class BoardController : BoardControllerFullContainerBase<MineSweeperTile>
    {
        #region ----Fields----
        [SerializeField] private RectTransform m_boardBackground;

        public int m_numberOfTiles = 0;
        [Range(1, 40)] public int m_numberOfBombs = 10;
        [HideInInspector] public List<MineSweeperTile> minesPositions = new List<MineSweeperTile>();

        public int m_numberOfRows = 1;
        public int m_numberOfColumns = 1;
        public event Action a_OnNormalTileSweep;
        public event Action<int, int> a_OnClearTileSweep;

        public event Action<bool, int, int> a_OnFlag;
        public event Action<bool, int, int> a_OnDeFlag;

        public event Action a_OnExplodeMine;
        public event Action a_OnBoardCreated;

        public event Action<string, MineSweeperTile, Action<Sprite>> a_OnChangeState;
        #endregion ----Fields----

        #region ----Methods----
        public override void Init(object data)
        {
            MinesweeperPayload boardPayload;
            if (data.GetType() != typeof(MinesweeperPayload))
            {
                Debug.LogError("Board init data isn't the correct type.");
                return;
            }

            boardPayload = (MinesweeperPayload)data;

            m_numberOfRows = boardPayload._rows;
            m_numberOfColumns = boardPayload._columns;
            m_numberOfBombs = boardPayload._mines;

            CreateBoard((BaseBoardPayload)boardPayload, callback: (data) => a_OnBoardCreated?.Invoke());
        }

        public override void CreateBoard(object _payload, Action<int, int> _createdTile = null, Action<object> callback = null)
        {
            //----Create Board----
            base.CreateBoard(_payload, OnTileCreated, (data) =>
            {
                m_numberOfTiles = (m_numberOfRows * m_numberOfColumns);
                OnBoardCreated(data);
                StartCoroutine(WaitForSeconds(.5f, () =>
                   {
                       callback?.Invoke(data);
                   }));
            });

        }
        public void OnTileCreated(int row, int column)
        {
            m_board[row, column].m_tileColumn = column;
            m_board[row, column].m_tileRow = row;
            m_board[row, column].transform.localScale = Vector2.one * .1f;
            LeanTween.scale(m_board[row, column].gameObject, Vector2.one * 1f, .5f).setEase(LeanTweenType.easeOutBack);

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

        }
        public void OnBoardCreated(object data)
        {
            BaseBoardDto boardDto;
            if (!MinesweeperHelpers.IsSameOrSubclass(typeof(BaseBoardDto), data.GetType()))
                return;
            boardDto = (BaseBoardDto)data;

            //----Set Background----
            RectTransform parentRectTransform = m_tileParent.GetComponent<RectTransform>();
            m_boardBackground.anchoredPosition = parentRectTransform.rect.center;

            float widthBackground = (m_numberOfColumns * boardDto._sizeOfTiles.x);
            float heightBackground = (m_numberOfRows * boardDto._sizeOfTiles.y);

            m_boardBackground.sizeDelta = new Vector2(widthBackground, heightBackground);
            parentRectTransform.sizeDelta = new Vector2(widthBackground, heightBackground);

            List<Vector2> boardList = new List<Vector2>();
            for (int i = 0; i < m_board.GetLength(0); i++)
                for (int j = 0; j < m_board.GetLength(1); j++)
                    boardList.Add(new Vector2(i, j));

            int boardListSize = boardList.Count;
            //-----Create Mines----
            for (int i = 0; i < m_numberOfBombs; i++)
            {
                int randomRowColumnIndex = UnityEngine.Random.Range(0, boardListSize);
                Vector2 randomRowColumn = boardList[randomRowColumnIndex];

                int randomRow = (int)randomRowColumn.x;
                int randomColumn = (int)randomRowColumn.y;

                MineSweeperTile tile = m_board[randomRow, randomColumn];
                tile.m_isMine = true;
                minesPositions.Add(tile);

                boardList.RemoveAt(randomRowColumnIndex);
                boardListSize--;
            }
            SetNumberBaseOnMinesAroundIt(m_numberOfRows, m_numberOfColumns);
        }

        IEnumerator WaitForSeconds(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
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
        #endregion ----Methods----
    }
}
