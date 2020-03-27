using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySlot : MonoBehaviour
{
    public int keyIndex;
    public KeyCode keyCode;

    public void InitKey(int index, KeyCode code)
    {
        keyIndex = index;
        keyCode = code;
    }

    public void ChangeKey(ArtisanKeycap keycap)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Instantiate(keycap.keycap, transform);
        Debug.Log("Inited" + keycap.keycapName);
    }

    public void EmptySlot()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
