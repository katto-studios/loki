using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsTest : MonoBehaviour{
    // Start is called before the first frame update
    void Start(){
        Debug.Log("Testing words...");

        GetWords.eOnInitalised += () => {
            for (int count = 0; count < 10; count++){
                Debug.Log(GetWords.Strings[count]);
            }
        };
        GetWords.Initalise(0);
    }

    // Update is called once per frame
    void Update(){
    }
}