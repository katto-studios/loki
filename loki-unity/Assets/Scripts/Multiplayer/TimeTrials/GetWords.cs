using System;
using System.Collections;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

//make call to playfab and get list of words
public class GetWords{
    public static event Action eOnInitalised;  
    public static event Action eOnReseted;
    public static event Action<string> eOnFailure;
    public static string[] Strings{ get; private set; }
    
    public static void Initalise(int _seed){
        GetText(_seed);
    }

    public static void Initalise(int _seed, Action _onSuccess, Action<string> _onFailure){
        eOnInitalised += () => {
            _onSuccess?.Invoke();
        };
        eOnFailure += (_str) => {
            _onFailure?.Invoke(_str);
        };
        GetText(_seed);
    }

    public static void Reset(){
        Strings = new string[0];
        eOnReseted?.Invoke();
    }

    private static void GetText(int _seed){
        PlayFabClientAPI.GetContentDownloadUrl(
            new GetContentDownloadUrlRequest(){
                Key = "TimeTrials/Words.txt"
            },
            (_result) => { PersistantCanvas.Instance.StartCoroutine(DownloadText(_result.URL, _seed)); },
            (_error) => {
                Debug.LogError(_error.GenerateErrorReport());
                eOnFailure?.Invoke(_error.GenerateErrorReport());
            }
        );
    }

    private static IEnumerator DownloadText(string _url, int _seed){
        using (UnityWebRequest req = UnityWebRequest.Get(_url)){
            yield return req.SendWebRequest();
            if (req.isNetworkError){
                GameplayConsole.Log("Network error: " + req.error);
                eOnFailure?.Invoke(req.error);
            }
            else{
                //Strings = RandomizeText(req.downloadHandler.text.Split(new string[]{"\n"}, StringSplitOptions.None), _seed);
                Strings = req.downloadHandler.text.Split(new string[]{"\n"}, StringSplitOptions.None);
                
                eOnInitalised?.Invoke();
            }
        }
    }

    public static string GetWord(){
        return Strings[Random.Range(0, Strings.Length - 1)];
    }

    private static string[] RandomizeText(string[] _randomizeMe, int _seed){
        Random.InitState(_seed);
        for (int count = 0; count < 100; count++){
            int number1 = Random.Range(0, _randomizeMe.Length);
            int number2 = Random.Range(0, _randomizeMe.Length);
            string temp = _randomizeMe[number1];
            _randomizeMe[number1] = _randomizeMe[number2];
            _randomizeMe[number2] = temp;
        }
        return _randomizeMe;
    }
}