using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PersistantCanvas : Singleton<PersistantCanvas>
{
    public GameObject transitionPanel;
    bool changingScene;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<PersistantCanvas>().Length > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
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

    IEnumerator LoadScene(int sceneIndex)
    {
        transitionPanel.transform.DOMoveY(400, 0.5f);
        yield return new WaitForSeconds(0.5f);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneIndex);

        while (!async.isDone)
        {
            yield return null;
        }

        transitionPanel.transform.DOMoveY(1200, 0.5f);

        changingScene = false;
    }
}
