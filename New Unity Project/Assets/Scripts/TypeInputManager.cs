using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeInputManager : MonoBehaviour
{
    public TypeGameManager typeGameManager;

    // Update is called once per frame
    void Update()
    {
        LetterCheck();
    }

    void LetterCheck()
    {
        foreach (char c in Input.inputString)
        {
            if (c != "\b"[0]) //Handle Backspace;
            {
                typeGameManager.BackSpacePressed();
            }
            else
            {
                Debug.Log(c);
                typeGameManager.AddCharacterToInputString(c);
            }
        }
    }
}
