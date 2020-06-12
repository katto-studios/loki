using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab.ClientModels;
using DG.Tweening;

public class PersistantCanvas : Singleton<PersistantCanvas>
{
    public GameObject transitionPanel;
    bool changingScene;
    bool settingsMenuOpen;
    public GameObject settingsMenu;
    public ConfirmationPanel confirmationPanel;

    public int previousScene;

    public Object testing;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<PersistantCanvas>().Length > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }

        transform.position = new Vector3(0, 0, -1);

        //GachaScene(testing);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsMenu();
        }
    }

    public void ToggleSettingsMenu()
    {
        ButtonChime.Instance.PlayChime();
        if (settingsMenuOpen) //Close
        {
            settingsMenuOpen = false;
            settingsMenu.transform.DOMoveX(-360, 0.5f);
        }
        else //Open
        {
            settingsMenuOpen = true;
            settingsMenu.transform.DOMoveX(360, 0.5f);
        }
    }

    public void ChangeScene(int sceneIndex)
    {
        if (!changingScene)
        {
            changingScene = true;
            //SceneManager.LoadScene(sceneIndex);
            StartCoroutine(LoadScene(sceneIndex));
        }
    }

    public void GachaScene(Object obj)
    {
        StartCoroutine(LoadGachaScene(obj));
    }

    IEnumerator LoadGachaScene(Object obj)
    {
        yield return StartCoroutine(LoadScene(12));
        GachaOpenning.Instance.Init(obj);
    }

    public void ViewProfileScene(string name) {
        PlayerDataCallBack pd = GetPlayerData;
        PlayerNotFoundCallBack pnf = PlayerNotFound;
        PlayFabPlayerData.SetTargetPlayer(name, pd, pnf);
    }

    IEnumerator LoadViewProfileScene(UserAccountInfo u)
    {
        yield return StartCoroutine(LoadScene(9));
        ViewProfileManager.Instance.Init(u);
    }

    public void PlayerNotFound()
    {
        PopupManager.Instance.ShowPopUp("Player not found");
    }

    public void GetPlayerData(UserAccountInfo u)
    {
        Debug.Log("Got " + u.Username);
        changingScene = true;
        StartCoroutine(LoadViewProfileScene(u));
    }


    public void HideScreen() {
        transitionPanel.transform.DOMoveY(400, 0.5f);
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        previousScene = SceneManager.GetActiveScene().buildIndex;
        transitionPanel.transform.DOMoveY(400, 0.5f);
        yield return new WaitForSeconds(0.5f);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneIndex);

        while (!async.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        transitionPanel.transform.DOMoveY(1200, 0.5f);

        changingScene = false;
    }

    public void ConfirmationPanel(string t, ConfirmationPanel.CPCallback confirm, ConfirmationPanel.CPCallback cancel)
    {
        confirmationPanel.gameObject.SetActive(true);
        confirmationPanel.Init(t, confirm, cancel);
    }

    public void ConfirmationPanel(string t, ConfirmationPanel.CPCallback confirm)
    {
        confirmationPanel.gameObject.SetActive(true);
        confirmationPanel.Init(t, confirm);
    }
}
