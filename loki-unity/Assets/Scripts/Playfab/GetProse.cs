using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.AdminModels;
using UnityEngine.Networking;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GetProse : Singleton<GetProse> {
    private List<Paragraph> m_prosesAvaliable = new List<Paragraph>();

    public void CheckForUpdate() {
        DontDestroyOnLoad(gameObject);

        HashSet<string> prosesToLoad = new HashSet<string>();

        //check if got words folder
        Directory.CreateDirectory(Application.persistentDataPath + "/Words");

        GetContentListRequest listReq = new GetContentListRequest();
        PlayFabAdminAPI.GetContentList(
            listReq,
            (_result) => {
                foreach (ContentInfo content in _result.Contents.Where(x => x.Key.StartsWith("Words"))) {
                    //check if we have it
                    if (!File.Exists(Application.persistentDataPath + "/" + content.Key)) {
                        Debug.Log("We don't have " + content.Key + "... downloading it");
                        GetContentDownloadUrlRequest dlReq = new GetContentDownloadUrlRequest();
                        dlReq.Key = content.Key;
                        PlayFabClientAPI.GetContentDownloadUrl(
                            dlReq,
                            (__result) => { StartCoroutine(DownloadData(__result.URL, content.Key)); },
                            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
                        );
                    } else {
                        BinaryFormatter bf = new BinaryFormatter();
                        FileStream fs = File.Open(Application.persistentDataPath + "/" + content.Key, FileMode.Open);
                        m_prosesAvaliable.Add(JsonUtility.FromJson<Paragraph>(bf.Deserialize(fs) as string));
                        fs.Close();
                    }
                }
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    public Paragraph GetRandomProse() {
         return m_prosesAvaliable[Random.Range(0, m_prosesAvaliable.Count - 1)];

        //DEBUGING SHIT
        //Debug.Log("Getting cursed prose");
        //bool xd = false;
        //foreach (Paragraph p in m_prosesAvaliable) {
        //    if (!p.Author.Equals("JK Rowling")) {
        //        if (!xd) {
        //            xd = true;
        //            continue;
        //        }
        //        return p;
        //    }
        //}
        //return m_prosesAvaliable[2];
    }

    private IEnumerator DownloadData(string _url, string _key) {
        using (UnityWebRequest webReq = UnityWebRequest.Get(_url)) {
            yield return webReq.SendWebRequest();
            if (webReq.isNetworkError) {
                Debug.LogError("Network error: " + webReq.error);
            } else {
                #region KEEP THIS
                //Debug.Log("Data gotten: " + webReq.downloadHandler.text);
                //string[] data = webReq.downloadHandler.text.Split('"');
                //string paraProse = data[3];
                //string paraAuthor = data[7];
                //string paraSrc = data[11];
                //m_prosesAvaliable.Add(new Paragraph(paraProse, paraAuthor, paraSrc));
                //string toStr = Encoding.UTF8.GetString(webReq.downloadHandler.data, 2, webReq.downloadHandler.data.Length - 2);
                //Debug.Log(webReq.downloadHandler.text);
                #endregion

                // Sometimes the json files have a 3 byte BOM infront of it
                // Thus, I cannot parse the .text into a json file
                // Hence, I use a try catch
                // If it does have the 3 byte BOM, I just truncate it
                Paragraph proseAvaliable;
                try {
                    proseAvaliable = JsonUtility.FromJson<Paragraph>(webReq.downloadHandler.text);
                } catch (System.ArgumentException) {
                    proseAvaliable = JsonUtility.FromJson<Paragraph>(Encoding.UTF8.GetString(webReq.downloadHandler.data, 3, webReq.downloadHandler.data.Length - 3));
                }

                m_prosesAvaliable.Add(proseAvaliable);

                //cache data
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Create(Application.persistentDataPath + "/" + _key);
                bf.Serialize(fs, JsonUtility.ToJson(proseAvaliable));
                fs.Close();
            }
        }
    }
}
