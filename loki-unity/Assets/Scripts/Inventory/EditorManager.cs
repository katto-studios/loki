using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EditorManagerState
{
    IDLE,
    SELECTED
}

public class EditorManager : Singleton<EditorManager>
{
    public GameObject collisionBoundsPrefab;
    public NetworkKeyboard keyboard;
    public List<EditorKey> editorKeys = new List<EditorKey>();
    public EditorManagerState state = EditorManagerState.IDLE;
    public InventorySlot selectedSlot;
    private Camera camera;

    // Start is called before the first frame update
    public void Init()
    {
        camera = FindObjectsOfType<Camera>()[0];
        Debug.Log("EditorManager Inited");
        state = EditorManagerState.IDLE;
        if (!keyboard) keyboard = FindObjectOfType<NetworkKeyboard>();

        foreach(KeySlot ks in keyboard.keySlots)
        {
            GameObject newColBound = Instantiate(collisionBoundsPrefab, ks.gameObject.transform);
            newColBound.GetComponent<EditorKey>().keySlot = ks;
            editorKeys.Add(newColBound.GetComponent<EditorKey>());
        }

        InventoryManager.Instance.InitInventory();
    }

    public void ChangeSelectedArtisan(InventorySlot slot)
    {
        state = EditorManagerState.SELECTED;
        selectedSlot = slot;
        slot.SetSelectedState();
    }

    public void Update()
    {
        if(state == EditorManagerState.SELECTED)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.collider.gameObject.name);
                }

                if (hit.collider.GetComponent<EditorKey>())
                {
                    KeySlot ks = hit.collider.GetComponent<EditorKey>().keySlot;
                    ChangeKeyRequest(ks, selectedSlot);
                    state = EditorManagerState.IDLE;
                }
            }
        } else if (state == EditorManagerState.IDLE)
        {
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.transform.name);
                    if (hit.collider.GetComponent<EditorKey>())
                    {
                        KeySlot ks = hit.collider.GetComponent<EditorKey>().keySlot;
                        if (ks.equipedKeycap)
                        {
                            foreach (InventorySlot eachIS in InventoryManager.Instance.inventorySlots)
                            {
                                if (eachIS.GetCurrentKeySlot() == ks)
                                {
                                    eachIS.SetInventoryState();
                                    ks.EmptySlot();
                                }
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.GetComponent<EditorKey>())
                    {
                        KeySlot ks = hit.collider.GetComponent<EditorKey>().keySlot;
                        if (ks.equipedKeycap)
                        {
                            foreach (InventorySlot eachIS in InventoryManager.Instance.inventorySlots)
                            {
                                if (eachIS.GetCurrentKeySlot() == ks)
                                {
                                    ChangeSelectedArtisan(eachIS);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void ChangeKey(KeySlot ks, InventorySlot invSlot)
    {
        if(invSlot.GetCurrentKeySlot()) invSlot.GetCurrentKeySlot().EmptySlot();
        //Add New Key
        ks.ChangeKey(invSlot.GetKeyCap());
        invSlot.SetEquipedState(ks);
    }

    public void ChangeKeyRequest(KeySlot ks, InventorySlot invSlot)
    {
        foreach (InventorySlot eachIS in InventoryManager.Instance.inventorySlots)
        {
            if (eachIS.GetCurrentKeySlot() == ks)
            {
                eachIS.SetInventoryState();
                PlayFabKeycapEquipInfo(eachIS, -1, null);
            }
        }
        PlayFabKeycapEquipInfo(invSlot, ks.keyIndex, ks);
    }

    void PlayFabKeycapEquipInfo(InventorySlot inv, int data, KeySlot ks)
    {
        ArtisanData ad = new ArtisanData(-1, "");
        try { PlayFabKeyboard.artisanData.TryGetValue(inv.GetKeyCap(), out ad); }
        catch { PopupManager.Instance.ShowPopUp("Error Getting Keycap"); };
        string keycapInstanceId = ad.itemInstanceID;
        PlayfabUserInfo.UpdateKeycapCustomData(keycapInstanceId, data, ks, inv);
        Debug.Log(keycapInstanceId + " " + data);
    }
}
