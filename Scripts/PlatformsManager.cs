using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TigerAndColoredSpheres
{
    public class PlatformsManager : MonoBehaviour
    {
        static PlatformsManager _instance;
        public static PlatformsManager instance
        {
            get
            {
                if (_instance == null) _instance = GameObject.Find("Platforms manager").GetComponent<PlatformsManager>();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        #region Platforms
        [Space]
        [Header("Platforms")]
        [Space]
        public float platformSpawnDistance = 10f;
        [Space]
        public float platformFallSpeed = 1;
        public float platformFallHeight = 3;

        public float fragilePlatformLifetime = 3;

        #endregion
        #region Rows
        [Space]
        [Header("Rows")]
        [Space]

        public PlatformRow[] rows;

        public List<int> platformsChanceList;

        public int rowsCount
        {
            get
            {
                return LevelsManager.currentLevel.rowsCount;
            }
        }
        public void InitializeRows()
        {
            rows = new PlatformRow[LevelsManager.currentLevel.rowsCount];
            for (int r = 0; r < rows.Length; r++) rows[r] = new PlatformRow();

            platformsChanceList = new List<int>();
            for (int p = 0; p < LevelsManager.currentLevel.levelPlatforms.Length; p++)
            {
                for (int c = 0; c < LevelsManager.currentLevel.levelPlatforms[p].spawnChance; c++)
                {
                    platformsChanceList.Add(p);
                }
            }
            if (LevelsManager.currentLevel.gapChance > 0)
            {
                for (int c = 0; c < LevelsManager.currentLevel.gapChance; c++)
                {
                    platformsChanceList.Add(-1);
                }
            }
        }
        [System.Serializable]
        public class PlatformRow
        {
            [Tooltip("Direction of movement.")]
            public float speed;
            public float time;
            public List<Platform> platforms;

            public PlatformRow()//Initialize row
            {
                platforms = new List<Platform>();
                speed = Random.Range(0, 2) * 2 - 1;
                time = LevelsManager.currentLevel.platformSpawnRate - instance.platformSpawnStartDelay;
            }

            [HideInInspector] public bool lastPlatformGap;
        }

        public float rowSize = 1.75f;
        #endregion
        #region Game
        [Space]
        [Header("Game")]
        [Space]

        public float platformsSpeedUpMultiplier = 2;

        [Tooltip("Delay at the beginning of the game.")]
        public float platformSpawnStartDelay = 0f; 

        public bool gameFrozen;
        #endregion
        #region Methods
        public Platform FindPlatform(string name)
        {
            for (int p = 0; p < LevelsManager.currentLevel.levelPlatforms.Length; p++)
            {
                if (LevelsManager.currentLevel.levelPlatforms[p].platform.name.Contains(name)) return LevelsManager.currentLevel.levelPlatforms[p].platform.prefab;
            }
            return null;
        }
        public static bool IsPointIngameZone(Vector3 point)
        {
            return !(Camera.main.WorldToScreenPoint(point).x < 0
                || Camera.main.WorldToScreenPoint(point).x >= Screen.width ||
                Camera.main.WorldToScreenPoint(point).y < 0
                || Camera.main.WorldToScreenPoint(point).y >= Screen.height);
        }
        /*
        public static Platform FindClosestPlatformInRow(int row, Vector3 pos, bool ignoreSlippery = false, bool ignoreBallsEmpty = false, float maxDistance = 1000)
        {
            Platform closestPlatform = null;

            float minDistance = maxDistance;

            for (int p = 0; p < instance.rows[row].platforms.Count; p++)
            {
                Platform _platform = instance.rows[row].platforms[p];

                if (!IsPointIngameZone(_platform.transform.position) || (ignoreSlippery && _platform.type.isSlippery) ||
                    (_platform.ball != null && _platform.ball.isBomb) || (ignoreBallsEmpty && _platform.ball == null)) continue;

                float currentDistance = Vector3.Distance(pos, _platform.transform.position);
                if (currentDistance > minDistance) continue;

                if (currentDistance < minDistance)
                {
                    closestPlatform = _platform;
                    minDistance = currentDistance;
                }
            }
            return closestPlatform;
        }
        public static Platform FindClosestPlatform(Vector3 pos, bool ignoreSlippery = false, bool ignoreBallsEmpty = false, float maxDistance = 1000)
        {
            int row = Mathf.RoundToInt(pos.z / instance.rowSize);

            Platform closestTopPlatform = null;
            Platform closestbottomPlatform = null;

            //Checking the top row
            if (row < instance.rowsCount - 2)
            {
                closestTopPlatform = FindClosestPlatformInRow(row + 1, pos, ignoreSlippery, ignoreBallsEmpty, maxDistance);
            }

            //Checking the bottom row
            if (row > 1)
            {
                closestbottomPlatform = FindClosestPlatformInRow(row - 1, pos, ignoreSlippery, ignoreBallsEmpty, maxDistance);
            }

            if (closestTopPlatform == null && closestbottomPlatform == null)
            {
                return FindClosestPlatformInRow(row, pos, ignoreSlippery, ignoreBallsEmpty, maxDistance);
            }
            else if (closestTopPlatform == null || closestbottomPlatform == null)
            {
                return closestTopPlatform == null ? closestbottomPlatform : closestTopPlatform;
            }
            else
            {
                return Vector3.Distance(pos, closestTopPlatform.transform.position) > Vector3.Distance(pos, closestbottomPlatform.transform.position) ? closestbottomPlatform : closestTopPlatform;
            }
        }
        */
        public static Platform[] FindPlatformsAtRange(Vector3 pos, bool ignoreSlippery = false, bool ignoreBallsEmpty = false, float range = 1000)
        {
            List<Platform> platforms = new List<Platform>();
            for (int row = 0; row < instance.rows.Length; row++)
            {
                for (int p = 0; p < instance.rows[row].platforms.Count; p++)
                {
                    Platform _platform = instance.rows[row].platforms[p];

                    if (!IsPointIngameZone(_platform.transform.position) || (ignoreSlippery && _platform.type.isSlippery) ||
                        (_platform.ball != null && _platform.ball.colorName == "Bomb") || (ignoreBallsEmpty && _platform.ball == null) ||
                       _platform == TigerPlayer.instance.currentPlatform) continue;

                    float currentDistance = Vector3.Distance(pos, _platform.transform.position);
                    if (currentDistance <= range)
                    {
                        platforms.Add(_platform);
                    }
                }
            }
            return platforms.ToArray();
        }
        public static Platform FindRandomPlatformAtRange(Vector3 pos, bool ignoreSlippery = false, bool ignoreBallsEmpty = false, float range = 1000)
        {
            Platform[] platforms = FindPlatformsAtRange(pos, ignoreSlippery, ignoreBallsEmpty, range);
            if (platforms.Length == 0) return null;
            return platforms[Random.Range(0, platforms.Length)];
        }
        public static void SpeedUpRow(int row)
        {
            instance.rows[row].speed = instance.platformsSpeedUpMultiplier * (instance.rows[row].speed > 0 ? 1 : -1);
        }
        public static void NormalizeRowSpeed(int row)
        {
            instance.rows[row].speed = instance.rows[row].speed > 0 ? 1 : -1;
        }
        public static void ChangeRowDirection(int row)
        {
            instance.rows[row].speed = instance.rows[row].speed > 0 ? -1 : 1;
        }

        public OnPlatformCreatedAction onPlatformCreated;
        public delegate void OnPlatformCreatedAction(Platform platform, bool isBombPlatform);

        private void Start()
        {
            instance = this;
            InitializeRows();
        }

        public void CreatePlatformRandom(int row)
        {
            int platformIndex = platformsChanceList[Random.Range(0, platformsChanceList.Count)];
            if (platformIndex == -1)
            {
                if (rows[row].lastPlatformGap)
                {
                    platformIndex = 0;
                    rows[row].lastPlatformGap = false;
                }
                else
                {
                    rows[row].lastPlatformGap = true;
                    return;
                }
            }
            PlatformAttribute platformAttribute = LevelsManager.currentLevel.levelPlatforms[platformIndex].platform;
            GameObject newPlatformObject = Instantiate(platformAttribute.prefab.gameObject,
                new Vector3(platformSpawnDistance * (rows[row].speed > 0 ? -1 : 1) + Random.Range(-0.1f, 0.1f), 0, rowSize * row + Random.Range(-0.1f, 0.1f)),
                Quaternion.identity);
            newPlatformObject.transform.SetParent(transform);

            Platform newPlatform = newPlatformObject.GetComponent<Platform>();
            newPlatform.type = platformAttribute;
            newPlatformObject.name = platformAttribute.name;

            newPlatform.row = row;

            rows[row].platforms.Add(newPlatform);

            if (onPlatformCreated != null) onPlatformCreated(newPlatform, LevelsManager.currentLevel.bombChance > 0 ? (Random.Range(0, 100) < LevelsManager.currentLevel.bombChance) : false);
        }
        public void DestroyPlatform(int row, Platform target)
        {
            rows[row].platforms.Remove(target);
            Destroy(target.gameObject);
        }
        public void DestroyRow(int row)
        {
            for (int p = 0; p < rows[row].platforms.Count; p++)
            {
                Platform _platform = rows[row].platforms[p];
                rows[row].platforms.Remove(_platform);
                Destroy(_platform.gameObject);
            }
            rows[row] = new PlatformRow();
        }
        public void DestroyRows(int row)
        {
            for (int r = 0; r < rows.Length; r++)
            {
                DestroyRow(r);
            }
        }
        private void FixedUpdate()
        {
            if (LevelsManager.currentLevel.levelPlatforms.Length == 0) return;

            for (int r = 0; r < rows.Length; r++)
            {
                for (int p = 0; p < rows[r].platforms.Count; p++)
                {
                    Platform _platform = rows[r].platforms[p];
                    bool movePlatform = true;
                    if (_platform.type.isFragile && _platform.TryGetComponent<FragilePlatform>(out FragilePlatform fragilePlatform))
                    {
                        if (fragilePlatform.lifeTime > 0)
                        {
                            fragilePlatform.lifeTime -= Time.deltaTime * LevelsManager.currentLevel.platformsSpeedMultiplier;
                            if (fragilePlatform.lifeTime < 0)
                            {
                                fragilePlatform.lifeTime = 0;
                                fragilePlatform.Fall();
                            }

                            if (fragilePlatform.isFalling)
                            {
                                movePlatform = false;
                            }
                        }
                    }
                    if (movePlatform) MovePlatform(_platform, rows[r].speed);

                    if (_platform.transform.position.x * (rows[r].speed > 0 ? 1 : -1) > platformSpawnDistance)
                    {
                        DestroyPlatform(r, _platform);
                    }
                }

                if (!gameFrozen)
                {
                    rows[r].time += Time.deltaTime * Mathf.Abs(rows[r].speed);

                    if (rows[r].time >= LevelsManager.currentLevel.platformSpawnRate / LevelsManager.currentLevel.platformsSpeedMultiplier)
                    {
                        CreatePlatformRandom(r);
                        rows[r].time = 0;
                    }
                }
            }
        }
        void MovePlatform(Platform _platform, float speed)
        {
            _platform.transform.position += speed * LevelsManager.currentLevel.platformsSpeedMultiplier * Vector3.right * Time.deltaTime;
        }
#if UNITY_EDITOR
        [Space]
        [Header("Gizmos")]
        [Space]
        [Range(0, 1)] public float gizmosFade = 1;
        public int drawRowsCount = 5;
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, gizmosFade);
            for (int r = 0; r < drawRowsCount; r++)
            {
                float z = r * rowSize;
                Gizmos.DrawLine(new Vector3(-platformSpawnDistance, 0, z), new Vector3(platformSpawnDistance, 0, z));
            } 
        }
    }
#endif
    #endregion
}