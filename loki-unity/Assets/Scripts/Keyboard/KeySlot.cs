using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySlot : MonoBehaviour
{
    public int keyIndex;
    public KeyCode keyCode;
    public GameObject defaultKeycapGO;
    public GameObject currentKeycapGO;
    public ArtisanKeycap equipedKeycap;

    public void Start()
    {
        defaultKeycapGO = transform.GetChild(0).gameObject;
    }

    public void InitKey(int index, KeyCode code)
    {
        keyIndex = index;
        keyCode = code;
    }

    public void ChangeKey(ArtisanKeycap keycap)
    {
        equipedKeycap = keycap;
        defaultKeycapGO.SetActive(false);
        Destroy(currentKeycapGO);
        currentKeycapGO = Instantiate(keycap.keycap, transform);
    }

    public void EmptySlot()
    {
        equipedKeycap = null;
        defaultKeycapGO.SetActive(true);
        Destroy(currentKeycapGO);
    }
}
