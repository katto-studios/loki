using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycapDatabase : Singleton<KeycapDatabase>
{
    [SerializeField]
    private List<ArtisanKeycap> artisanKeycaps;

    void Start()
    {
        if (FindObjectsOfType<KeycapDatabase>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public ArtisanKeycap getKeyCapFromID(int id)
    {
        foreach(ArtisanKeycap ak in artisanKeycaps)
        {
            if(id == ak.id)
            {
                return ak;
            }
        }
        Debug.Log("Keycap " + id + " does not exist");
        return null; //Error
    }
}
