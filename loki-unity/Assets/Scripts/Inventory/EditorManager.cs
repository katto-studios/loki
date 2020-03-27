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
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.GetComponent<EditorKey>())
                {
                    KeySlot ks = hit.collider.GetComponent<EditorKey>().keySlot;
                    ChangeKey(ks, selectedSlot);
                }
            }
        }
    }

    public void ChangeKey(KeySlot ks, InventorySlot invSlot)
    {

    }
}
