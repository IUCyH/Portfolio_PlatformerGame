using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPopup
{
    PopupType Type { get; }

    void SetPopup(string titleText, string contentText);
    void OnPressOkButton();
}
