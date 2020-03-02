using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public GameObject keysGO;
    public bool usePlaceholders;
    public GameObject placeHolderKeycap;
    public List<GameObject> keys;

    KeyCode[] keyCodes =
    {
        KeyCode.Escape,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Alpha0,
        KeyCode.Minus,
        KeyCode.Plus,
        KeyCode.Backspace,
        KeyCode.Tab,
        KeyCode.Q,
        KeyCode.W,
        KeyCode.E,
        KeyCode.R,
        KeyCode.T,
        KeyCode.Y,
        KeyCode.U,
        KeyCode.I,
        KeyCode.O,
        KeyCode.P,
        KeyCode.LeftCurlyBracket,
        KeyCode.RightCurlyBracket,
        KeyCode.Backslash,
        KeyCode.CapsLock,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.F,
        KeyCode.G,
        KeyCode.H,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L,
        KeyCode.Colon,
        KeyCode.BackQuote,
        KeyCode.Return,
        KeyCode.LeftShift,
        KeyCode.Z,
        KeyCode.X,
        KeyCode.C,
        KeyCode.V,
        KeyCode.B,
        KeyCode.N,
        KeyCode.M,
        KeyCode.Comma,
        KeyCode.Period,
        KeyCode.Question,
        KeyCode.RightShift,
        KeyCode.LeftControl,
        KeyCode.LeftWindows,
        KeyCode.LeftAlt,
        KeyCode.Space,
        KeyCode.RightAlt,
        KeyCode.Joystick7Button10,
        KeyCode.Joystick7Button10,
        KeyCode.RightControl
    };

    // Start is called before the first frame update
    void Start()
    {
        InitKeyboard();
    }

    void InitKeyboard()
    {
        foreach(Transform child in keysGO.GetComponentInChildren<Transform>())
        {
            keys.Add(child.gameObject);
        }

        if (usePlaceholders)
        {
            foreach (GameObject key in keys)
            {
                Instantiate(placeHolderKeycap, key.transform);
            }
        }
    }

    public void KeyboardKeyDown(KeyCode keyCode)
    {
        for(int i = 0; i < keyCodes.Length; i++)
        {
            if(keyCode == keyCodes[i])
            {
                KeyboardKeyDown(i);
            }
        }
    }

    public void KeyboardKeyDown(int i)
    {
        Transform keyTransform = keys[i].transform.GetChild(0);
        keyTransform.localPosition = new Vector3(0, -0.2f, 0);
    }

    public void KeyboardKeyUp(KeyCode keyCode)
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (keyCode == keyCodes[i])
            {
                KeyboardKeyUp(i);
            }
        }
    }

    public void KeyboardKeyUp(int i)
    {
        Transform keyTransform = keys[i].transform.GetChild(0);
        keyTransform.localPosition = new Vector3(0, 0, 0);
    }
}
