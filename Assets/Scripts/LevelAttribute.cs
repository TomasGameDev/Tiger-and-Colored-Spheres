using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TigerAndColoredSpheres
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level", order = 5000)]
    public class LevelAttribute : ScriptableObject
    {
        public int rowsCount = 2;
        public float platformSpawnRate;
        public float platformsSpeedMultiplier;

        public LevelPlatform[] levelPlatforms;

        public LevelBall[] levelBalls;

        [Range(0, 100)] public int gapChance = 10;
        [Range(0, 100)] public int bombChance = 10;
    }
}