using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TigerAndColoredSpheres
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Ability", order = 5001)]
    public class AbilityAttribute : ScriptableObject
    {
        public string abilityName;
        public Sprite icon;
        public int prtice = 100;
        [Tooltip("Seconds")]
        public float abilityTime = 15;
        [Space]
        [Header("Abilities")]
        [Space]
        public bool protection = false;
        public bool magnetic = false;
    }
}