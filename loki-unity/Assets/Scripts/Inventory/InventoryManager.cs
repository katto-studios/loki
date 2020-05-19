using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;

public class InventoryManager : Singleton<InventoryManager>
{
    public GameObject inventoryContent;
    public GameObject inventorySlotPrefab;
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public void InitInventory()
    {
        PlayerInventoryCallBack pcib = GetInventoryData;
        PlayFabPlayerData.GetUserInventory(PlayfabUserInfo.AccountInfo, pcib);
    }

    public void GetInventoryData(List<ItemInstance> items, UserAccountInfo u)
    {
        foreach (ItemInstance eachItem in items)
        {
            if (eachItem.ItemClass.Equals("KEYCAP"))
            {
                GameObject newInventorySlot = Instantiate(inventorySlotPrefab, inventoryContent.transform);

                string id = eachItem.ItemId;
                ArtisanKeycap keycap = KeycapDatabase.Instance.getKeyCapFromID(id);

                string equipIndex = "-2";
                try { eachItem.CustomData.TryGetValue("EQUIP_SLOT", out equipIndex); }
                catch { };
                int ei = int.Parse(equipIndex);

                newInventorySlot.GetComponent<InventorySlot>().SetArtisanKeycap(keycap, ei);
                inventorySlots.Add(newInventorySlot.GetComponent<InventorySlot>());
            }

            if (eachItem.ItemClass.Equals("ColourPack"))
            {

            }
        }
    }

    public void OnInventorySlotClicked(InventorySlot slot)
    {
        if(EditorManager.Instance.state == EditorManagerState.IDLE)
        {
            EditorManager.Instance.ChangeSelectedArtisan(slot);
        }
    }
}
