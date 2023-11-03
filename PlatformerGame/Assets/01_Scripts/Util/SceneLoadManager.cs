using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LoadScene
{
    Title,
    Game
}

public class SceneLoadManager : Singleton_DontDestroy<SceneLoadManager>
{
    public void Load(LoadScene scene)
    {
        SceneManager.LoadScene((int)scene);
    }
}
