using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.AdminModels;
using UnityEngine.Networking;

public class GetProse : Singleton<GetProse> {
    private List<Paragraph> m_prosesAvaliable = new List<Paragraph>();

    public void CheckForUpdate() {
        //bool hasUpdate = false;
        //Paragraph[] whatWeHave = Resources.LoadAll<Paragraph>("Words");
        List<string> prosesToLoad = new List<string>();

        DontDestroyOnLoad(gameObject);

        GetContentListRequest listReq = new GetContentListRequest();
        PlayFabAdminAPI.GetContentList(
            listReq,
            (_result) => {
                foreach (ContentInfo content in _result.Contents) {
                    GetContentDownloadUrlRequest dlReq = new GetContentDownloadUrlRequest();
                    dlReq.Key = content.Key;
                    PlayFabClientAPI.GetContentDownloadUrl(
                        dlReq,
                        (__result) => { StartCoroutine(GetRequest(__result.URL)); },
                        (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
                    );
                }
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    public Paragraph GetRandomProse() {
        return m_prosesAvaliable[Random.Range(0, m_prosesAvaliable.Count - 1)];
    }

    private IEnumerator GetRequest(string _url) {
        using (UnityWebRequest webReq = UnityWebRequest.Get(_url)) {
            yield return webReq.SendWebRequest();
            if (webReq.isNetworkError) {
                Debug.LogError("Network error: " + webReq.error);
            } else {
                //Debug.Log("Data gotten: " + webReq.downloadHandler.text);
                string[] data = webReq.downloadHandler.text.Split(':');
                string paraProse = data[1];
                string paraAuthor = data[2];
                string paraSrc = data[3];
                m_prosesAvaliable.Add(new Paragraph(paraProse, paraAuthor, paraSrc));
            }
        }
    }
}
