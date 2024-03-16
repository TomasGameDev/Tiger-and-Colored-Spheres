using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TigerAndColoredSpheres
{
    public class Platform : MonoBehaviour
    {
        public PlatformAttribute type;

        public int row = -1;

        public Ball ball;

        public void DestroyPlatform()
        {
            if (row >= 0)
            {
                PlatformsManager.instance.DestroyPlatform(row, this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PickUpBall()
        {
            ball.PickUp();
            ball = null;
        }
        public void PickUpBallMagnet()
        {
            ball.PickMagnet();
            ball = null;
        }
    }
    [System.Serializable]
    public struct LevelPlatform
    {
        public PlatformAttribute platform;
        [Range(1, 100)] public int spawnChance;
    }
}