using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NetworkGameRenderer : TypeGameRenderer {
    protected override void Start() {
        typeGameManager = NetworkGameManager.Instance;
    }

    public void Initalise() {
        wordTextMesh.text = typeGameManager.wordsString;
    }

    protected override void Update() {
        base.Update();
    }
}
