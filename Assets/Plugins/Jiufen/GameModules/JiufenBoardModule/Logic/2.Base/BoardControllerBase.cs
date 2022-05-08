using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.Board.Logic
{
    public abstract class BoardControllerBase<T> : MonoBehaviour, IBoardController<T>
    {
        #region Fields
        #region Interface
        #region BackingFields
        [Header("Necessary References")]
        [SerializeField] private GameObject m_tilePrefabField;
        [SerializeField] private Transform m_tileParentField;
        [Header("Board")]
        private T[,] m_boardField;
        #endregion BackingFields

        #region Properties
        public GameObject m_tilePrefab { get => m_tilePrefabField; }
        public Transform m_tileParent { get => m_tileParentField; }
        public T[,] m_board { get => m_boardField; set => m_boardField = value; }
        #endregion Properties
        #endregion Interface
        #endregion Fields

        #region Methods
        public abstract void Init();

        public abstract void CreateBoard(object payload, Action<int, int> _createdTile = null, Action<object> _endCreationCallback = null);

        #endregion Methods
    }
}