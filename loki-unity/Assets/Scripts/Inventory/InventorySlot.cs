using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum InventorySlotState
{
    INVENTORY,
    SELECTED,
    EQUIPED
}

public class InventorySlot : MonoBehaviour
{
    private ArtisanKeycap keycap;
    private TextMeshProUGUI itemName;
    [SerializeField]
    private InventorySlotState inventorySlotState;
    private int equipInfo;

    public void SetArtisanKeycap(ArtisanKeycap newKeycap, int newEquipInfo)
    {
        itemName = GetComponentInChildren<TextMeshProUGUI>();   
        keycap = newKeycap;
        itemName.text = keycap.keycapName;

        equipInfo = newEquipInfo;
        if(equipInfo < 0)
        {
            inventorySlotState = InventorySlotState.INVENTORY;
        } else
        {
            inventorySlotState = InventorySlotState.EQUIPED;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        itemName = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetEquipedState()
    {

    }

    public void SetSelectedState()
    {

    }

    public void SetInventoryState()
    {

    }
}
