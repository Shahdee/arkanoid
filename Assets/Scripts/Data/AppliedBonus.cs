using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// bonus applied to scene entityt

public class AppliedBonus : IUpdatable
{
    float m_CurrTime;

    const float DEFAULT_ELAPSE_TIME = 3f; // sec
    const float BALL_ELLAPSE_TIME = 7f;
    const float SPEED_ELAPSE_TIME = 4f;

    UnityAction<AppliedBonus> m_OnTimeElapseCallback;

    public void AddTimeElapseListener(UnityAction<AppliedBonus> listener){
        m_OnTimeElapseCallback += listener;
    }

    public void RemoveTimeElapseListener(UnityAction<AppliedBonus> listener){
        m_OnTimeElapseCallback -= listener;
    }

    void OnTimeElapse(){
        if (m_OnTimeElapseCallback != null)
            m_OnTimeElapseCallback(this);
    }

    Bonus.BonusType m_BonusType;

    public Bonus.BonusType GetBonusType(){
        return m_BonusType;
    }

    public AppliedBonus(Bonus.BonusType bType){
        m_BonusType = bType;
    }

    bool m_BonusActive = false;

    public void ActivateBonus(bool activate){
        if (m_BonusActive != activate){

            // Debug.LogError("ActivateBonus ");
            m_BonusActive = activate;

            if (activate){
                m_CurrTime = GetBonusElapseTime();
            }
        }  
    }

    public bool isActivated(){
        return m_BonusActive;
    }

    public void ActivateBonusTime(){

        ActivateBonus(true);
        m_CurrTime += GetBonusElapseTime();

        // Debug.LogError("ActivateBonusTime " + m_CurrTime);
    }
    
    public void UpdateMe(float deltatime){

        UpdateTimer(deltatime);
    }

    void UpdateTimer(float deltatime){

        if (! m_BonusActive) return;

        // Debug.Log("Bonus " + m_BonusType  + " tm= " + m_CurrTime);

        if (m_CurrTime > 0){
            m_CurrTime -= deltatime;
        }
        else{
            ActivateBonus(false);
            OnTimeElapse();
        }
    }

    float GetBonusElapseTime(){

        switch(m_BonusType){
            case Bonus.BonusType.Acceleration:
            case Bonus.BonusType.Deacceleration:
                return SPEED_ELAPSE_TIME;

            case Bonus.BonusType.Ball:
                return BALL_ELLAPSE_TIME;

            case Bonus.BonusType.PlatfromIncrease:
                return DEFAULT_ELAPSE_TIME;

            default:
                return 0;
        }
    }
}
