using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CPInventorySlot : MonoBehaviour
{
    private ColourPack colourPack;
    private TextMeshProUGUI itemName;
    [SerializeField]
    private InventorySlotState inventorySlotState;
    private int equipInfo;

    public GameObject equipedPanel;
    public GameObject selectedPanel;

    public void SetColourPack(ColourPack newcp, int newEquipInfo)
    {
        itemName = GetComponentInChildren<TextMeshProUGUI>();
        colourPack = newcp;
        itemName.text = newcp.name;

        equipInfo = newEquipInfo;
        if (equipInfo == 0)
        {
            SetInventoryState();
        }
        else
        {
            SetEquipedState();
        }
    }

    public ColourPack GetColourPack()
    {
        return colourPack;
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
        if (inventorySlotState != InventorySlotState.SELECTED)
        {
            InventoryManager.Instance.OnInventorySlotClicked(this);
        }
    }
}
