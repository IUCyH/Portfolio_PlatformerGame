using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkPopup : MonoBehaviour, IPopup
{
    public PopupType Type { get; } = PopupType.Ok;

    public void OnPress()
    {
        gameObject.SetActive(false);
    }
}
