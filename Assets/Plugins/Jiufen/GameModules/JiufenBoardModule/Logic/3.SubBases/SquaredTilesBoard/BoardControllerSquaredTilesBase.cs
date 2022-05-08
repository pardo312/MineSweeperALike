using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.Board.Logic
{
    public abstract class BoardControllerSquaredTilesBase<T> : BoardControllerBase<T>
    {
        public override void CreateBoard(object payload, Action<int, int> _createdTile = null, Action<object> _endCreationCallback = null)
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
            m_board = new T[((endRow * 2)) - 1, (endColumn * 2) - 1];
            for (int i = -endRow + 1; i < endRow; i++)
            {
                for (int j = -endColumn + 1; j < endColumn; j++)
                {
                    //Instantiate
                    GameObject instancedGO = Instantiate(m_tilePrefab, Vector3.zero, Quaternion.identity, m_tileParent);

                    //Set Position and scale
                    RectTransform instanceRectTransform = instancedGO.GetComponent<RectTransform>();
                    instanceRectTransform.anchoredPosition = parentRectTransform.rect.center + new Vector2(j * boardPayload._squareSize * 100, i * boardPayload._squareSize * 100);
                    instanceRectTransform.sizeDelta = new Vector2(sizeForChilds, sizeForChilds);

                    //Set Board and tile
                    m_board[(i + (endRow)) - 1, (j + (endColumn)) - 1] = instancedGO.GetComponent<T>();

                    //Callback
                    _createdTile?.Invoke((i + (endRow)) - 1, (j + (endColumn)) - 1);
                }
            }
        }
    }
}