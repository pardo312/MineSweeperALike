using System;
using UnityEngine;

namespace JiufenGames.Board.Logic
{
    public abstract class BoardControllerFullContainerBase<T> : BoardControllerBase<T>
    {
        public override void CreateBoard(object payload, Action<int, int> _createdTile = null, Action<object> _endCreationCallback = null)
        {
            //Init Payload
            BaseBoardPayload boardPayload;
            if (!BoardHelpers.IsSameOrSubclass(typeof(BaseBoardPayload), payload.GetType()))
            {
                Debug.LogError("Board init data isn't the correct type.");
                return;
            }
            boardPayload = (BaseBoardPayload)payload;

            //Set variables
            RectTransform parentRectTransform = m_tileParent.GetComponent<RectTransform>();

            //Get parent sizeDelta
            float parentWidth = parentRectTransform.rect.width;
            float parentHeight = parentRectTransform.rect.height;

            //Set sizes for childs
            Vector2 sizeOfTile;
            if (boardPayload._squaredTiles)
            {
                if (boardPayload._rows > boardPayload._columns)
                    sizeOfTile = new Vector2((parentHeight / boardPayload._rows), (parentHeight / boardPayload._rows));
                else
                    sizeOfTile = new Vector2((parentWidth / boardPayload._columns), (parentWidth / boardPayload._columns));
            }
            else
                sizeOfTile = new Vector2((parentWidth / boardPayload._columns), (parentHeight / boardPayload._rows));

            //Init Board
            bool isEvenRow = boardPayload._rows % 2 == 0;

            int endRow, initRow;
            if (isEvenRow)
            {
                endRow = boardPayload._rows / 2;
                initRow = -endRow;
            }
            else
            {
                endRow = (boardPayload._rows + 1) / 2;
                initRow = (-endRow) + 1;
            }

            bool isEvenCol = boardPayload._columns % 2 == 0;
            int endColumn, initColumn;
            if (isEvenCol)
            {
                endColumn = boardPayload._columns / 2;
                initColumn = -endColumn;
            }
            else
            {

                endColumn = (boardPayload._columns + 1) / 2;
                initColumn = (-endColumn) + 1;
            }

            //CreateBoard
            m_board = new T[boardPayload._rows, boardPayload._columns];
            for (int i = initRow; i < endRow; i++)
                for (int j = initColumn; j < endColumn; j++)
                {
                    //Instantiate
                    GameObject instancedGO = Instantiate(m_tilePrefab, Vector3.zero, Quaternion.identity, m_tileParent);
                    RectTransform instanceRectTransform = instancedGO.GetComponent<RectTransform>();
                    float offsetPosX = (j * sizeOfTile.x) + (isEvenCol ? sizeOfTile.x / 2 : 0);
                    float offsetPosY = (i * sizeOfTile.y) + (isEvenRow ? sizeOfTile.y / 2 : 0);

                    instanceRectTransform.anchoredPosition = parentRectTransform.rect.center + new Vector2(offsetPosX, offsetPosY);
                    instancedGO.GetComponent<RectTransform>().sizeDelta = sizeOfTile;

                    //Set Board and tile
                    m_board[i - initRow, j - initColumn] = instancedGO.GetComponent<T>();

                    //Callback
                    _createdTile?.Invoke(i - initRow, j - initColumn);
                }

            _endCreationCallback(new BaseBoardDto { _sizeOfTiles = sizeOfTile });
        }
    }
}