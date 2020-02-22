using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantCanvas : Singleton<PersistantCanvas>
{
    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<PersistantCanvas>().Length > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
