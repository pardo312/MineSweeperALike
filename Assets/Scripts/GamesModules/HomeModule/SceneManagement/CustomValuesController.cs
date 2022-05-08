using JiufenGames.MineSweeperAlike.SceneManagement;
using JiufenPackages.GameManager.Logic;
using JiufenPackages.SceneFlow.Logic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.HomeModule
{

    public class CustomValuesController : MonoBehaviour
    {
        #region ----Fields----
        public Action<int, int, int> OnCustomValuesPlay;
        public CustomValuesView view;
        //0,1 -> row,col ; 2-> numOfMines
        public Slider[] listOfSliders;
        #endregion ----Fields----

        #region ----Methods----
        public void SendCustomValues()
        {
            OnCustomValuesPlay?.Invoke((int)listOfSliders[0].value, (int)listOfSliders[1].value, (int)listOfSliders[2].value);
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
