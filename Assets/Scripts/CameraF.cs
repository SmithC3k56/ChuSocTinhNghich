using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraF : MonoBehaviour
{
    public Transform target; 
    public float smoothSpeed = 0.125f; 

    // public Vector3 offset;

    void FixedUpdate()
    {
        if (target != null)
        {
            // Vector3 desiredPosition = target.position + offset;
            Vector3 desiredPosition = new Vector3(x: target.position.x,y: target.position.y,z: transform.position.z) ;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
