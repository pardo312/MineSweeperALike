using TMPro;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.TimerModule
{
    public class TimerController : MonoBehaviour
    {
        #region ----Fields----
        [SerializeField] private TMP_Text timerText;
        private float timer;
        private bool isReady;
        #endregion ----Fields----

        #region ----Methods----
        public void Init(int counterInit)
        {
            isReady = true;
            timer = counterInit;
        }

        public int GetCurrentTimer()
        {
            return Mathf.FloorToInt(timer);
        }

        public void Update()
        {
            if (!isReady)
                return;

            timer += Time.deltaTime;
            SetTimerText();
        }


        private void SetTimerText()
        {
            int timerAsInt = Mathf.FloorToInt(timer);

            if (timerAsInt < 60)
            {
                timerText.text = $"0:{FormatDecimalString(timerAsInt)}";
            }
            else if (timerAsInt < 3600)
            {
                int minutes = timerAsInt / 60;
                int seconds = timerAsInt % 60;

                timerText.text = $"{FormatDecimalString(minutes)}:{FormatDecimalString(seconds)}";
            }
            else
            {
                int seconds = timerAsInt % 60;
                int minutes = (timerAsInt / 60) % 60;
                int hours = (timerAsInt / 60) / 60;

                timerText.text = $"{FormatDecimalString(hours)}:{FormatDecimalString(minutes)}:{FormatDecimalString(seconds)}";
            }
        }

        private string FormatDecimalString(int value)
        {
            if (value < 10)
                return $"0{value}";
            else
                return $"{value}";
        }
        #endregion ----Methods----
    }
}