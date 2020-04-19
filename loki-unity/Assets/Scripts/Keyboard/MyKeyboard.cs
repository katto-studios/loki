using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;

[RequireComponent(typeof(NetworkKeyboard))]
public class MyKeyboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerInventoryCallBack picb = GetInventoryData;
        PlayFabPlayerData.GetUserInventory(PlayfabUserInfo.AccountInfo, picb);
    }

    public void GetInventoryData(List<ItemInstance> items)
    {
        GetComponent<NetworkKeyboard>().Init(items);
    }
}
