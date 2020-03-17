using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RenderPlayerKeycaps()
    {
        List<ArtisanKeycap> playerKeycaps = PlayfabUserInfo.playerKeycaps;
        
        foreach(ArtisanKeycap keycap in playerKeycaps)
        {
            
        }
    }
}
