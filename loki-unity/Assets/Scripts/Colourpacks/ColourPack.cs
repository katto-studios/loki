using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
