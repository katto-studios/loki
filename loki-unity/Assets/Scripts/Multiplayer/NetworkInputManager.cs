using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInputManager : TypeInputManager {
    private void Start() {
        typeGameManager = NetworkGameManager.Instance;
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }
}
