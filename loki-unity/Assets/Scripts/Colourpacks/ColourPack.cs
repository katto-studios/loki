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

public enum KeyGroup
{
    NONE,
    ALPHAS,
    MODIFIERS,
    SPACEBAR,
    TRI,
    NUMBERS
}

[CreateAssetMenu]
public class ColourPack : ScriptableObject
{
    public string name;
    public string id;
    public Color color;
    public ColourPackRarity colourPackRarity;

    public List<int> GetKeyGroup(KeyGroup kg)
    {
        switch (kg)
        {
            case KeyGroup.ALPHAS:
                return new List<int>()
                {
                    15,16,17,18,19,20,21,22,23,24,25,26,27,
                    29,30,31,32,33,34,35,36,37,38,39,
                    42,43,44,45,46,47,48,49,50,51
                };
            case KeyGroup.MODIFIERS:
                return new List<int>()
                {
                    0,13,14,28,40,41,52,53,54,55,57,58,59,60
                };
            case KeyGroup.NUMBERS:
                return new List<int>()
                {
                    1,2,3,4,5,6,7,8,9,10,11,12
                };
            case KeyGroup.SPACEBAR:
                return new List<int>()
                {
                    56
                };
            case KeyGroup.TRI:
                return new List<int>()
                {
                    0,56,40
                };
            case KeyGroup.NONE:
                return null;
        }
        return null;
    }

    [Serializable]
    public struct AdditionalColour
    {
        public Color color;
        public List<int> keys;
        public KeyGroup keyGroup;
    }

    public List<AdditionalColour> additionalColours;

    public Color GetKeyColour(int i)
    {
        foreach(AdditionalColour ac in additionalColours)
        {
            foreach (int k in ac.keys)
            {
                if (k == i)
                {
                    return ac.color;
                }
            }

            foreach (int k in GetKeyGroup(ac.keyGroup))
            {
                if (k == i)
                {
                    return ac.color;
                }
            }
        }
        return color;
    }
}
