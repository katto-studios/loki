using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour {
    public TextMeshProUGUI userName;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(UpdateName());
    }

    private IEnumerator UpdateName() {
        while (true) {
            userName.text = PlayfabUserInfo.GetUsername();
            if (!userName.text.Equals("")) {
                break;
            }
            yield return null;
        }
    }
}
