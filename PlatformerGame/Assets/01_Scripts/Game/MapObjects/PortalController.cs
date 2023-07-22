using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField]
    Transform linkedPortal;
    Transform target;
    
    bool targetEnter;

    void Update()
    {
        if (!targetEnter) return;
        
        if(!ReferenceEquals(target, null) && InputManager.GetKeyDown(Key.Interaction))
        {
            target.position = linkedPortal.position;
            
            targetEnter = false;
            target = null;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        targetEnter = true;
        target = other.transform;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        targetEnter = false;
        target = null;
    }
}
