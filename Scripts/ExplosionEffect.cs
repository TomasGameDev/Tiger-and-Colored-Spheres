using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TigerAndColoredSpheres
{
    public class ExplosionEffect : MonoBehaviour
    {
        public float explosionTime;
        public float explosionSpeed = 1f;
        public float explosionSize = 10f;

        MeshRenderer meshRenderer;

        public Color explosionStart;
        public Color explosionEnd;

        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void FixedUpdate()
        {
            if (explosionTime > explosionSize)
            {
                //Destroy(gameObject);
            }
            else
            {
                explosionTime += explosionSpeed * Time.deltaTime;
                transform.localScale = new Vector3(explosionTime, explosionTime, explosionTime);

                meshRenderer.material.color = Color.Lerp(explosionStart, explosionEnd, 1f / explosionSize * explosionTime);
            }
        }
    }
}