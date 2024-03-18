using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TigerAndColoredSpheres
{
    public class BallsManager : MonoBehaviour
    {
        static BallsManager _instance;
        public static BallsManager instance
        {
            get
            {
                if (_instance == null) _instance = GameObject.Find("Balls manager").GetComponent<BallsManager>();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public Ball[] balls;
        public Ball bomb;

        public RectTransform ballsPanel;

        public GameObject levelBallPanelPrefab;

        public float levelBallHeightOffset = 20;

        void Start()
        {
            PlatformsManager.instance.onPlatformCreated += CreateRandomBallOnPlatform;

            CreateBallPanels();
        }

        public void CreateRandomBallOnPlatform(Platform platform, bool isBombPlatform)
        {
            GameObject newBallObject = Instantiate(isBombPlatform ? bomb.gameObject : balls[Random.Range(0, balls.Length)].gameObject,
            platform.transform.position,
            Quaternion.identity);
            newBallObject.transform.SetParent(platform.transform);

            Ball newBall = newBallObject.GetComponent<Ball>();
            newBallObject.name = newBall.colorName;

            platform.ball = newBall;
        }

        public float GetPanelHeight(int index)
        {
            return (levelBallPanelPrefab.GetComponent<RectTransform>().sizeDelta.y + levelBallHeightOffset) * index;
        }

        public static bool TryPickUpBall(Ball ball)
        {
            for (int b = 0; b < instance.ballPanels.Count; b++)
            {
                if (instance.ballPanels[b].data.count > 0 && instance.ballPanels[b].data.ball.colorName == ball.colorName)
                {
                    instance.ballPanels[b].SubstractBall();
                    if (!instance.CheckHasLevelBalls()) instance.OnCollectAllBalls();
                    return true;
                }
            }
            return false;
        }

        public void OnCollectAllBalls()
        {
            PanelsManager.OpenLevelCompletePanel();
            TigerPlayer.instance.isFreeze = true;
            PlatformsManager.instance.generatePlatforms = true;
        }

        public bool CheckHasLevelBalls()
        {
            for (int b = 0; b < ballPanels.Count; b++)
            {
                if (ballPanels[b].data.count > 0) return true;
            }
            return false;
        }
        public void CreateBallPanels()
        {
            LevelBall[] levelBalls = LevelsManager.currentLevel.levelBalls;
            for (int b = 0; b < levelBalls.Length; b++)
            {
                InitializeLevelBall(levelBalls[b].ball, levelBalls[b].count, b);
            } 
            ballsPanel.sizeDelta = new Vector2(ballsPanel.sizeDelta.x, GetPanelHeight(levelBalls.Length) + levelBallHeightOffset);
        }
        public List<BallPanel> ballPanels = new List<BallPanel>();

        [System.Serializable]
        public class BallPanel
        {
            public GameObject panel;
            public Image icon;
            public Text countText;
            public LevelBall data;

            public void SubstractBall()
            {
                data.count--;
                countText.text = data.count.ToString();
                if (data.count <= 0)
                {
                    panel.GetComponent<Image>().color = new Color(1, 1, 1, 0.4f);
                }
            }

            public BallPanel(GameObject _panel, Image _icon, Text _countText, Ball ball, int ballsCount)
            {
                panel = _panel;
                icon = _icon;
                icon.color = ball.color;
                countText = _countText;
                data = new LevelBall();
                data.ball = ball;
                data.count = ballsCount;
                countText.text = ballsCount.ToString();
            }
        }
        public void InitializeLevelBall(Ball ball, int ballsCount, int index)
        {
            GameObject levelBallPanelObject = Instantiate(levelBallPanelPrefab);
            levelBallPanelObject.name = ball.colorName;

            levelBallPanelObject.GetComponent<RectTransform>().SetParent(ballsPanel);
            levelBallPanelObject.transform.localScale = Vector3.one;

            BallPanel levelBallPanel = new BallPanel(levelBallPanelObject,
                levelBallPanelObject.transform.GetChild(0).GetComponent<Image>(),//Icon
                levelBallPanelObject.transform.GetChild(1).GetComponent<Text>(),//Text
                ball, ballsCount);

            ballPanels.Add(levelBallPanel);
            levelBallPanelObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -GetPanelHeight(index) - levelBallHeightOffset);
        }
    }
    [System.Serializable]
    public struct LevelBall
    {
        public Ball ball;
        public int count;
    }
}