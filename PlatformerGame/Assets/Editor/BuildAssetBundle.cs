using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAssetBundle : EditorWindow
{
    [MenuItem("AssetBundle/Build AssetBundle")]
    public static void Build()
    {
        BuildPipeline.BuildAssetBundles(Path.Combine(Application.dataPath, "AssetBundles"), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
