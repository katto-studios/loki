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
}
