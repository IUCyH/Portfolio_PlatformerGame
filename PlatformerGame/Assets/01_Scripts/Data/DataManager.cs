using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

public class DataManager : Singleton_DontDestroy<DataManager>
{
    PlayerData playerData;
    DatabaseReference dbReference;

    protected override void OnAwake()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        Load();
    }

    public void Save()
    {
        var jsonData = JsonUtility.ToJson(playerData);

        dbReference.Child(playerData.userID).SetRawJsonValueAsync(jsonData);
    }

    public void Load()
    {
        if (playerData == null)
        {
            playerData = new PlayerData { userID = "0001" };
            Save();
            return;
        }
        
        dbReference.Child(playerData.userID).GetValueAsync().ContinueWith(task =>
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
                var snapshot = task.Result;
                playerData.SetData(snapshot);
            }
        });
    }
}
