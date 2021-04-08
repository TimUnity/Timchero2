using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 0.120f;
    public Vector3 offset;

    private void Start()
    {
        offset = new Vector3(0f, 30f, 7f);
    }

    private void FixedUpdate()
    {
        try
        {
            Vector3 fixedPosition = new Vector3(0f, 1f, target.position.z);
            Vector3 destinatedPosition = fixedPosition + offset;
            Vector3 smoothed = Vector3.Lerp(transform.position, destinatedPosition, smooth);
            transform.position = smoothed;
        }
        catch (Exception e)
        {
            return;
        }

        //transform.LookAt(target);
    }
}
