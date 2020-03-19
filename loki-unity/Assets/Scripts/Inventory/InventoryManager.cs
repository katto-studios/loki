using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public GameObject inventoryContent;
    public GameObject inventorySlotPrefab;
    // Start is called before the first frame update
    void Start()
    {
        RenderInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
    }
}
