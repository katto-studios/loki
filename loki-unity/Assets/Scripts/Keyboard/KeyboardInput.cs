using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Sends Input Data to Keyboard Component
public class KeyboardInput : MonoBehaviour
{
    public NetworkKeyboard keyboard;
    // Update is called once per frame
    void Update()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                keyboard.KeyboardKeyDown(kcode);
            }

            if (Input.GetKeyUp(kcode))
            {
                keyboard.KeyboardKeyUp(kcode);
            }
        }
    }
}
