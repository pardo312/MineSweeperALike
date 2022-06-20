using JiufenPackages.SceneFlow.Logic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.UIHelpers
{
    public class SoloSelectPanelController : SelectPanelControllerBase
    {
        public UnityEvent<int> loadGameEvent;
        public UnityEvent<int> newGameEvent;

        public override void ShowWholePanel(RectTransform _panelRectTransform)
        {
            for (int i = 0; i < panels.Length; i++)
            {
                DataManager.m_instance.ReadEvent(DataKeys.CHECK_SAVED_BOARD_STATE, i, (dataResponse) =>
                {
                    bool gameExist = (bool)dataResponse.data;
                    int difficulty = i;

                    GameObject buttonLoad = panels[i].optionsContainer.GetChild(0).gameObject;
                    GameObject separator = panels[i].optionsContainer.GetChild(1).gameObject;
                    GameObject buttonNewGame = panels[i].optionsContainer.GetChild(panels[i].optionsContainer.childCount - 1).gameObject;

                    buttonLoad.SetActive(gameExist);
                    if (gameExist)
                        buttonLoad.GetComponent<Button>().onClick.AddListener(() => loadGameEvent?.Invoke(difficulty));

                    separator.SetActive(gameExist);

                    buttonNewGame.GetComponent<Button>().onClick.AddListener(() => newGameEvent?.Invoke(gameExist ? -difficulty : difficulty));

                    if (i == panels.Length - 1)
                        base.ShowWholePanel(_panelRectTransform);
                });
            }
        }
    }
}
