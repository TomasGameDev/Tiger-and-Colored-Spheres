using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TigerAndColoredSpheres
{
    public class MagnetEffect : MonoBehaviour
    {
        public Transform[] particles;
        public float animationSpeed = 1;
        void Update()
        {
            float magnetDist = TigerPlayer.instance.magneticAbilityDistance;
            for (int p = 0; p < particles.Length; p++)
            {
                if (particles[p].localPosition.magnitude < 0.1f)
                {
                    particles[p].localPosition = new Vector3(
                    Random.Range(-magnetDist, magnetDist),
                    Random.Range(-magnetDist, magnetDist),
                    Random.Range(-magnetDist, magnetDist));
                    particles[p].localPosition = Vector3.ClampMagnitude(particles[p].localPosition, magnetDist);
                    particles[p].LookAt(transform.position);
                }
                particles[p].localPosition = Vector3.MoveTowards(particles[p].localPosition, Vector3.zero, animationSpeed * Time.deltaTime);
            }
        }
    }
}