using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

    public class GUIButtonBase : UIObject
    {
        public ComplexButton m_Button;
        public UnityAction<GUIButtonBase> m_OnBtnClickClbck;
		void OnClick(ComplexButton cbtn)
        {
            // TODO future - play sound 

            if (m_OnBtnClickClbck != null)
                m_OnBtnClickClbck(this);
        }

		void Awake()
        {
            if (m_Button != null)
                m_Button.m_OnBtnClickClbck += OnClick;
        }
    }