using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldPopup : MonoBehaviour, IPopup, IWindow
{
    [SerializeField]
    TextMeshProUGUI title;
    [SerializeField]
    TextMeshProUGUI content;

    public PopupType Type { get; } = PopupType.InputField;

    public void SetPopup(string titleText, string contentText)
    {
        title.text = titleText;
        content.text = contentText;
    }
    
    public void OnPressOkButton()
    {
        PopupManager.Instance.ClosePopup(Type);
    }
    
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
