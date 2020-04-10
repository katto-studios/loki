using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(GachaMenuHandler))]
public class GachaMenuEditor : Editor {
    private GachaMenuHandler m_handler;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        m_handler = (GachaMenuHandler)target;

        if (GUILayout.Button("Gacha")) {
            m_handler.Gacha();
        }
    }
}
