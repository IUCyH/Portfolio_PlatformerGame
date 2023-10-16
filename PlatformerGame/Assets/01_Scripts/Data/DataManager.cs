using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class DataManager : Singleton_DontDestroy<DataManager>
{
    const string PlayerDataCollection = "PlayerData";

    Dictionary<string, string> dataDic = new Dictionary<string, string>();
    PlayerData playerData;
    FirebaseFirestore db;
    string uuid;
    char[] trimChars = new[] { '"', '{', '}' };
    
    bool loaded;

    protected override void OnAwake()
    {
        db = FirebaseFirestore.DefaultInstance;
        uuid = SystemInfo.deviceUniqueIdentifier;
        
        Load();
    }

    public void Save()
    {
        var jsonData = JsonUtility.ToJson(playerData);
        var docRef = db.Collection(PlayerDataCollection).Document(uuid);
        var result = jsonData.Split(",");

        foreach (var data in result)
        {
            var split = data.Split(":");
            var key = split[0].Trim(trimChars);
            var value = split[1].Trim(trimChars);
            
            dataDic.Add(key, value);
        }
        
        docRef.SetAsync(dataDic).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data Save Completed");
            }
        });
    }

    public async void Load()
    {
        var userRef = db.Collection(PlayerDataCollection).Document(uuid);
        var snapShot = await userRef.GetSnapshotAsync();

        if (!snapShot.Exists)
        {
            playerData = new PlayerData
            {
                attackDamage = 5f,
                hp = 100f,
                level = 1,
                name = "User"
            };
            
            Save();
            return;
        }

        foreach (var data in snapShot.ToDictionary())
        {
            Debug.Log(data.Value);
        }
    }
}
