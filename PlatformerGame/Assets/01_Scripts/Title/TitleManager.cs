using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TitleManager : Singleton<TitleManager>
{
    public void OnPressStartButton()
    {
        //Load or Create Data
        //Load Game Scene
        DataManager.Instance.Load();
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
