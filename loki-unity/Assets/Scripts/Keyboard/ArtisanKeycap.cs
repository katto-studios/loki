using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeycapRarity {
    COMMON,
    RARE,
    EPIC,
    LEGENDARY,
    UNIQUE
};

[CreateAssetMenu]
public class ArtisanKeycap : ScriptableObject
{
    public string keycapName;
    public string id;
    public GameObject keycap;
    public KeycapRarity rarity;
}
