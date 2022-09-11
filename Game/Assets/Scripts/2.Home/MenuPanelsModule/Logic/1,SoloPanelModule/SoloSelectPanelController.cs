using JiufenPackages.SceneFlow.Logic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.UIHelpers
{
    public class SoloSelectPanelController : SelectPanelControllerBase
    {
        [Header("Predifine difficulty")]
        public UnityEvent<int> loadGameEvent;
        public UnityEvent<int> newGameEvent;

        [Header("Custom difficulty")]
        public UnityEvent<int> loadCustomGameEvent;
        public UnityEvent<int> newCustomGameEvent;

        public override void ShowWholePanel()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                DataManager.m_instance.ReadEvent(DataKeys.CHECK_SAVED_BOARD_STATE, i, (dataResponse) =>
                {
                    bool gameExist = (bool)dataResponse.data;
                    bool isCustomPanel = i == panels.Length - 1;

                    int difficulty = i;

                    GameObject buttonLoad = panels[i].optionsContainer.GetChild(0).gameObject;
                    GameObject separator = panels[i].optionsContainer.GetChild(1).gameObject;
                    GameObject buttonNewGame = panels[i].optionsContainer.GetChild(panels[i].optionsContainer.childCount - 1).gameObject;

                    buttonLoad.SetActive(gameExist);

                    UnityEvent<int> loadEventToInkvoke = isCustomPanel ? loadCustomGameEvent : loadGameEvent;
                    if (gameExist)
                        buttonLoad.GetComponent<Button>().onClick.AddListener(() => loadEventToInkvoke?.Invoke(difficulty));

                    separator.SetActive(gameExist);

                    UnityEvent<int> newEventToInkvoke = isCustomPanel ? newCustomGameEvent : newGameEvent;
                    buttonNewGame.GetComponent<Button>().onClick.AddListener(() => newEventToInkvoke?.Invoke(gameExist ? -difficulty : difficulty));

                    base.ShowWholePanel();
                });
            }
        }
    }
}
