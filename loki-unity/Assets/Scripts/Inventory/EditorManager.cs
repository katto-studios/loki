using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
    public GameObject collisionBoundsPrefab;
    public Keyboard keyboard;
    // Start is called before the first frame update
    void Start()
    {
        if (!keyboard) keyboard = Keyboard.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
