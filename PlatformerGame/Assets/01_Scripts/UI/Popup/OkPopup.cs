using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkPopup : MonoBehaviour, IPopup
{
    public PopupType Type { get; } = PopupType.Ok;
    public GameObject ThisGameObject { get; private set; }

    void Awake()
    {
        ThisGameObject = gameObject;
    }

    public void OnPress()
    {
        gameObject.SetActive(false);
    }
}
