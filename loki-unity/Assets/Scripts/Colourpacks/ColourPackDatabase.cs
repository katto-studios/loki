using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourPackDatabase : Singleton<ColourPackDatabase>
{
    [SerializeField]
    private List<ColourPack> colourPacks;

    public ColourPack GetColourPackFromID(string id)
    {
        foreach (ColourPack cp in colourPacks)
        {
            if (id.Equals(cp.id))
            {
                return cp;
            }
        }
        Debug.Log("ColourPack " + id + " does not exist");
        return null;
    }
}
