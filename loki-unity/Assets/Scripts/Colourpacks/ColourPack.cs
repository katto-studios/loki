using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ColourPackRarity
{
    COMMON,
    RARE,
    EPIC,
    LEGENDARY,
    UNIQUE
}

[CreateAssetMenu]
public class ColourPack : ScriptableObject
{
    public string name;
    public string id;
    public Color color;
    public ColourPackRarity colourPackRarity;

    [Serializable]
    public struct AdditionalColour
    {
        public Color color;
        public List<int> keys;
    }

    public List<AdditionalColour> additionalColours;

    public Color GetKeyColour(int i)
    {
        foreach(AdditionalColour ac in additionalColours)
        {
            foreach(int k in ac.keys)
            {
                if(k == i)
                {
                    return ac.color;
                }
            }
        }
        return color;
    }
}
