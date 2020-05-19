using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class CPInventorySlot : MonoBehaviour
{
    private ColourPack colourPack;
    private TextMeshProUGUI itemName;
    [SerializeField]
    private InventorySlotState inventorySlotState;
    private int equipInfo;
    public ItemInstance ItemInstance;

    private Image background;
    private Image itemImage;

    public GameObject equipedPanel;
    public GameObject selectedPanel;

    public void SetColourPack(ColourPack newcp, int newEquipInfo, ItemInstance ie)
    {
        itemName = GetComponentInChildren<TextMeshProUGUI>();
        colourPack = newcp;
        itemName.text = newcp.name;
        background = GetComponent<Image>();
        itemImage = transform.GetChild(1).GetComponent<Image>();

        Color nc = newcp.color;
        nc.a = 1f;
        itemImage.color = nc;

        switch (newcp.colourPackRarity)
        {
            case ColourPackRarity.COMMON:
                background.color = new Color(0.8f, 0.8f, 0.8f);
                break;
            case ColourPackRarity.RARE:
                background.color = new Color(0.6f, 0.6f, 0.8f);
                break;
            case ColourPackRarity.EPIC:
                background.color = new Color(0.7f, 0.5f, 0.7f);
                break;
            case ColourPackRarity.LEGENDARY:
                background.color = new Color(0.8f, 0.6f, 0.4f);
                break;
            case ColourPackRarity.UNIQUE:
                background.color = new Color(0.8f, 0.3f, 0.3f);
                break;
        }

        equipInfo = newEquipInfo;
        if (equipInfo != 0)
        {
            SetEquipedState();
        }
        else
        {
            SetInventoryState();
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
