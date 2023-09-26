using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public class DataManager : Singleton_DontDestroy<DataManager>
{
    PlayerData playerData;
    DatabaseReference dbReference;
    DataSnapshot snapshot;
    string uuid;

    bool loaded;

    protected override void OnAwake()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        playerData = new PlayerData();
        uuid = SystemInfo.deviceUniqueIdentifier;
    }
    
    IEnumerator Coroutine_SetData()
    {
        var waitUntilLoaded = new WaitUntil(() => loaded);
        
        yield return waitUntilLoaded;
        
        if (snapshot.Exists)
        {
            playerData.SetData(snapshot);
        }
        else
        {
            PopupManager.Instance.OpenPopup(PopupType.InputField, "Notice", "Type your nick name");
        }

        loaded = false;
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
                snapshot = task.Result;
                loaded = true;
            }
        });
        
        StartCoroutine(Coroutine_SetData());
    }
}
