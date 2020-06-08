using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

//use this to get a random word
public class TimeTrialWordFactory : MonoBehaviour{
    private int m_index = 0;
    private void Start(){
        GetWords.Initalise(Random.Range(0, int.MaxValue));
    }

    public string GetWord(){
        return GetWords.GetWord().TrimStart().TrimEnd();
        //return "XD";
    }

    public WordLine GetLine(){
        WordLine returnMe = new WordLine();
        for (int count = 0; count < 5; count++){
            returnMe.Line.Add(GetWord());
        }

        return returnMe;
    }
}
