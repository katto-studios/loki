using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;

[RequireComponent(typeof(NetworkKeyboard))]
public class MyKeyboard : Singleton<MyKeyboard>
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerInventoryCallBack picb = GetInventoryData;
        PlayFabPlayerData.GetUserInventory(PlayfabUserInfo.AccountInfo, picb);
    }

    public void SetColour(ColourPack cp)
    {
        GetComponent<NetworkKeyboard>().SetColourPack(cp);
    }

    public void GetInventoryData(List<ItemInstance> items, UserAccountInfo u)
    {
        GetComponent<NetworkKeyboard>().Init(items);
    }
}
