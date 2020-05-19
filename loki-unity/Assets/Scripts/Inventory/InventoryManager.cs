using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;

public class InventoryManager : Singleton<InventoryManager>
{
    public GameObject inventoryContent;
    public GameObject cpInventoryContent;
    public GameObject inventorySlotPrefab;
    public GameObject cpInventorySlotPrefab;
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public List<CPInventorySlot> cpInventorySlots = new List<CPInventorySlot>();

    public CPInventorySlot currentCPSlot;

    public void InitInventory()
    {
        PlayerInventoryCallBack pcib = GetInventoryData;
        PlayFabPlayerData.GetUserInventory(PlayfabUserInfo.AccountInfo, pcib);
        PlayFabKeyboard.UpdatedCallback += UpdateCustomData;
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

            if (eachItem.ItemClass.Equals("COLOURPACK"))
            {
                GameObject newInventorySlot = Instantiate(cpInventorySlotPrefab, cpInventoryContent.transform);

                string id = eachItem.ItemId;
                ColourPack cp = ColourPackDatabase.Instance.GetColourPackFromID(id);

                string equipIndex = "-2";
                try { eachItem.CustomData.TryGetValue("EQUIP_SLOT", out equipIndex); }
                catch { };
                int ei = int.Parse(equipIndex);

                if(ei > 0)
                {
                    currentCPSlot = newInventorySlot.GetComponent<CPInventorySlot>();
                }

                newInventorySlot.GetComponent<CPInventorySlot>().SetColourPack(cp, ei);
                cpInventorySlots.Add(newInventorySlot.GetComponent<CPInventorySlot>());
            }
        }

        inventoryContent.SetActive(false);
        cpInventoryContent.SetActive(true);
    }

    public void ClickButton(int i)
    {
        if(i == 0)
        {
            inventoryContent.SetActive(true);
            cpInventoryContent.SetActive(false);
        } else if (i==1)
        {
            inventoryContent.SetActive(false);
            cpInventoryContent.SetActive(true);
        }
    }

    public void OnInventorySlotClicked(InventorySlot slot)
    {
        if(EditorManager.Instance.state == EditorManagerState.IDLE)
        {
            EditorManager.Instance.ChangeSelectedArtisan(slot);
        }
    }

    public void OnInventorySlotClicked(CPInventorySlot slot)
    {
        if(currentCPSlot != slot)
        {
            PlayFabKeyboard.UpdateCustomData(slot.GetColourPack().id, 1);
            PlayFabKeyboard.UpdateCustomData(currentCPSlot.GetColourPack().id, 0);
        }
    }

    public void UpdateCustomData(string instanceID, int data)
    {
        foreach(CPInventorySlot slot in cpInventorySlots)
        {
            if(slot.GetColourPack().id == instanceID)
            {
                if(data == 0)
                {
                    slot.SetInventoryState();
                } else if (data == 1)
                {
                    slot.SetEquipedState();
                    currentCPSlot = slot;
                    MyKeyboard.Instance.SetColour(slot.GetColourPack());
                }
            }
        }
    }
}
