using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkTRAnalytics : TRAnalytics {
    // Start is called before the first frame update
    protected override void Start() {
        typeGameManager = NetworkGameManager.Instance;
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }
}
