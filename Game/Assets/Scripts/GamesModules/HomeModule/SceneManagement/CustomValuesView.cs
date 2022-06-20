using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.HomeModule
{
    public class CustomValuesView : MonoBehaviour
    {
        #region ----Fields----
        public TMP_Text[] slidersText;
        #endregion ----Fields----

        #region ----Methods----
        public bool UpdateTextView(Slider slider, int index)
        {
            if (!slidersText[index].text.Equals(slider.value.ToString()))
            {
                if (!slider.wholeNumbers)
                    slidersText[index].text = slider.value.ToString("n2");
                else
                    slidersText[index].text = slider.value.ToString();
                return true;
            }
            return false;
        }
        #endregion ----Methods----
    }
}