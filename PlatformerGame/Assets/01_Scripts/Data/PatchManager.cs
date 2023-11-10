using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Firebase.Firestore;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Networking;
using File = UnityEngine.Windows.File;

public class PatchManager : Singleton_DontDestroy<PatchManager>
{
    const string SpriteFolderName = "Sprites";
    const string AssetBundleCacheFolderName = "AssetBundles_Platformer";
    
    FirebaseFirestore db;
    StorageReference storage;
    [SerializeField]
    List<string> assetBundleNames = new List<string>();

    protected override void OnAwake()
    {
        db = FirebaseFirestore.DefaultInstance;
        storage = FirebaseStorage.DefaultInstance.GetReferenceFromUrl(@"gs://platformergame-3ccf7.appspot.com");
        
        Load();
    }

    async void Load()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Application.persistentDataPath, AssetBundleCacheFolderName));

        await GetBundleNames();

        if (directoryInfo.Exists)
        {
            //TODO : Patch
        }
        else
        {
            directoryInfo.Create();
            StartCoroutine(Coroutine_Cache());
        }
    }

    async Task GetBundleNames()
    {
        var snapshot = await db.Collection("AssetBundleNames").GetSnapshotAsync();

        if (snapshot != null)
        {
            foreach (var document in snapshot.Documents)
            {
                var result = document.ToDictionary();

                foreach (var keyValuePair in result)
                {
                    assetBundleNames.Add(keyValuePair.Key);
                }
            }
        }
    }

    IEnumerator Coroutine_Cache()
    {
        var spriteRef = storage.Child(SpriteFolderName);

        for (int i = 0; i < assetBundleNames.Count; i++)
        {
            var fileName = assetBundleNames[i];
            Uri uri = null;

            spriteRef.Child(fileName).GetDownloadUrlAsync().ContinueWith(task =>
            {
                uri = task.Result;
            });

            while (uri == null) yield return null;

            var request = UnityWebRequest.Get(uri);
            var path = Path.Combine(Application.persistentDataPath, AssetBundleCacheFolderName, fileName);

            yield return request.SendWebRequest();
            
            File.WriteAllBytes(path, request.downloadHandler.data);
        }
    }
}
