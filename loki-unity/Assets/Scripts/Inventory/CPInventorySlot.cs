using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab.ClientModels;

public class CPInventorySlot : MonoBehaviour
{
    private ColourPack colourPack;
    private TextMeshProUGUI itemName;
    [SerializeField]
    private InventorySlotState inventorySlotState;
    private int equipInfo;
    public ItemInstance ItemInstance;

    public GameObject equipedPanel;
    public GameObject selectedPanel;

    public void SetColourPack(ColourPack newcp, int newEquipInfo, ItemInstance ie)
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

        ItemInstance = ie;

        DamageCheck();
    }

    public void DamageCheck()
    {
        if (!equipedPanel)
        {
            equipedPanel = transform.GetChild(2).gameObject;
        }

        if (!selectedPanel)
        {
            equipedPanel = transform.GetChild(3).gameObject;
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
        DamageCheck();
        selectedPanel.SetActive(false);
        equipedPanel.SetActive(true);
        inventorySlotState = InventorySlotState.EQUIPED;
    }

    public void SetSelectedState()
    {

        DamageCheck();
        selectedPanel.SetActive(true);
        equipedPanel.SetActive(false);
        inventorySlotState = InventorySlotState.SELECTED;
    }

    public void SetInventoryState()
    {

        DamageCheck();
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
