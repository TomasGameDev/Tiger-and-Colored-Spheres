using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TigerAndColoredSpheres
{
    [CreateAssetMenu(fileName = " Platform", menuName = " Platform", order = 5000)]
    public class PlatformAttribute : ScriptableObject
    {
        public Platform prefab;
        public bool isFast = false;
        public bool isFragile = false;
        public bool isSlippery = false; 
    }
}