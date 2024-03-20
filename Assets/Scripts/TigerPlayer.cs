using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TigerAndColoredSpheres
{
    public class TigerPlayer : MonoBehaviour
    {
        static TigerPlayer _instance;
        public static TigerPlayer instance
        {
            get
            {
                if (_instance == null) _instance = GameObject.Find("Tiger").GetComponent<TigerPlayer>();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        [Space]
        [Header("Moving")]
        [Space]

        public float jumpTime = 1;

        public float _jumpTime = 0;

        public float jumpHeight = 1;
        public AnimationCurve jumpcurve;

        public Platform currentPlatform;

        bool firstJump = true;

        public float jumpDistance = 3f;
        [Space]
        [Header("Abilities")]
        [Space]

        public AbilityAttribute _currentAbility;

        public float abilityTime;
        public float magneticAbilityDistance;
        public float ballMagnetizationTime = 1f;
        public GameObject protetionEffect;
        public GameObject magnetEffect;

        public AbilityAttribute currentAbility
        {
            get
            {
                return _currentAbility;
            }
            set
            {
                if (value == null)
                {
                    if (_currentAbility.protection) protetionEffect.SetActive(false);
                    if (_currentAbility.magnetic) magnetEffect.SetActive(false);

                    _currentAbility = null;
                    return;
                }
                _currentAbility = value;

                abilityTime = _currentAbility.abilityTime;

                if (_currentAbility.protection) protetionEffect.SetActive(true);
                if (_currentAbility.magnetic) magnetEffect.SetActive(true);
            }
        }
        [Space]
        [Header("Others")]
        [Space]

        public Platform startPlatform;

        //Events
        public UnityAction OnLandPlatformComplete;

        public bool Jump(Platform _targetPlatform)
        {
            if (_jumpTime > 0 || isFreeze || (currentPlatform != null && (currentPlatform == _targetPlatform) ||
                (_targetPlatform.TryGetComponent<FragilePlatform>(out FragilePlatform fragilePlatformIsFalling)) && fragilePlatformIsFalling.isFalling)) return false;

            if (Vector3.Distance(transform.position, _targetPlatform.transform.position) > jumpDistance) return false;

            if (firstJump)
            {
                if (startPlatform.TryGetComponent<FragilePlatform>(out FragilePlatform fragilePlatform)) fragilePlatform.Fall();
                lastPosition = GetPos2D(startPlatform.transform.position);
                firstJump = false;
            }
            else
            {
                if (currentPlatform != null) lastPosition = GetPos2D(currentPlatform.transform.position);
                else lastPosition = GetPos2D(transform.position);

                if (currentPlatform.type.isFast)
                {
                    PlatformsManager.NormalizeRowSpeed(currentPlatform.row);
                }
            }

            currentPlatform = _targetPlatform;

            _jumpTime = jumpTime;

            SoundsManager.PlaySound("Jump");

            return true;
        }
        Vector2 lastPosition;
        Vector2 GetPos2D(Vector3 pos)
        {
            return new Vector2(pos.x, pos.z);
        }

        public bool isFreeze;

        private void Update()
        {
            if (isFreeze)
            {
                if (currentPlatform != null && currentPlatform.TryGetComponent<FragilePlatform>(out FragilePlatform fragilePlatform) && fragilePlatform.isFalling) transform.position = currentPlatform.transform.position;
                return;
            }
            if (_jumpTime > 0) //Jumping
            {
                _jumpTime -= Time.deltaTime;

                float lerp = 1f / jumpTime * _jumpTime;
                lerp = 1f - lerp;

                Vector2 horizontalLerp = Vector2.Lerp(lastPosition, GetPos2D(currentPlatform.transform.position), lerp);
                transform.position = new Vector3(horizontalLerp.x, jumpcurve.Evaluate(lerp) * jumpHeight, horizontalLerp.y);

                if (_jumpTime < 0)
                {
                    OnLandPlatform(currentPlatform);
                }
            }
            else //Riding platform
            {
                if (currentPlatform != null)
                {
                    transform.position = currentPlatform.transform.position;

                    if (currentPlatform.type.isFast)
                    {
                        PlatformsManager.SpeedUpRow(currentPlatform.row);
                    }
                    if (currentPlatform.TryGetComponent<FragilePlatform>(out FragilePlatform fragilePlatform) && fragilePlatform.isFalling)
                    {
                        OnLoose();

                        if (currentAbility != null && currentAbility.protection)
                        {
                            JumpClosestPlatform();
                        }
                    }
                }
            }

            if (!PlatformsManager.IsPointIngameZone(transform.position))
            {
                OnLoose(true);
            }

            //Ability
            if (currentAbility == null)
            {
                return;
            }
            else
            {
                abilityTime -= Time.deltaTime;
                AbilitiesManager.SetAbilityTime(1f / currentAbility.abilityTime * abilityTime, currentAbility);
                if (abilityTime <= 0)
                {
                    currentAbility = null;
                    AbilitiesManager.SetAbilitiesTime(0f);
                }
                else if (currentAbility.magnetic)
                {
                    Platform[] platformsWithball = PlatformsManager.FindPlatformsAtRange(transform.position, false, true, magneticAbilityDistance);
                    for (int p = 0; p < platformsWithball.Length; p++)
                    {
                        if (BallsManager.TryPickUpBall(platformsWithball[p].ball))
                        {
                            platformsWithball[p].PickUpBallMagnet();
                        }
                    }
                }
            }
        }

        public void OnLoose(bool ignoreProtection = false)
        {
            if ((currentAbility != null && !ignoreProtection && currentAbility.protection) || isFreeze) return;
            isFreeze = true;
            PlatformsManager.instance.generatePlatforms = true;
            PanelsManager.OpenGameOverPanel();
        }

        public void OnLandPlatform(Platform _targetPlatform)
        {
            if (_targetPlatform.type.isSlippery && (_targetPlatform.ball == null || _targetPlatform.ball.colorName != "Bomb"))
            {
                JumpClosestPlatform();
            }
            if (_targetPlatform.type.isFragile)
            {
                _targetPlatform.GetComponent<FragilePlatform>().StartCrumbling();
            }
            if (_targetPlatform.ball != null)
            {
                if (_targetPlatform.ball.TryGetComponent<Bomb>(out Bomb bomb))
                {
                    OnLoose();
                    bomb.Explode();
                    _targetPlatform.ball = null;
                    return;
                }

                if (BallsManager.TryPickUpBall(_targetPlatform.ball))
                {
                    _targetPlatform.PickUpBall();
                }
            }
            if (OnLandPlatformComplete != null) OnLandPlatformComplete();
        }

        public void JumpClosestPlatform()
        {
            Platform closestPlatform = PlatformsManager.FindRandomPlatformAtRange(transform.position, true, false, jumpDistance);
            if (closestPlatform != null)
            {
                Jump(closestPlatform);
            }
            else
            {
                Jump(PlatformsManager.FindRandomPlatformAtRange(transform.position, false, false, jumpDistance));
            }
        }

#if UNITY_EDITOR
        [Space]
        [Header("Gizmos. Jump radius")]
        [Space]
        [Range(0, 1)] public float gizmosFade = 1;
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 0, gizmosFade);
            Gizmos.DrawWireSphere(transform.position, jumpDistance);

            Gizmos.color = new Color(0, 1, 0, gizmosFade);
            Gizmos.DrawWireSphere(transform.position, magneticAbilityDistance);
        }
#endif
    }
}