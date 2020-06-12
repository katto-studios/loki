using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private Vector3 defaultPos;
    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = -(Input.mousePosition.x/Screen.width - 0.5f) * distance + defaultPos.x;
        float zPos = -(Input.mousePosition.y/Screen.height - 0.5f) * distance + defaultPos.z;
        transform.localPosition = new Vector3(xPos, defaultPos.y, zPos);
    }
}
