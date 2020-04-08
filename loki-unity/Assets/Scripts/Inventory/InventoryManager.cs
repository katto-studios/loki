using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public GameObject inventoryContent;
    public GameObject inventorySlotPrefab;
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    // Start is called before the first frame update
    void Start()
    {
        //InitInventory();
    }

    public void InitInventory()
    {
        List<ArtisanKeycap> playerKeycaps = PlayfabUserInfo.playerKeycaps;
        
        foreach(ArtisanKeycap keycap in playerKeycaps)
        {
            GameObject newInventorySlot = Instantiate(inventorySlotPrefab, inventoryContent.transform);
            ArtisanData ad = new ArtisanData(-1, "");
            try
            {
                PlayfabUserInfo.artisanData.TryGetValue(keycap, out ad);
            } catch {
                PopupManager.Instance.ShowPopUp("Error getting keycap data");
            };

            int ei = ad.equipInfo;
            newInventorySlot.GetComponent<InventorySlot>().SetArtisanKeycap(keycap, ei);
            inventorySlots.Add(newInventorySlot.GetComponent<InventorySlot>());
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
