using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace RPG.Camera
{
    public enum CameraType
    {
        FREE_LOOK = 0, 
        AIM = 1, 
        DIALOGUE = 2 
    }

    [System.Serializable]
    public struct CameraData
    {
        public CinemachineVirtualCamera camera;
        public CameraType cameraType;
    }
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance { get; private set; }
        
        [SerializeField] List<CameraData> cameras;
        [Range(0, 10), SerializeField] private int minPriority, maxPriority;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetHigherPriority(CameraType cameraType)
        {
            foreach (CameraData camData in cameras)
            {
                if (camData.cameraType == cameraType)
                    camData.camera.Priority = maxPriority;
                else 
                    camData.camera.Priority = minPriority;
            }
        }
    }
}
