using JiufenGames.PopupModule;
using JiufenPackages.SceneFlow.Logic;
using JiufenPackages.ServiceLocator;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.HomeModule
{

    public class CustomPanelController : MonoBehaviour
    {
        #region ----Fields----
        public Action<int, int, int, bool> OnCustomValuesPlay;
        public CustomValuesView view;
        //0,1 -> row,col ; 2-> numOfMines
        public Slider[] listOfSliders;
        #endregion ----Fields----

        #region ----Methods----
        public void CleanBoardDataCustom(int dataExist)
        {
            if (dataExist < 0)
            {
                Dictionary<ButtonType, Action> buttonDictionary = new Dictionary<ButtonType, Action>();
                buttonDictionary.Add(ButtonType.BACK, () => { });
                buttonDictionary.Add(ButtonType.CONFIRM, () =>
                {
                    DataManager.m_instance.ReadEvent(DataKeys.SAVE_BOARD_DATA, new BoardSaveData() { difficulty = BoardDifficultyEnum.CUSTOM });
                    SendCustomValues(0);
                    ServiceLocator.m_Instance.GetService<IPopupManager>().HidePopup();
                });
                ServiceLocator.m_Instance.GetService<IPopupManager>().ShowInfoPopup("Do you want to create new game and erase old one?", buttonDictionary);
            }
            else
            {
                DataManager.m_instance.ReadEvent(DataKeys.SAVE_BOARD_DATA, new BoardSaveData() { difficulty = BoardDifficultyEnum.CUSTOM });
                SendCustomValues(0);
            }
        }

        public void SendCustomValues(int isOldData)
        {
            OnCustomValuesPlay?.Invoke((int)listOfSliders[0].value, (int)listOfSliders[1].value, (int)listOfSliders[2].value, isOldData != 0);
        }

        private void Update()
        {
            for (int i = 0; i < listOfSliders.Length; i++)
            {
                bool hasChanged = view.UpdateTextView(listOfSliders[i], i);
                if (i != listOfSliders.Length - 1 && hasChanged)
                    listOfSliders[listOfSliders.Length - 1].maxValue = listOfSliders[0].value * listOfSliders[1].value;
            }
        }
        #endregion ----Methods----
    }
}
