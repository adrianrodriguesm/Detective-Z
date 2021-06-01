using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraController2D : MonoBehaviour
{
    public Transform target;
    PixelPerfectCamera m_PixelPerfectCamera;
    void Start()
    {
        m_PixelPerfectCamera = GetComponent<PixelPerfectCamera>();
        m_PixelPerfectCamera.assetsPPU = 16;

    }

    // Update is called once per frame
    void Update()
    {
            
        Vector3 newPosition = target.position;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
