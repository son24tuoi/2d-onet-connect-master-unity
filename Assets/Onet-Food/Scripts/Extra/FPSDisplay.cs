using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
	private float m_deltaTime = 0.0f;

	private int m_w;
	private int m_h;

	private float m_msec;
	private float m_fps;
	private string m_text;

	private void Update()
	{
		m_deltaTime += (Time.unscaledDeltaTime - m_deltaTime) * 0.1f;
	}

	private void OnGUI()
	{
		m_w = Screen.width;
		m_h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, m_w, m_h * 2 / 100);

		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = m_h * 2 / 100;
		style.normal.textColor = Color.white;

		m_msec = m_deltaTime * 1000.0f;
		m_fps = 1.0f / m_deltaTime;

		m_text = string.Format("{0:0.0} ms ({1:0.} fps)", m_msec, m_fps);

		GUI.Label(rect, m_text, style);
	}
}
