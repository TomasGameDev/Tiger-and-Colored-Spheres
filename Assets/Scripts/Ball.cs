using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TigerAndColoredSpheres
{
    public class Ball : MonoBehaviour
    {
        public string colorName = "Red";
        [Tooltip("Level balls icon color")]
        public Color color = Color.red;
        public void PickUp()
        {
            CancelInvoke("FlyToPlayer");
            SoundsManager.PlaySound("Collect");
            DestroyBall();
            GameShop.balance += GameShop.instance.ballPrice;
        }
        public void PickMagnet()
        {
            platformPos = transform.position;
            flyTime = TigerPlayer.instance.ballMagnetizationTime * Random.Range(0.9f, 1.1f);
            InvokeRepeating("FlyToPlayer", 0, Time.deltaTime);
        }
        float flyTime;
        Vector3 platformPos;
        void FlyToPlayer()
        {
            flyTime -= Time.deltaTime;
            transform.position = Vector3.Lerp(platformPos, TigerPlayer.instance.transform.position,
                1f - (1f / TigerPlayer.instance.ballMagnetizationTime * flyTime));
            if (flyTime <= 0)
            {
                PickUp();
                CancelInvoke("FlyToPlayer");
            }
        }
        public void DestroyBall()
        {
            Destroy(gameObject);
        }
    }
}