using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace TigerAndColoredSpheres
{
    public class GameCamera : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        static GameCamera _instance;
        public static GameCamera instance
        {
            get
            {
                if (_instance == null) _instance = GameObject.Find("Camera Panel").GetComponent<GameCamera>();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public Camera currentCamera;

        public LayerMask platformLayer;
        public UnityAction onSelectPlatform;
        public void OnPointerDown(PointerEventData eventData)
        {
            Ray ray = currentCamera.ScreenPointToRay(eventData.position);
            Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, platformLayer))
            {
                if (hit.collider.transform.parent.TryGetComponent<Platform>(out Platform targetPlatform))
                {
                    bool canJump = TigerPlayer.instance.Jump(targetPlatform);
                    if (canJump && onSelectPlatform != null) onSelectPlatform();
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            //
        }
    }
}