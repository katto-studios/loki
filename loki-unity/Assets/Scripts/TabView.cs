using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//master control for tabs
[ExecuteAlways]
public class TabView : MonoBehaviour {
	[Header("Tab info")]
	public int buttonSpacing = 50;
	public Vector2 buttonSize = new Vector2(250, 50);

	private HashSet<IndividualTab> m_tabs = new HashSet<IndividualTab>();
	public void Subscribe(IndividualTab _tab) {
		//alignment
		_tab.TabButton.RectTransform.sizeDelta = buttonSize;
		_tab.TabButton.RectTransform.anchoredPosition = Vector2.down * (buttonSize.y + buttonSpacing) * m_tabs.Count;
		//select it
		OnSelectOther(_tab);
		m_tabs.Add(_tab);
	}

	public void OnSelectOther(IndividualTab _new) {
		foreach (IndividualTab tab in m_tabs) {
			tab.Panel.SetActive(tab == _new);
		}
	}
}