using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

	// most commonly used button with a single text and icon
    public class CommonButton : GUIButtonBase
	{
		public Image m_Background; // can be null
		public Text m_Header; // can be null
		public Image m_Icon; // can be null 

		// public bool m_Active = true;

		// public void MakeActive(bool active)
		// {
		// 	if (m_Active != active)
		// 	{
		// 		m_Active = active;
		// 		Color color = active ? Color.white : Color.gray;
		// 		m_Header.color = m_Background.color = color;
		// 	}
		// }

		public void SetText(string text)
		{
			if (m_Header != null)
            	m_Header.text = text;
		}

		public void SetIcons(Sprite iconOn, Sprite iconOff)
		{
			//temp do nothing
		}

        // public void SetIcon(Sprite icon)
        // {
        //     m_Icon.sprite = icon;
        // }

		// public void BlockElement(bool block)
		// {
		// 	m_Button.SetInteractable(! block);

		// 	if (m_Background != null)
		// 		m_Background.color = block ? GUIColors.HALF_DEFAULT : GUIColors.DEFAULT;

		// 	if (m_Header != null)
		// 		m_Header.color = block ? GUIColors.HALF_DEFAULT : GUIColors.DEFAULT;

		// 	if (m_Icon != null)
		// 		m_Icon.color = block ? GUIColors.HALF_DEFAULT : GUIColors.DEFAULT;
		// }

        public void AnimateForAttention(float t)
        {
            // float sc = (Mathf.Sin(t * 3) + 1) * 0.07f + 1f;
            // m_RectTransform.localScale = PrefabManager.Vector3One * sc;
        }
	}
