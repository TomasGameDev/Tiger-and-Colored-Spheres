using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TigerAndColoredSpheres
{
    public class LevelsManager : MonoBehaviour
    {
        static LevelsManager _instance;
        public static LevelsManager instance
        {
            get
            {
                if (_instance == null) _instance = GameObject.Find("Levels manager").GetComponent<LevelsManager>();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public LevelAttribute[] levelAttributes;

        public LevelAttribute endlessLevelAttributes;

        [Tooltip("Affects level selection if greater than 0.")]
        public int setLevel = 0;

        public Text levelText;

        public static LevelAttribute currentLevel
        {
            get
            {
                return currentLevelIndex > 0 ? instance.levelAttributes[currentLevelIndex - 1] : instance.endlessLevelAttributes;
            }
        }

        public static int currentLevelIndex
        {
            get
            {
                if (instance.setLevel != 0) return instance.setLevel;
                return PlayerPrefs.GetInt("LEVEL");
            }
            set
            {
                if (instance.levelText != null)
                {
                    instance.levelText.text = currentLevelIndex > 0 ? "Level " + value.ToString() : "";
                }
                PlayerPrefs.SetInt("LEVEL", value);
            }
        }
        public static int levelsCount
        {
            get
            {
                return instance.levelAttributes.Length;
            }
        }
        public static int levelsCompleted
        {
            get
            {
                return PlayerPrefs.GetInt("LEVELS_COMPLETED");
            }
            set
            {
                PlayerPrefs.SetInt("LEVELS_COMPLETED", value);
            }
        }
        private void Start()
        {
            instance = this;
            currentLevelIndex = currentLevelIndex;
            if (levelsCompleted <= 0) levelsCompleted = 1;
            print(levelsCompleted);
        }
    }
}