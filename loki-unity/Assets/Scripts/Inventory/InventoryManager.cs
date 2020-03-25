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
        RenderInventory();
    }

    public void RenderInventory()
    {
        List<ArtisanKeycap> playerKeycaps = PlayfabUserInfo.playerKeycaps;
        
        foreach(ArtisanKeycap keycap in playerKeycaps)
        {
            GameObject newInventorySlot = Instantiate(inventorySlotPrefab, inventoryContent.transform);
            int ei = -1;
            try
            {
                PlayfabUserInfo.keycapEquipInfo.TryGetValue(keycap, out ei);
            } catch { };
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
