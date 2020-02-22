using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChime : Singleton<ButtonChime>
{
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Object.DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayChime()
    {
        audioSource.Play();
    }
}
