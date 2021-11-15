using JiufenGames.MineSweeperAlike.Board.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private BoardController m_boardController;
        void Start()
        {
            m_boardController.Init();

        }
    }
}
