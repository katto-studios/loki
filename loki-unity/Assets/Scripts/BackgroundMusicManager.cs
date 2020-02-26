using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : Singleton<BackgroundMusicManager>
{
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<BackgroundMusicManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            Object.DontDestroyOnLoad(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }
}
