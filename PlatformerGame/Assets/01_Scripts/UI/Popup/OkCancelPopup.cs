using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OkCancelPopup : MonoBehaviour, IPopup
{
    [SerializeField]
    TextMeshProUGUI title;
    [SerializeField]
    TextMeshProUGUI content;

    public PopupType Type { get; } = PopupType.OkCancel;
    public GameObject ThisGameObject { get; private set; }

    void Awake()
    {
        ThisGameObject = gameObject;
    }

    public void SetPopup(string title, string content)
    {
        this.title.text = title;
        this.content.text = content;
    }
    
    public void OnPressOkButton()
    {
        PopupManager.Instance.ClosePopup(Type);
    }
}
