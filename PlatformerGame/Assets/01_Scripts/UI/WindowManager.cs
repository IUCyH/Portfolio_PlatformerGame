using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : Singleton_DontDestroy<WindowManager>
{
    Stack<IWindow> openWindows = new Stack<IWindow>();

    public void OpenAndPushIntoStack(IWindow window)
    {
        if (openWindows.Contains(window)) return;
        
        openWindows.Push(window);
        window.Open();
    }

    public IWindow CloseAndPopFromStack()
    {
        if (openWindows.Count <= 0) return null;
        
        var window = openWindows.Pop();
        window.Close();

        return window;
    }
}
