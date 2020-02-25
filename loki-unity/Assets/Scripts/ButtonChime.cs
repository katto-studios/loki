using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChime : Singleton<ButtonChime>
{
    AudioSource audioSource;
        
    public List<AudioClip> audioClips;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<ButtonChime>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            Object.DontDestroyOnLoad(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayChime()
    {
        PlayChime(0);
    }

    public void PlayChime(int i)
    {
        audioSource.clip = audioClips[i];
        audioSource.Play();
    }
}
