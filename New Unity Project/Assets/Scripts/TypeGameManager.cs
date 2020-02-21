using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeGameManager : Singleton<TypeGameManager>
{

    public string wordsString;
    string inputString = "";
    string inputWord = "";
    public TextMeshProUGUI inputTextMesh;
    public List<TRWord> words;
    public int wordIndex;

    private void Start()
    {
        ConvertStringToTRWords(wordsString);
    }

    void ConvertStringToTRWords(string s)
    {
        char[] ncwords = s.ToCharArray();
        List<char> nextTRWord = new List<char>();
        for(int i = 0; i < ncwords.Length; i++)
        {
            nextTRWord.Add(ncwords[i]);
            if(ncwords[i].Equals(' ') || i == ncwords.Length - 1) //Make new TRWord when theres a space or is last char
            {
                TRWord nextTRWordSO = ScriptableObject.CreateInstance<TRWord>();
                nextTRWordSO.word = nextTRWord.ToArray();
                words.Add(nextTRWordSO);
                nextTRWord.Clear();
            }
        }
    }

    void UpdateTextMesh()
    {
        if (words[wordIndex].CompareWords(inputWord.ToCharArray()))
        {
            inputTextMesh.color = new Color(1, 1, 1);
        }
        else
        {
            inputTextMesh.color = new Color(1, 0.5f, 0.5f);
        }
        inputTextMesh.text = inputWord;
    }

    public void AddCharacterToInputString(char character)
    {
        //Update input strings
        inputString += character;
        inputWord += character;

        Debug.Log(words[wordIndex]);

        //Check to move on to the next word
        if(character == ' ' && words[wordIndex].CompareWords(inputWord.ToCharArray()))
        {
            NextWord();
        }
        
        //Update the textMesh
        UpdateTextMesh();
    }

    void NextWord()
    {
        inputWord = "";
        wordIndex++;

        if (wordIndex.Equals(words.Count))
        {
            Complete();
        }
    }

    void Complete()
    {
        
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
