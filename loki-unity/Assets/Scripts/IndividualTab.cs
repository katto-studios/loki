using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//individual tab
[ExecuteAlways]
public class IndividualTab : MonoBehaviour {
	[Header("Tab info")]
	public TabView parentTab;
	public string tabName;

	private TabButton m_btnTabButton;
	[HideInInspector]
	public TabButton TabButton { get { return m_btnTabButton; } }   //button accessor
	private GameObject m_panel;
	public GameObject Panel {
		get {
			if (!m_panel) {
				m_panel = transform.GetChild(0).gameObject;
			}
			return m_panel;
		}
	}

	private void Start() {
		if (!parentTab) {
			parentTab = FindObjectOfType<TabView>();
			if (!parentTab) {
				throw new System.Exception("No parent tab found!");
			}
		}

		if (!Panel) throw new System.Exception("No content!");

		m_btnTabButton = GetComponentInChildren<TabButton>();
		m_btnTabButton.OnButtonClick += ButtonClickEvent;

		m_btnTabButton.Text = tabName;

		parentTab.Subscribe(this);
	}

	private void ButtonClickEvent() {
		parentTab.OnSelectOther(this);
	}
}
