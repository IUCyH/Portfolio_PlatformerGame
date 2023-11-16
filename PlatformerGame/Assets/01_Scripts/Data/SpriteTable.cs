using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum KindOfAssetBundle
{
    Player,
    UI
}

public class SpriteTable : Singleton_DontDestroy<SpriteTable>
{
    Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    [SerializeField]
    List<AssetBundle> assetBundles = new List<AssetBundle>();

    protected override void OnAwake()
    {
        LoadAssetBundles();
    }

    void LoadAssetBundles()
    {
        var path = Path.Combine(Application.persistentDataPath, "AssetBundles_Platformer");
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo[] files = directoryInfo.GetFiles();

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.Contains("manifest") || files[i].Name.Contains("txt")) continue;

            var bundle = AssetBundle.LoadFromFile(files[i].FullName);
            if (!string.IsNullOrEmpty(bundle.name))
            {
                assetBundles.Add(bundle);
            }
        }
    }

    public Sprite GetSprite(KindOfAssetBundle kindOfAssetBundle, string spriteName)
    {
        if (sprites.ContainsKey(spriteName)) return sprites[spriteName];

        var spriteArr = assetBundles[(int)kindOfAssetBundle].LoadAllAssets<Sprite>();

        for (int i = 0; i < spriteArr.Length; i++)
        {
            sprites.Add(spriteArr[i].name, spriteArr[i]);
        }
        
        return sprites[spriteName];
    }
}
