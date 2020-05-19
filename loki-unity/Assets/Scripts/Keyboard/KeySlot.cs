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

    public void ChangeColour(ColourPack cp)
    {
        defaultKeycapGO.GetComponentInChildren<MeshRenderer>().material.color = cp.GetKeyColour(keyIndex);
    }

    public void ChangeKey(ArtisanKeycap keycap)
    {
        defaultKeycapGO = transform.GetChild(0).gameObject;
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

    public void KeyDown()
    {
        Transform keyTransform = defaultKeycapGO.transform;
        if (currentKeycapGO)
        {
            keyTransform = currentKeycapGO.transform;
        }
        keyTransform.localPosition = new Vector3(0, -0.2f, 0);
    }

    public void KeyUp()
    {
        Transform keyTransform = defaultKeycapGO.transform;
        if (currentKeycapGO)
        {
            keyTransform = currentKeycapGO.transform;
        }
        keyTransform.localPosition = new Vector3(0, 0, 0);
    }
}
