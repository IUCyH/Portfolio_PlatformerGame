using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldPopup : MonoBehaviour, IPopup
{
    [SerializeField]
    TextMeshProUGUI title;
    [SerializeField]
    TextMeshProUGUI content;

    public PopupType Type { get; } = PopupType.InputField;
    public GameObject ThisGameObject { get; private set; }

    void Awake()
    {
        ThisGameObject = gameObject;
    }

    public void SetPopup(string titleText, string contentText)
    {
        title.text = titleText;
        content.text = contentText;
    }
    
    public void OnPressOkButton()
    {
        PopupManager.Instance.ClosePopup(Type);
    }
}
