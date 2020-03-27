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
    public Keyboard keyboard;
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
        if (!keyboard) keyboard = Keyboard.Instance;

        foreach(KeySlot ks in keyboard.keySlots)
        {
            GameObject newColBound = Instantiate(collisionBoundsPrefab, ks.gameObject.transform);
            newColBound.GetComponent<EditorKey>().keySlot = ks;
            editorKeys.Add(newColBound.GetComponent<EditorKey>());
        }
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
                    ChangeKey(ks, selectedSlot);
                    state = EditorManagerState.IDLE;
                }
            }
        }

        if (state == EditorManagerState.IDLE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.collider.gameObject.name);
                }
            }
        }
    }

    public void ChangeKey(KeySlot ks, InventorySlot invSlot)
    {
        if(invSlot.GetCurrentKeySlot()) invSlot.GetCurrentKeySlot().EmptySlot();
        foreach(InventorySlot eachIS in InventoryManager.Instance.inventorySlots)
        {
            if(eachIS.GetCurrentKeySlot() == ks)
            {
                eachIS.SetInventoryState();
            }
        }
        //Add New Key
        ks.ChangeKey(invSlot.GetKeyCap());
        invSlot.SetEquipedState(ks);

        //Update the KeycapEquipInfo
    }
}
