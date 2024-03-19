using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TigerAndColoredSpheres
{
    public class PanelsManager : MonoBehaviour
    {
        static PanelsManager _instance;
        public static PanelsManager instance
        {
            get
            {
                if (_instance == null) _instance = GameObject.Find("Panels manager").GetComponent<PanelsManager>();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public GameObject gamePanel;
        public GameObject levelCompletePanel;
        public GameObject gameOverPanel;
        public GameObject buttonsPanel;
        public GameObject cameraPanel;
        public GameObject purchaseUnsuccessfulPanel;
        public GameObject pausePanel;

        public static void OpenLevelCompletePanel()
        {
            instance.levelCompletePanel.SetActive(true);
            instance.gamePanel.SetActive(false);
            instance.cameraPanel.GetComponent<Image>().enabled = false;
            instance.purchaseUnsuccessfulPanel.SetActive(false);
            instance.pausePanel.SetActive(false);
        }

        public static void CloseLevelCompletePanel()
        {
            LevelsManager.currentLevelIndex++;
            LevelsManager.levelsCompleted++;
            GameManager.RestartGame();
        }

        public static void OpenGameOverPanel()
        {
            instance.gameOverPanel.SetActive(true);
            instance.buttonsPanel.SetActive(true);
            instance.pausePanel.SetActive(false);
        }
        public static void CloseGameOverPanel()
        {
            GameManager.RestartGame();
        }

        public static void OpenPausePanel()
        {
            instance.gamePanel.SetActive(false);
            instance.cameraPanel.GetComponent<Image>().enabled = false;
            instance.pausePanel.SetActive(true);
            instance.purchaseUnsuccessfulPanel.SetActive(false);
        }
        public static void ClosePausePanel()
        {
            instance.gamePanel.SetActive(true);
            instance.pausePanel.SetActive(false);
            instance.cameraPanel.GetComponent<Image>().enabled = true;
        }
        public static void OpenPurchaseUnsuccessfulPanel()
        {
            instance.purchaseUnsuccessfulPanel.SetActive(true);
            GameManager.PauseGame();
        }
        public static void ClosePurchaseUnsuccessfulPanel()
        {
            instance.purchaseUnsuccessfulPanel.SetActive(false);
            GameManager.UnpauseGame();
        }
    }
}