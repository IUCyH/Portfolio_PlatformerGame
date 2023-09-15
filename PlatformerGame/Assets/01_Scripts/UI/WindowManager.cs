using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : Singleton_DontDestroy<WindowManager>
{
    Stack<GameObject> openWindows = new Stack<GameObject>();

    public void OpenWindow(GameObject window)
    {
        if (openWindows.Contains(window)) return;
        
        openWindows.Push(window);
        window.SetActive(true);
    }

    public GameObject CloseWindow()
    {
        if (openWindows.Count <= 0) return null;
        
        var window = openWindows.Pop();
        window.SetActive(false);

        return window;
    }
}
