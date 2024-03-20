using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TigerAndColoredSpheres
{
    public class GameTutorial : MonoBehaviour
    {
        public Transform firstPlatform;
        public GameObject colorBallTutorialPanel;
        public GameObject bombTutorialPanel;
        private void Start()
        {
            if (PlayerPrefs.GetInt("TUTORIAL") == 1 || (PlayerPrefs.GetInt("TUTORIAL") == 2 && LevelsManager.currentLevelIndex == -1))
            {
                gameObject.SetActive(false);
                return;
            }
            PlatformsManager.instance.onPlatformCreated += SetFirstPlatform;
            TigerPlayer.instance.OnLandPlatformComplete += OnClickPlatform;
        }
        public void SetFirstPlatform(Platform platform, bool isBomb)
        {
            if (!isBomb && platform.type.isDefault)
            {
                firstPlatform = PlatformsManager.instance.rows[0].platforms[0].transform;
                clickPlatform.gameObject.SetActive(true);
                PlatformsManager.instance.onPlatformCreated -= SetFirstPlatform;
            }
        }
        public RectTransform clickPlatform;
        public void OnClickPlatform()
        {
            clickPlatform.gameObject.SetActive(false);
            TigerPlayer.instance.OnLandPlatformComplete -= OnClickPlatform;
            if (LevelsManager.currentLevelIndex == -1)
            {
                CompleteTutorialEndlessLevel();
                return;
            }
            colorBallTutorialPanel.SetActive(true);
        }
        public void OpenBombTutorial()
        {
            bombTutorialPanel.SetActive(true);
        }
        public void CompleteTutorial()
        {
            PlayerPrefs.SetInt("TUTORIAL", 1);
            gameObject.SetActive(false);
            PlatformsManager.instance.gameFrozen = false;
        }
        public void CompleteTutorialEndlessLevel()
        {
            PlayerPrefs.SetInt("TUTORIAL", 2);
            gameObject.SetActive(false);
            PlatformsManager.instance.gameFrozen = false;
        }
        void Update()
        {
            if (Mathf.Abs(firstPlatform.position.x) < 0.4f)
            {
                PlatformsManager.instance.gameFrozen = true;
            }
            clickPlatform.position = Camera.main.WorldToScreenPoint(firstPlatform.position);
        }
    }
}