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

        void Start()
        {
            CreateLevels();
        }
        int columns = 2;
        int rows = 5;
        void CreateLevels()
        {
            int levelsCount = LevelsManager.levelsCount;
            Vector2 buttonSize = levelButtonPrefab.GetComponent<RectTransform>().sizeDelta;
            int row = 0;
            int column = 0;
            for (int l = 0; l < rows; l++)
            {
                GameObject levelButton = Instantiate(levelButtonPrefab);
                levelButton.name = "Level button " + (l + 1).ToString();
                levelButton.GetComponent<RectTransform>().SetParent(levelsPanel);
                levelButton.GetComponent<RectTransform>().localScale = Vector3.one;
                levelButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    -(offest.x + buttonSize.x) * column - offest.x,
                    -(offest.y + buttonSize.y) * row - offest.y
                    );
                column++;
                if (column == columns)
                {
                    row++;
                    column = 0;
                }

                levelButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(l + 1); });
                levelButton.transform.GetChild(0).GetComponent<Text>().text = (l + 1).ToString();
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
            int lastPanelIndex = Mathf.CeilToInt(levels.Count / columns * rows) - 1;
            if (panelIndex == lastPanelIndex) return;
            panelIndex++; 
            for (int l = 0; l < levels.Count; l++)
            {
                levels[l].transform.GetChild(0).GetComponent<Text>().text = (l + 1 + columns * rows * panelIndex).ToString();
            }
            if (panelIndex == lastPanelIndex) nextLevelsButton.SetActive(false);
            else nextLevelsButton.SetActive(true);
        }
        public void PrewLevels()
        {
            if (panelIndex == 0) return;
            panelIndex--;
            for (int l = 0; l < levels.Count; l++)
            {
                levels[l].transform.GetChild(0).GetComponent<Text>().text = (l + 1 + columns * rows * panelIndex).ToString();
            }
            if (panelIndex == 0) prewLevelsButton.SetActive(false);
            else prewLevelsButton.SetActive(true);
        }
        void OnButtonClick(int levelId)
        {
            LevelsManager.currentLevelIndex = columns * rows * panelIndex + levelId;
            SceneManager.LoadScene("Game");
        }
    }
}