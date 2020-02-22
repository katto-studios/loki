using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRWord : ScriptableObject
{
    public char[] word;

    public bool CompareWords(char[] cword)
    {

        if(cword.Length > word.Length)
        {
            return false;
        }

        for(int i = 0; i < cword.Length; i++)
        {
            if(!cword[i].Equals(word[i]))
            {
                Debug.Log("Mismatch at " + cword[i] + " " + word[i]);
                return false;
            }
        }
        return true;
    }
}
