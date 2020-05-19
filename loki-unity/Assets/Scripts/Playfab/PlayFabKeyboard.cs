using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public struct ArtisanData
{
    public int equipInfo;
    public string itemInstanceID;

    public ArtisanData(int e, string instanceId)
    {
        equipInfo = e;
        itemInstanceID = instanceId;
    }
}

public static class PlayFabKeyboard
{
    public static List<ArtisanKeycap> playerKeycaps = new List<ArtisanKeycap>();
    public static Dictionary<ArtisanKeycap, ArtisanData> artisanData = new Dictionary<ArtisanKeycap, ArtisanData>();
    public static event Action<string, int> UpdatedCallback;
    //INVENTORY STUFF

    static bool recievedKeycaps;
    public static void UpdatePlayerKeycaps()
    {
        playerKeycaps.Clear();
        artisanData.Clear();
        recievedKeycaps = false;
        PersistantCanvas.Instance.StartCoroutine(GetUserInventoryRequest());
    }

    static IEnumerator GetUserInventoryRequest()
    {
        List<ArtisanKeycap> newInventory = GetUserInventory();

        while (!recievedKeycaps)
        {
            yield return null;
        }

        playerKeycaps = newInventory;

        string dText = "Inventory Items: ";
        foreach (ArtisanKeycap keycap in playerKeycaps)
        {
            dText += keycap.name + ", ";
        }

        if (Keyboard.Instance)
        {
            Debug.Log("Initing");
            //Keyboard.Instance.InitKeyboard();
        }

        Debug.Log(dText);
    }

    public static void UpdateCustomData(string instanceID, int data)
    {
        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest()
            {
                FunctionName = "UpdateKeycapInfo",
                FunctionParameter = new { ItemId = instanceID, Data_update = data },
            },
            (_result) => {
                Debug.Log(instanceID + " " + data);
                UpdatedCallback?.Invoke(instanceID, data);
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    public static List<ArtisanKeycap> GetUserInventory()
    {
        List<ArtisanKeycap> newInventory = new List<ArtisanKeycap>();
        PlayFabClientAPI.GetUserInventory(
            new GetUserInventoryRequest(),
            (GetUserInventoryResult result) =>
            {
                foreach (var eachItem in result.Inventory)
                {
                    if (eachItem.ItemClass.Equals("KEYCAP"))
                    {
                        Debug.Log(eachItem.ItemId);
                        string id = eachItem.ItemId;
                        ArtisanKeycap newKey = KeycapDatabase.Instance.getKeyCapFromID(id);
                        newInventory.Add(newKey);

                        string equipIndex = "-2";
                        try { eachItem.CustomData.TryGetValue("EQUIP_SLOT", out equipIndex); }
                        catch { };
                        int ei = int.Parse(equipIndex);
                        Debug.Log(ei);
                        artisanData.Add(newKey, new ArtisanData(ei, eachItem.ItemInstanceId));
                    }
                }

                recievedKeycaps = true;
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
        return newInventory;
    }
}
