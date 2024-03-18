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
            if(PlayerPrefs.GetInt("TUTORIAL") == 1)
            {
                gameObject.SetActive(false);
                return;
            }
            PlatformsManager.instance.onPlatformCreated += SetFirstPlatform;
            GameCamera.instance.onSelectPlatform += OnClickPlatform;
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
            PlatformsManager.instance.gameFrozen = false;
            clickPlatformTutorial = true;
            clickPlatform.gameObject.SetActive(false);
            GameCamera.instance.onSelectPlatform -= OnClickPlatform;
            colorBallTutorialPanel.SetActive(true);
        }
        public void OpenBombTutorial()
        {
            bombTutorialPanel.SetActive(true);
        }
        public void CompleteTutorial()
        {
            PlayerPrefs.SetInt("TUTORIAL", 1);
        }
        public bool clickPlatformTutorial; 
        void Update()
        {
            if (!clickPlatformTutorial)
            {
                if (Mathf.Abs(firstPlatform.position.x) < 0.4f)
                {
                    PlatformsManager.instance.gameFrozen = true;
                }
                clickPlatform.position = Camera.main.WorldToScreenPoint(firstPlatform.position);
            }
        }
    }
}