using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateableObject : MonoBehaviour
{
    bool isRotating;
    Vector3 anchor;
    Vector3 previousEulerAngles;
    Vector3 defaultEulerAngles;
    Quaternion defaultRotation;
    public float sensitivity;
    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        defaultEulerAngles = transform.eulerAngles;
        defaultRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //Update isRotating values
        if (Input.GetMouseButtonDown(0) && !isRotating)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
                isRotating = true;
                anchor = Input.mousePosition;
                previousEulerAngles = transform.eulerAngles;
            }
        }
        else if(Input.GetMouseButtonUp(0) && isRotating)
        {
            isRotating = false;
        }

        if (isRotating)
        {
            transform.eulerAngles = previousEulerAngles +
                new Vector3(-(Input.mousePosition.y - anchor.y) * sensitivity,
                -( Input.mousePosition.x - anchor.x) * sensitivity, 0);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, defaultRotation, Time.deltaTime * 3);
        }
    }
}
