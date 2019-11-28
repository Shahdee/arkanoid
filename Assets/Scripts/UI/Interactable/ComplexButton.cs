using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.UI
{
	// a part of a more complex object 
	// cant go back to prefab buffer 
	// can be locked to prevent from clicking 

	// all visual part and info is stored in a class, derived from PrefabObjectUI
	// and this class is an object inside that class 

	// most commonly used toggle with a single text, which is localized

	public class ComplexButton : Button 
	{
		public Graphic[] m_Raycasts;
		public RectTransform m_RTrsToAnimate;

        // ButtonState - ? do we need it
        Vector3 m_InitialScale;
        bool m_Pressed = false;
        bool m_Enter = false;
        bool m_Down = false;

		protected override void Awake()
		{
            base.Awake();
            if (m_RTrsToAnimate != null)
                m_InitialScale = m_RTrsToAnimate.localScale;
		}

		void ChangeState(bool pressed){
            if (m_Pressed != pressed && m_RTrsToAnimate != null)
            {
                m_Pressed = pressed;
                if (m_Pressed)
                {
                    m_RTrsToAnimate.localScale = m_InitialScale * 0.9f;
                }
                else {
                    m_RTrsToAnimate.localScale = m_InitialScale;
                }
            }
        }

        void SetEnter(bool enter){
            m_Enter = enter;
            ChangeState(m_Down && m_Enter);
        }

        void SetDown(bool down){
            m_Down = down;
            ChangeState(m_Down && m_Enter);
        }

		public UnityAction<ComplexButton> m_OnBtnClickClbck;
        void OnClick()
        {
            if (m_OnBtnClickClbck!=null)
                m_OnBtnClickClbck(this);
        }

		public UnityAction<ComplexButton> m_OnBtnDownClbck;
        void OnDown()
        {
            if (m_OnBtnDownClbck!=null)
                m_OnBtnDownClbck(this);
            SetDown(true);
        }

		public UnityAction<ComplexButton> m_OnBtnUpClbck;
        void OnUp()
        {
            if (m_OnBtnUpClbck!=null)
                m_OnBtnUpClbck(this);
            SetDown(false);
        }

		public UnityAction<ComplexButton> m_OnBtnEnterClbck;
        void OnEnter()
        {
            if (m_OnBtnEnterClbck!=null)
                m_OnBtnEnterClbck(this);
            SetEnter(true);
        }

		public UnityAction<ComplexButton> m_OnBtnExitClbck;
        void OnExit()
        {
            if (m_OnBtnExitClbck!=null)
                m_OnBtnExitClbck(this);
            SetEnter(false);
        }

#region button events 
		public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (eventData.button != PointerEventData.InputButton.Left) return; // base internal class has similar check

            MakeSound();

			OnClick();
        }

		public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            base.OnPointerDown(eventData);

            // Debug.LogError("OnPointerDown");

            OnDown();
        }

		public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (eventData.button != PointerEventData.InputButton.Left) return;
			
            // Debug.LogError("OnPointerUp");

			OnUp();
        }

		public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            // Debug.LogError("OnPointerEnter ");

			OnEnter();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            // Debug.LogError("OnPointerExit");

            OnExit();
        }

#endregion // button events 

		void MakeSound()
		{
			//TODO: R - later 
		}

		public void SetInteractable(bool interact)
		{
            for (int i=0; i<m_Raycasts.Length; i++)
            {
                m_Raycasts[i].raycastTarget = interact;
                interactable = interact;
            }
		}

		void Tutorial()
		{
			// TODO: R - consider tutorial 
		}

	}
}