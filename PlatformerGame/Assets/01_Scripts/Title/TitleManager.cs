using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TitleManager : Singleton<TitleManager>
{
    protected override void OnAwake()
    {
        //Load and Apply Setting Data
    }

    public void OnPressStartButton()
    {
        //Load or Create Data
        //Load Game Scene
    }

    public void OnPressOptionsButton()
    {
        //Open Option Window
    }
    
    public void OnPressQuitButton()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
