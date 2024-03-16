using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TigerAndColoredSpheres
{
    public class GameManager : MonoBehaviour
    {
        static GameManager _instance;
        public static GameManager instance
        {
            get
            {
                if (_instance == null) _instance = GameObject.Find("Game manager").GetComponent<GameManager>();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public GameObject[] loadDataObjects;
        private void Start()
        {
            LoadGame();
        }
        public static void TogglePauseGame()
        {
            if (gamePaused)
            {
                UnpauseGame();

                PanelsManager.ClosePausePanel();
            }
            else
            {
                PauseGame();

                PanelsManager.OpenPausePanel();
            }
        }
        static bool gamePaused = false;
        public static void PauseGame()
        {
            Time.timeScale = 0;
            gamePaused = true;
        }
        public static void UnpauseGame()
        {
            Time.timeScale = 1;
            gamePaused = false;
        }

        public static void GoToMenu()
        {
            PanelsManager.ClosePausePanel();
        }

        void LoadGame()
        {
            for (int o = 0; o < loadDataObjects.Length; o++) loadDataObjects[o].GetComponent<ILoadData>().LoadData();
        }

        public static void RestartGame()
        {
            SceneManager.LoadScene("Game");
        }
    }
    public interface ILoadData
    {
        public void LoadData();
    }
}
