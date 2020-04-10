using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaMenuHandler : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Gacha() {
        KeycapRarity gotThisRarity = GetRate(Random.Range(0.0f, 1.0f));
        Debug.Log(gotThisRarity.ToString());
    }

    //I'm following azur lane rates
    private KeycapRarity GetRate(float _input) {
        if (_input <= 0.3f) {
            return KeycapRarity.COMMON;
        } else if (_input <= 0.81f) {
            return KeycapRarity.RARE;
        } else if (_input <= 0.93f) {
            return KeycapRarity.EPIC;
        } else {
            return KeycapRarity.LEGENDARY;
        }
    }
}
