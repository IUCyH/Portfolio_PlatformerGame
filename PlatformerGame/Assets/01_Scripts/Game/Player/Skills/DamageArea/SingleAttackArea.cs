using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Inside of Single Area!");
    }
}
