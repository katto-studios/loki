using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct MainMenuItem
{
    public GameObject gameObject;
    public int sceneIndex;
}

public class MainMenuScreen : MonoBehaviour
{
    [SerializeField]
    private List<MainMenuItem> mainMenuItems;
    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
        foreach(MainMenuItem item in mainMenuItems)
        {
            item.gameObject.SetActive(false);
        }
        mainMenuItems[0].gameObject.SetActive(true);
    }

    public void ChangeItem(int index)
    {
        ButtonChime.Instance.PlayChime();
        mainMenuItems[currentIndex].gameObject.SetActive(false);
        currentIndex = index;
        mainMenuItems[currentIndex].gameObject.SetActive(true);
    }

    public void ChangeUp()
    {
        int next = currentIndex + 1;
        if(next >= mainMenuItems.Count)
        {
            next = 0;
        }
        ChangeItem(next);
    }

    public void ChangeDown()
    {
        int next = currentIndex - 1;
        if (next < 0)
        {
            next = mainMenuItems.Count - 1;
        }
        ChangeItem(next);
    }

    public void ChangeScene()
    {
        FindObjectOfType<SceneChanger>().GetComponent<SceneChanger>().ChangeScene(mainMenuItems[currentIndex].sceneIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeDown();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeUp();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeScene();
        }
    }
}
