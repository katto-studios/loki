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
    private KeySlot currentKeySlot;

    public GameObject equipedPanel;
    public GameObject selectedPanel;

    public void SetArtisanKeycap(ArtisanKeycap newKeycap, int newEquipInfo)
    {
        itemName = GetComponentInChildren<TextMeshProUGUI>();   
        keycap = newKeycap;
        itemName.text = keycap.keycapName;

        currentKeySlot = Keyboard.Instance.EquipInfoToKeySlot(newEquipInfo);

        equipInfo = newEquipInfo;
        if(equipInfo < 0)
        {
            SetInventoryState();
        } else
        {
            SetEquipedState();
        }
    }

    public ArtisanKeycap GetKeyCap()
    {
        return keycap;
    }

    // Start is called before the first frame update
    void Start()
    {
        itemName = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetEquipedState()
    {
        selectedPanel.SetActive(false);
        equipedPanel.SetActive(true);
        inventorySlotState = InventorySlotState.EQUIPED;
    }

    public void SetSelectedState()
    {
        selectedPanel.SetActive(true);
        equipedPanel.SetActive(false);
        inventorySlotState = InventorySlotState.SELECTED;
    }

    public void SetInventoryState()
    {
        selectedPanel.SetActive(false);
        equipedPanel.SetActive(false);
        inventorySlotState = InventorySlotState.INVENTORY;
    }

    public void OnClick()
    {
        InventoryManager.Instance.OnInventorySlotClicked(this);
    }
}
