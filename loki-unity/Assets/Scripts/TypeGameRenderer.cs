using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TypeGameRenderer : MonoBehaviour
{
    protected TypeGameManager typeGameManager;
    public TextMeshProUGUI wordTextMesh;
    public GameObject GamePanel;

    public Slider slider;
    public TextMeshProUGUI comboTextMesh;
    public TextMeshProUGUI scoreTextMesh;

    protected virtual void Start()
    {
        typeGameManager = TypeGameManager.Instance;
        wordTextMesh.text = typeGameManager.wordsString;
    }

    protected virtual void Update()
    {
        slider.value = typeGameManager.GetComboTimer();
        comboTextMesh.text = "X" + typeGameManager.combo;
        scoreTextMesh.text = "score: " + typeGameManager.score;
    }

    public void UpdateInput()
    {
        string newWordString = "<color=#BBFFBB>";

        for(int i = 0; i < typeGameManager.words.Count; i++)
        {
            if (i == typeGameManager.wordIndex)
            {
                newWordString += "<color=#FFFFBB>";

                bool canUnderline = true;

                if (typeGameManager.GetInputWord() != ("").ToCharArray())
                {
                    for (int j = 0; j < typeGameManager.words[i].word.Length; j++)
                    {
                        char nextChar = typeGameManager.words[i].word[j];
                        if (j < typeGameManager.GetInputWord().Length)
                        {
                            if (!typeGameManager.GetInputWord()[j].Equals(nextChar) && canUnderline)
                            {
                                canUnderline = false;
                                newWordString += "<color=#FFFFFF>";
                                newWordString += "<u>" + nextChar + "</u>";
                            }
                            else
                            {
                                newWordString += nextChar;
                            }
                        } else
                        {
                            if (canUnderline)
                            {
                                canUnderline = false;
                                newWordString += "<color=#FFFFFF>";
                                newWordString += "<u>" + nextChar + "</u>";
                            }
                            else
                            {
                                newWordString += nextChar;
                            }
                        }
                    }
                } else
                {
                    newWordString += typeGameManager.words[i].word.ArrayToString();
                }
            }
            else
            {
                newWordString += typeGameManager.words[i].word.ArrayToString();
            }
        }

        wordTextMesh.text = newWordString;
    }

    public void GameComplete()
    {
        GamePanel.SetActive(false);
    }
}
