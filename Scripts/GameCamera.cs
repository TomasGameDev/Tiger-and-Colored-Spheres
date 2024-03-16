using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TigerAndColoredSpheres
{
    public class GameCamera : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Camera currentCamera;

        public LayerMask platformLayer;
         
        public void OnPointerDown(PointerEventData eventData)
        {
            
            Ray ray = currentCamera.ScreenPointToRay(eventData.position);
            Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, platformLayer))
            { 
                if (hit.collider.transform.parent.TryGetComponent<Platform>(out Platform targetPlatform))
                {
                    TigerPlayer.instance.Jump(targetPlatform);
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            //
        }
    }
}