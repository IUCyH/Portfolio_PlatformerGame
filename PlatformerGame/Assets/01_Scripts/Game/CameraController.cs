using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform cam;
    [SerializeField]
    Transform target;
    [SerializeField]
    float moveSpeed;

    void Start()
    {
        cam = transform;
    }

    // Update is called once per frame
    void Update()
    {
        var nextPos = Vector3.Lerp(cam.position, target.position, moveSpeed * Time.deltaTime);
        nextPos.z = cam.position.z;
        
        cam.position = nextPos;
    }
}
