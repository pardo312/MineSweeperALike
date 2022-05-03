using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.SceneManagement
{
    public class BGHomeAnimation : MonoBehaviour
    {
        public Image bgPrefab;
        public Transform parentBG;
        [Range(1, 16)]
        public int numberOfLines = 4;
        [Range(.3f,3)]
        public float distanceBetweenBars = 1;

        private List<Image> bgList = new List<Image>();
        private float scaleFactor;
        private float deltaA = 2.5f;
        public void Start()
        {
            scaleFactor = .4f / numberOfLines;
            for (int i = 0; i < numberOfLines; i++)
            {
                Image newBg = Instantiate(bgPrefab, parentBG);
                newBg.name = i.ToString();

                //Scale
                newBg.transform.localScale += new Vector3(.05f - (scaleFactor * distanceBetweenBars * i), .025f - (scaleFactor * distanceBetweenBars * .5f * i), 0);

                //Alpha
                Image imageBg = newBg;
                Color temp = imageBg.color;
                temp.a -= (scaleFactor * deltaA * i);
                imageBg.color = temp;

                //Add to list
                bgList.Add(newBg);
            }
        }

        public void Update()
        {
            float scaleDelta = (scaleFactor) / (60 * 2);
            for (int i = 0; i < bgList.Count; i++)
            {
                Image imageBg = bgList[i];
                Color temp = imageBg.color;
                if (imageBg.color.a <= 0)
                {
                    //Scale
                    bgList[i].transform.localScale = new Vector3(1.05f, 1.025f, 1);
                    Debug.Log($"Tile {i} has reset");

                    //Alpha
                    temp.a = 1;
                    imageBg.color = temp;
                    return;
                }

                //Scale
                bgList[i].transform.localScale -= new Vector3(scaleDelta * distanceBetweenBars, scaleDelta * (distanceBetweenBars * .5f), 0);

                //Alpha
                temp.a -= (scaleDelta * deltaA);
                imageBg.color = temp;
            }
        }
    }
}