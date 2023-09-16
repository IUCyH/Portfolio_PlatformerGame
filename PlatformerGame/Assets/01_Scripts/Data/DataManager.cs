using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

public class DataManager : Singleton_DontDestroy<DataManager>
{
    PlayerData playerData;
    DatabaseReference dbReference;
    string uuid;

    protected override void OnAwake()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        playerData = new PlayerData();
        uuid = SystemInfo.deviceUniqueIdentifier;
    }

    public void Save()
    {
        var jsonData = JsonUtility.ToJson(playerData);

        dbReference.Child(uuid).SetRawJsonValueAsync(jsonData);
    }

    public void Load()
    {
        dbReference.Child(uuid).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Data Load is Canceled");
            }
            else if (task.IsFaulted)
            {
                Debug.Log("Data Load is Faulted");
            }
            else
            {
                Debug.Log("Data Load was success");
                var snapshot = task.Result;
                playerData.SetData(snapshot);
            }
        });
    }
}
