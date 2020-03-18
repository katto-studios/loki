using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    private ArtisanKeycap keycap;
    public TextMeshProUGUI itemName;

    public void SetArtisanKeycap(ArtisanKeycap newKeycap)
    {
        itemName = GetComponentInChildren<TextMeshProUGUI>();   
        keycap = newKeycap;
        itemName.text = keycap.keycapName;
    }

    // Start is called before the first frame update
    void Start()
    {
        itemName = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
