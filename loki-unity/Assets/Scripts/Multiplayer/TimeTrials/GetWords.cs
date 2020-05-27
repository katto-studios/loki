using System;
using System.Collections;
using System.Linq;
using PlayFab.ClientModels;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

//make call to playfab and get list of words
public class GetWords{
    public static event Action eOnInitalised;  
    public static event Action eOnReseted;
    public static string[] Strings{ get; private set; }
    
    public static void Initalise(int _seed){
        
    }

    public static void Inistalise(int _seed, Action _onSuccess, Action _onFailure){
        
    }

    public static void Reset(){
        Strings = new string[0];
        eOnReseted?.Invoke();
    }

    private static IEnumerator DownloadText(string _url, int _seed){
        using (UnityWebRequest req = UnityWebRequest.Get(_url)){
            yield return req.SendWebRequest();
            if (req.isNetworkError){
                GameplayConsole.Log("Network error: " + req.error);
            }
            else{
                Strings = RandomizeText(req.downloadHandler.text.Split('\\'), _seed);
                
                eOnInitalised?.Invoke();
            }
        }
    }

    private static string[] RandomizeText(string[] _randomizeMe, int _seed){
        Random.InitState(_seed);
        return _randomizeMe;
    }
}