using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeGameManager : MonoBehaviour
{

    string inputString = "";
    string inputWord = "";
    public TextMeshProUGUI inputTextMesh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateTextMesh()
    {
        inputTextMesh.text = inputWord;
    }

    public void AddCharacterToInputString(char character)
    {
        inputString += character;
        inputWord += character;

        if(character == ' ')
        {
            inputWord = "";
        }

        UpdateTextMesh();
    }

    public void BackSpacePressed()
    {
        if(inputWord.Length != 0)
        {
            inputString = inputString.Substring(0, inputString.Length - 1);
            inputWord = inputWord.Substring(0, inputWord.Length - 1);
        }

        UpdateTextMesh();
    }
}
