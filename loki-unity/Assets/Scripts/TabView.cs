using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabView : MonoBehaviour {
    [Header("Tab info")]
    public float tabHeight = 20;
    public float maxTabWidth = 200;
    public Sprite tabImg;

    private GameObject[] m_tabs;
    // Start is called before the first frame update
    void Start() {
        int noOfChildren = transform.childCount;
        if(noOfChildren > 1) {
            List<GameObject> children = new List<GameObject>();
            foreach(Transform child in transform) {
                children.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }

            m_tabs = children.ToArray();
            m_tabs[0].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}