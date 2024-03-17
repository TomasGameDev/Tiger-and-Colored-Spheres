using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TigerAndColoredSpheres
{
    public class MenuLevels : MonoBehaviour
    {
        public RectTransform levelsPanel;
        public GameObject levelButtonPrefab;
        public List<GameObject> levels;
        public GameObject nextLevelsButton;
        public GameObject prewLevelsButton;
        public Vector2 offest;

        public int rows = 6;
        public int columns = 2;

        void Start()
        {
            CreateLevels();
            UpdateText();
        }
        void CreateLevels()
        {
            int levelsCount = LevelsManager.levelsCount;
            Vector2 buttonSize = levelButtonPrefab.GetComponent<RectTransform>().sizeDelta;
            int row = 0;
            int column = 0;
            for (int l = 0; l < rows * columns; l++)
            {
                int buttonIndex = l + 1;
                GameObject levelButton = Instantiate(levelButtonPrefab);
                levelButton.name = "Level button " + buttonIndex.ToString();
                levelButton.GetComponent<RectTransform>().SetParent(levelsPanel);
                levelButton.GetComponent<RectTransform>().localScale = Vector3.one;
                levelButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    (offest.x + buttonSize.x) * column + offest.x,
                    -(offest.y + buttonSize.y) * row - offest.y
                    );
                column++;
                if (column == columns)
                {
                    row++;
                    column = 0;
                }
                levelButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(buttonIndex); });

                levelButton.transform.GetChild(0).GetComponent<Text>().text = buttonIndex.ToString();
                levels.Add(levelButton);
            }
            levelsPanel.sizeDelta = new Vector2(
                    (offest.x + buttonSize.x) * columns + offest.x,
                    (offest.y + buttonSize.y) * rows + offest.y
                    );
        }

        int panelIndex = 0;
        public void NextLevels()
        {
            int lastPanelIndex = Mathf.CeilToInt(LevelsManager.levelsCount / (columns * rows));
            if (panelIndex >= lastPanelIndex) return;
            panelIndex++;
            UpdateText();
            if (panelIndex >= lastPanelIndex)
            {
                nextLevelsButton.SetActive(false);
                prewLevelsButton.SetActive(true);
            }
            else
            {
                nextLevelsButton.SetActive(true);
                prewLevelsButton.SetActive(true);
            }
        }
        public void PrewLevels()
        {
            if (panelIndex == 0) return;
            panelIndex--;
            UpdateText();
            if (panelIndex <= 0)
            {
                prewLevelsButton.SetActive(false);
                nextLevelsButton.SetActive(true);
            }
            else
            {
                prewLevelsButton.SetActive(true);
                nextLevelsButton.SetActive(true);
            }
        }
        void UpdateText()
        {
            for (int l = 0; l < levels.Count; l++)
            {
                int index = l + 1 + columns * rows * panelIndex;
                if (index > LevelsManager.levelsCount) levels[l].SetActive(false);
                else
                {
                    if (index > LevelsManager.levelsCompleted)
                    {
                        levels[l].GetComponent<Button>().interactable = false;
                    }
                    else
                    {
                        levels[l].GetComponent<Button>().interactable = true;
                    }
                    levels[l].SetActive(true);
                    levels[l].transform.GetChild(0).GetComponent<Text>().text = (index).ToString();
                }
            }
        }
        void OnButtonClick(int levelIndex)
        {
            int _levelIndex = columns * rows * panelIndex + levelIndex;
            SoundsManager.PlaySoundOutScene("Button click");
            LevelsManager.currentLevelIndex = _levelIndex;
            SceneManager.LoadScene("Game");
        }
    }
}