using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TigerAndColoredSpheres
{
    public class FragilePlatform : Platform
    { 
        public float _lifeTime = 0;
        public float lifeTime
        {
            get
            {
                return _lifeTime;
            }
            set
            {
                helthBar.SetActive(true);
                _lifeTime = value;
                helthBar.transform.GetChild(0).localScale = new Vector3(1f / PlatformsManager.instance.fragilePlatformLifetime * _lifeTime, 1, 1);
                if (_lifeTime <= 0) Fall();
            }
        }
        public GameObject helthBar;
        public bool isFalling;

        public void Fall()
        {
            isFalling = true;
            InvokeRepeating("Falling", 0, Time.deltaTime);
        }
        void Falling()
        {
            transform.position += Vector3.down * PlatformsManager.instance.platformFallSpeed * Time.deltaTime;

            if (transform.position.y < -PlatformsManager.instance.platformFallHeight)
            {
                CancelInvoke("Falling");
                DestroyPlatform();
            }
        }
        public void StartCrumbling()
        {
            lifeTime = PlatformsManager.instance.fragilePlatformLifetime;
        }
    }
}