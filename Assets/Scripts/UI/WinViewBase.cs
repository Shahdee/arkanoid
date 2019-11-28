using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinViewBase : MonoBehaviour, IInitable, IUpdatable
{
    public enum WinType{
        Menu,
        Gameplay,
        LevelComplete
    }

    public Canvas m_Canvas;
    public GraphicRaycaster m_Raycaster;
    public WinType m_WindowType;
    protected WinControllerBase m_Controller;
    protected virtual WinControllerBase CreateController(){
        // implement in derived
        return null;
    }

    public void Init(){
        m_Controller = CreateController();

        gameObject.SetActive(true);

        InInit();
    }

    protected virtual void InInit(){
        // implement in derived
    }

    public void Open(){
        SetVisible(true);
    }

    public void Close(){
        SetVisible(false);
    }

    bool m_Visible;
    bool m_FirstTime = true;

    void SetVisible(bool visible){
        if (m_Visible != visible || m_FirstTime){

            m_Visible = visible;
            m_FirstTime = false;

            m_Canvas.enabled = m_Raycaster.enabled = visible;

            if (visible)
                OnShow();
            else
                OnHide();
        }
    }

    public bool isVisibile(){
        return m_Visible;
    }

    protected virtual void OnShow(){
        // instead of enable
        //implement in derived
    }

    protected virtual void OnHide(){
        // instead of disable
        //implement in derived
    }

    public virtual void UpdateMe(float deltaTime){
        // implement in derived
    }
}
