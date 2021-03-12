using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = target.position;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
