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

    PlayerData playerData;
    FirebaseFirestore db;
    string uuid;

    public PlayerData PlayerData => playerData;

    protected override async void OnAwake()
    {
        db = FirebaseFirestore.DefaultInstance;
        uuid = SystemInfo.deviceUniqueIdentifier;
        
        await Load();
    }

    public void Save()
    {
        var jsonData = JsonUtility.ToJson(playerData);
        var docRef = db.Collection(PlayerDataCollection).Document(uuid);
        var dataDic = JsonToDitionary.Convert(jsonData);
        
        docRef.SetAsync(dataDic).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data Save Completed");
            }
        });
    }

    public async Task Load()
    {
        var userRef = db.Collection(PlayerDataCollection).Document(uuid);
        var snapShot = await userRef.GetSnapshotAsync();

        if (!snapShot.Exists)
        {
            playerData = new PlayerData
            {
                name = "User",
                maxHP = 100f,
                hp = 100f,
                level = 1,
                levelUpProgress = 0f
            };
            
            Save();
            return;
        }

        var json = DictionaryToJson.Convert(snapShot.ToDictionary());
        playerData = JsonUtility.FromJson<PlayerData>(json);
    }
}
