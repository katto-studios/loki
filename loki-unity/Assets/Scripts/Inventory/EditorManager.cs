using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : Singleton<EditorManager>
{
    public GameObject collisionBoundsPrefab;
    public Keyboard keyboard;
    public List<EditorKey> editorKeys = new List<EditorKey>();
    // Start is called before the first frame update
    public void Init()
    {
        Debug.Log("EditorManager Inited");
        if (!keyboard) keyboard = Keyboard.Instance;

        foreach(KeySlot ks in keyboard.keySlots)
        {
            GameObject newColBound = Instantiate(collisionBoundsPrefab, ks.gameObject.transform);
            newColBound.GetComponent<EditorKey>().keySlot = ks;
            editorKeys.Add(newColBound.GetComponent<EditorKey>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
