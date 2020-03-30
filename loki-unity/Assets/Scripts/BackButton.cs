using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public void GoBackToMainMenu()
    {
        GoToScene(1);
    }

    public void GoToScene(int i)
    {
        PersistantCanvas.Instance.ChangeScene(1);
    }
}
