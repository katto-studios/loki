using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public GameObject keysGO;
    public GameObject placeHolderKeycap;
    public List<GameObject> keys;
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

        foreach (GameObject key in keys)
        {
            Instantiate(placeHolderKeycap, key.transform);
        }
    }

    void KeyboardKeyDown(KeyCode keyCode)
    {

    }

    void KeyboardKeyUp(KeyCode keyCode)
    {

    }
}
