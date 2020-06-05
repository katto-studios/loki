using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles the input
public class TimeTrialInputHandler : Singleton<TimeTrialInputHandler>{
    public event Action<char> eOnKeyDown;

    private void Update(){
        foreach (char ch in Input.inputString){
            eOnKeyDown?.Invoke(ch);
        }
    }
}