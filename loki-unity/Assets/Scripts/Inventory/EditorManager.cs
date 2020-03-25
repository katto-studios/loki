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
    // Start is called before the first frame update
    public void Init()
    {
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
}
