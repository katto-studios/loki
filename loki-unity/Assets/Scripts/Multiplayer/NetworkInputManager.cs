using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInputManager : MonoBehaviour {
    NetworkGameManager typeGameManager;

    private void Start() {
        typeGameManager = NetworkGameManager.Instance;
    }

    // Update is called once per frame
    void Update() {
        LetterCheck();
    }

    void LetterCheck() {
        foreach (char c in Input.inputString) {
            if (c == "\b"[0]) //Handle Backspace;
            {
                typeGameManager.BackSpacePressed();
            } else {
                typeGameManager.AddCharacterToInputString(c);
            }
        }
    }
}
