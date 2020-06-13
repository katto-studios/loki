using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public bool BackToMainMenu;
    public bool refresh;
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
        PersistantCanvas.Instance.ChangeScene(i);
    } 

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            int scene = 1;
            if (!BackToMainMenu)
            {
                scene = PersistantCanvas.Instance.previousScene;
            }
            PersistantCanvas.Instance.ChangeScene(scene);
        }

        if (Input.GetKey(KeyCode.LeftControl) && refresh)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                PersistantCanvas.Instance.ChangeScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
