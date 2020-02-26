using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(int sceneIndex)
    {

        ButtonChime.Instance.PlayChime(1);
        PersistantCanvas.Instance.ChangeScene(sceneIndex);
    }
}
