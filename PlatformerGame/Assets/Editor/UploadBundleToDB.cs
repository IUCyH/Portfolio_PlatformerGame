using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Firestore;
using Firebase.Storage;
using UnityEditor;
using UnityEngine;

public class UploadBundleToDB : EditorWindow
{
    [MenuItem("AssetBundle/Upload To DB")]
    public static void Upload()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        StorageReference storage = FirebaseStorage.DefaultInstance.GetReferenceFromUrl(@"gs://platformergame-3ccf7.appspot.com");
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Application.dataPath, "AssetBundles"));
        Dictionary<string, Dictionary<string, string>> bundleDicList = new Dictionary<string, Dictionary<string, string>>();
        var files = directoryInfo.GetFiles();
        var spriteRef = storage.Child("Sprites");
        
        for (int i = 0; i < files.Length; i++)
        {
            if(files[i].Name.Contains("meta")) continue;
            
            var fileName = files[i].Name;
            var name = files[i].Name;
            
            if (fileName.Contains("."))
            {
                var split = fileName.Split('.');
                name = split[0];
            }

            if (bundleDicList.ContainsKey(name))
            {
                bundleDicList[name].Add(fileName, fileName);
            }
            else
            {
                bundleDicList.Add(name, new Dictionary<string, string>());
                bundleDicList[name].Add(fileName, fileName);
            }
            if (fileName.Contains("manifest"))
            {
                var crc = GetCRC(Path.Combine(Application.dataPath, "AssetBundles", fileName));
                bundleDicList[name].Add("CRC", crc);
            }
            
            db.Collection("AssetBundleNames").Document(name).SetAsync(bundleDicList[name]);

            spriteRef.Child(fileName).PutFileAsync(Path.Combine(Application.dataPath, "AssetBundles", fileName));
        }
    }

    static string GetCRC(string path)
    {
        StreamReader streamReader = new StreamReader(path);
        string crc = null;
        
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            if (line.Contains("CRC"))
            {
                crc = line.Split(':')[1];
                crc = crc.Trim();
            }
        }

        streamReader.Close();
        return crc;
    }
}
