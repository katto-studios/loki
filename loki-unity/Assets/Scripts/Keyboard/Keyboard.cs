using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : Singleton<Keyboard>
{
    public GameObject keysGO;
    public List<GameObject> keys;
    public List<KeySlot> keySlots;

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
        KeyCode.Equals,
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

    public KeySlot EquipInfoToKeySlot(int ei)
    {
        return keySlots[ei];
    }

    public void ClearKeyboardData()
    {
        foreach(KeySlot keySlot in keySlots)
        {
            keySlot.EmptySlot();
        }
    }

    public void InitKeyboard()
    {
        ClearKeyboardData();
        int ind = 0;
        foreach(Transform child in keysGO.GetComponentInChildren<Transform>())
        {
            keys.Add(child.gameObject);
            child.GetComponent<KeySlot>().InitKey(ind, keyCodes[ind]);
            keySlots.Add(child.GetComponent<KeySlot>());
            ind++;
        }

        if(PlayFabKeyboard.playerKeycaps != null)
        {
            foreach (ArtisanKeycap keycap in PlayFabKeyboard.playerKeycaps)
            {
                Debug.Log("YES");
                ArtisanData ad = new ArtisanData(-1, "");
                try { PlayFabKeyboard.artisanData.TryGetValue(keycap, out ad); }
                catch { PopupManager.Instance.ShowPopUp("Error Getting Keycap"); };
                int keySlot = ad.equipInfo;
                if (keySlot >= 0)
                {
                    KeySlot thisKs = EquipInfoToKeySlot(keySlot);
                    thisKs.ChangeKey(keycap);
                }
            }
        }

        if (FindObjectOfType<EditorManager>()) EditorManager.Instance.Init();
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
        keySlots[i].KeyDown();
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
        keySlots[i].KeyUp();
    }
}
