using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerBackButton : MonoBehaviour{
    public void ExitGame(){
        PhotonNetwork.LeaveRoom();
        PersistantCanvas.Instance.ChangeScene(1);
    }
}