using UnityEditor;
using UnityEngine;

class ReconnectToPrefab : ScriptableWizard
{
    public GameObject prefab;

    [MenuItem("Custom/Reconnect to prefab")]
    static void Init()
    {
        foreach (GameObject go in Selection.gameObjects)
            EditorUtility.ReconnectToLastPrefab(go);
    }

    void OnWizardUpdate()
    {
        helpString = "When you loose the link to multiple prefabs (Multiselection doesn't work in Unity!)\neg Copy/Pasting objects from playmode to normal";
    }
}

