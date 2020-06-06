using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//use this to get a random word
public class TimeTrialWordFactory : MonoBehaviour{
    private int m_index = 0;
    private void Start(){
        GetWords.Initalise(0);
    }

    public string GetWord(){
        return GetWords.GetWord().TrimStart().TrimEnd();
        //return "XD";
    }
}
