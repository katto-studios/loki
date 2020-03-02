using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public GameObject keysGO;
    public bool usePlaceholders;
    public GameObject placeHolderKeycap;
    List<GameObject> keys;
    // Start is called before the first frame update
    void Start()
    {
        InitKeyboard();
    }

    void InitKeyboard()
    {
        foreach(Transform child in keysGO.GetComponentInChildren<Transform>())
        {
            keys.Add(child.gameObject);
        }

        if (usePlaceholders)
        {
            foreach (GameObject key in keys)
            {
                Instantiate(placeHolderKeycap, key.transform);
            }
        }
    }

    void KeyboardKeyDown(KeyCode keyCode)
    {

    }

    void KeyboardKeyUp(KeyCode keyCode)
    {

    }
}
