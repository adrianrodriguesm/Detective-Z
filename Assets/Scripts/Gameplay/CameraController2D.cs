using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraController2D : MonoBehaviour
{
    public Transform target;
    PixelPerfectCamera pixelPerfectCamera;
    void Start()
    {
        pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
        pixelPerfectCamera.assetsPPU = 8;
    }

    // Update is called once per frame
    void Update()
    {
        if(StoryManager.Instance.IsSimulationEnd())
        {
            pixelPerfectCamera.assetsPPU = 16;
            Vector3 newPosition = target.position;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }

    }
}
