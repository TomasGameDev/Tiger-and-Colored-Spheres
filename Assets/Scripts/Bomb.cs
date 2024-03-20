using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TigerAndColoredSpheres
{
    public class Bomb : Ball
    {
        public GameObject explosionEffectPrefab;

        public void Explode()
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            SoundsManager.PlaySound("Explosion");
            Destroy(gameObject);
        }
    }
}