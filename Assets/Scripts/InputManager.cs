using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// handles screen touches  

public class InputManager : MonoBehaviour, IUpdatable, IInitable
{

    public void Init(){
        
    }

    public void UpdateMe(float deltaTime)
    {
        UpdateAxes();
    }

    public float m_Horizontal = 0;
    public float m_Vertical = 0;

    public float GetHorizontal(){
        return m_Horizontal;
    }

    public float GetVertical(){
        return m_Vertical;
    }

    void UpdateAxes(){
        m_Horizontal = Input.GetAxis("Horizontal");
        m_Vertical = Input.GetAxis("Vertical");
    }
}
