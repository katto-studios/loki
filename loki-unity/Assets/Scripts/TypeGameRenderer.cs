using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeGameRenderer : MonoBehaviour
{
    TypeGameManager typeGameManager;
    public TextMeshProUGUI wordTextMesh;
    public GameObject GamePanel;

    private void Start()
    {
        typeGameManager = TypeGameManager.Instance;
        wordTextMesh.text = typeGameManager.wordsString;
    }

    public void UpdateInput()
    {
        string newWordString = "<color=#55FF55>";
        for(int i = 0; i < typeGameManager.words.Count; i++)
        {
            if(i == typeGameManager.wordIndex)
            {
                newWordString += "<color=#FFFFFF>";
            }

            newWordString += typeGameManager.words[i].word.ArrayToString();
        }

        wordTextMesh.text = newWordString;
    }

    public void GameComplete()
    {
        GamePanel.SetActive(false);
    }
}
