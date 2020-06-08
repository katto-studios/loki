using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public bool BackToMainMenu;
    public void GoBackToMainMenu()
    {
        if (BackToMainMenu)
        {
            GoToScene(1);
        }
        else
        {
            GoToScene(PersistantCanvas.Instance.previousScene);
        }
    }

    public void GoToScene(int i)
    {
        PersistantCanvas.Instance.ChangeScene(PersistantCanvas.Instance.previousScene);
    } 

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            PersistantCanvas.Instance.ChangeScene(PersistantCanvas.Instance.previousScene);
        }
    }
}
