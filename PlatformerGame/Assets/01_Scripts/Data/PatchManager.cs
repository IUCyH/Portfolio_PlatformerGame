using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Firebase.Firestore;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Networking;

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
            Patch();
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
                    if(keyValuePair.Key == "CRC") continue;
                    
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

        CreateCRCInfoFile();
    }

    void CreateCRCInfoFile()
    {
        var path = Path.Combine(Application.persistentDataPath, AssetBundleCacheFolderName);
        var crcInfoPath = Path.Combine(path, "CRCInfos.txt");
        StreamWriter streamWriter = new StreamWriter(crcInfoPath);
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo[] files = directoryInfo.GetFiles();
        
        for (int i = 0; i < files.Length; i++)
        {
            if(!files[i].Name.Contains("manifest") || files[i].Name.Contains("AssetBundles")) continue;

            using (StreamReader streamReader = new StreamReader(files[i].FullName))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    if(line == null) continue;
                    if (line.Contains("CRC"))
                    {
                        var crc = line.Split(':')[1];
                        crc = crc.Trim();
                        streamWriter.WriteLine($"{files[i].Name.Split('.')[0]} : {crc}");
                    }
                }
            }
        }
        
        streamWriter.Close();
    }

    async void Patch()
    {
        var path = Path.Combine(Application.persistentDataPath, AssetBundleCacheFolderName, "CRCInfos.txt");
        var assetBundleRef = db.Collection("AssetBundleNames");
        
        using (StreamReader streamReader = new StreamReader(path))
        {
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if(line == null) continue;
                var split = line.Split(':');
                split[0] = split[0].Trim();
                split[1] = split[1].Trim();

                var snapshot = await assetBundleRef.Document(split[0]).GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    var dic = snapshot.ToDictionary();
                    foreach (var pair in dic)
                    {
                        if (pair.Key == "CRC" && !split[1].Equals(pair.Value))
                        {
                            StartCoroutine(Coroutine_DownloadNewBundle(split[0]));
                            
                            break;
                        }
                    }
                }
            }
        }
    }

    IEnumerator Coroutine_DownloadNewBundle(string fileName)
    {
        var bundleRef = storage.Child(SpriteFolderName).Child(fileName);
        var manifestRef = storage.Child(SpriteFolderName).Child(fileName + ".manifest");
        Uri uri = null;

        bundleRef.GetDownloadUrlAsync().ContinueWith(task =>
        {
            uri = task.Result;
        });
        while (uri == null) yield return null;

        var request = UnityWebRequest.Get(uri);
        yield return request.SendWebRequest();

        var bundlePath = Path.Combine(Application.persistentDataPath, AssetBundleCacheFolderName, fileName);
        File.Delete(bundlePath);
        File.WriteAllBytes(bundlePath, request.downloadHandler.data);

        uri = null;
        manifestRef.GetDownloadUrlAsync().ContinueWith(task =>
        {
            uri = task.Result;
        });
        while (uri == null) yield return null;

        request = UnityWebRequest.Get(uri);
        yield return request.SendWebRequest();

        var manifestPath = Path.Combine(Application.persistentDataPath, AssetBundleCacheFolderName, fileName + ".manifest");
        File.Delete(manifestPath);
        File.WriteAllBytes(manifestPath, request.downloadHandler.data);

        using (StreamReader streamReader = new StreamReader(manifestPath))
        {
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if (line == null) continue;

                if (line.Contains("CRC"))
                {
                    var crc = line.Split(':')[1];
                    crc = crc.Trim();
                    UpdateCRCInfoFile(fileName, crc);
                    break;
                }
            }
        }
    }

    void UpdateCRCInfoFile(string bundleName, string crc)
    {
        var path = Path.Combine(Application.persistentDataPath, AssetBundleCacheFolderName, "CRCInfos.txt");
        List<string> lines = new List<string>();
        StreamReader streamReader = new StreamReader(path);

        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            if (line == null) continue;
            var split = line.Split(':');
            split[0] = split[0].Trim();
            split[1] = split[1].Trim();
            if (bundleName == split[0])
            {
                lines.Add($"{bundleName} : {crc}");
            }
            else
            {
                lines.Add(line);
            }
        }
        streamReader.Close();

        using (StreamWriter streamWriter = new StreamWriter(path, false))
        {
            for (int i = 0; i < lines.Count; i++)
            {
                streamWriter.WriteLine(lines[i]);
            }
        }
    }
}
