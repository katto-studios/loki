using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
[AddComponentMenu("UI/TabButton", 31)]
[ExecuteAlways]
public class TabButton : MonoBehaviour, IPointerDownHandler {
	public RectTransform RectTransform { get; private set; }
	private TextMeshProUGUI m_text;
	public string Text {
		get {
			if (!m_text) {
				m_text = GetComponentInChildren<TextMeshProUGUI>();
			}
			return m_text.text;
		}
		set { m_text.SetText(value); }
	}

	public Image Image { get; private set; }
	public event Action OnButtonClick;

	void Start() {
		RectTransform = GetComponent<RectTransform>();
		Image = GetComponent<Image>();
		m_text = GetComponentInChildren<TextMeshProUGUI>();
	}

	public void OnPointerDown(PointerEventData eventData) {
		OnButtonClick();
	}
}
