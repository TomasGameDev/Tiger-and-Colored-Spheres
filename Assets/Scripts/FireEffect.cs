using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffect : MonoBehaviour
{
    public Transform[] particles;
    public float animationSpeed = 10;
    public float animationRange = 12;
    private void Start()
    {
        for (int p = 0; p < particles.Length; p++)
        {
            particles[p].rotation = Random.rotation;
            particles[p].localPosition += particles[p].forward * Random.Range(0, animationRange);
        }
    }
    void Update()
    {
        float magnetDist = animationRange;
        for (int p = 0; p < particles.Length; p++)
        {
            if (particles[p].localPosition.magnitude > animationRange)
            {
                particles[p].localPosition = Vector3.zero;
                particles[p].rotation = Random.rotation;
            }
            particles[p].localPosition += particles[p].forward * animationSpeed * Time.deltaTime;
        }
    }
}
