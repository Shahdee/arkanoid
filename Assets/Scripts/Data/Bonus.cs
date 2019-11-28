using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// flying bonus on stage

public class Bonus : GObject
{
    // types
    // moves 
    // collide with platform and floor 
    // 

    Vector3 m_MoveDir = new Vector3(0, -1);
    const float DEFAULT_SPEED = 5;

    public enum BonusType{
        Acceleration = 0,
        Deacceleration,
        Ball,
        PlatfromIncrease,
    }

    float m_Speed;
    BonusType m_BonusType;

    // On floor touch 
    public UnityAction<Bonus> m_OnFloorTouchCallback;

    public void AddFloorTouchListener(UnityAction<Bonus> listener){
        m_OnFloorTouchCallback += listener;
    }

    public void RemoveFloorTouchListener(UnityAction<Bonus> listener){
        m_OnFloorTouchCallback -= listener;
    }

    void OnFloorTouch(){
        if (m_OnFloorTouchCallback != null)
            m_OnFloorTouchCallback(this);
    }

     // On platform touch 
    public UnityAction<Bonus> m_OnPlatformTouchCallback;

    public void AddPlatformTouchListener(UnityAction<Bonus> listener){
        m_OnPlatformTouchCallback += listener;
    }

    public void RemovePlatformTouchListener(UnityAction<Bonus> listener){
        m_OnPlatformTouchCallback -= listener;
    }

    void OnPlatformTouch(){
        if (m_OnPlatformTouchCallback != null)
            m_OnPlatformTouchCallback(this);
    }


    AppliedBonus m_AppliedBonus;

    public override void Init(){
        m_Speed = DEFAULT_SPEED;
    }

    public void SetBonus(AppliedBonus bonus){
        m_AppliedBonus = bonus;
        UpdateColor(m_AppliedBonus.GetBonusType());
    }

    public AppliedBonus GetBonus(){
        return m_AppliedBonus;
    }

    void UpdateColor(Bonus.BonusType bType){
        switch(bType){
            case BonusType.Acceleration:
                m_Renderer.color = Color.grey;
            break;

            case BonusType.Deacceleration:
                m_Renderer.color = Color.cyan;
            break;

            case BonusType.Ball:
                m_Renderer.color = Color.red;
            break;

            case BonusType.PlatfromIncrease:
                m_Renderer.color = Color.magenta;
            break;
        }
    }

    static Vector3 m_VcTemp = new Vector3();

    public override void UpdateMe(float deltaTime){

        m_VcTemp = m_Transform.position; 

        m_VcTemp.x += m_Speed * deltaTime * m_MoveDir.x;
        m_VcTemp.y += m_Speed * deltaTime * m_MoveDir.y;
        
        m_Transform.position = m_VcTemp;

        UpdatePhysics(deltaTime);
    }

    public override void ClearForBuffer(){
        m_AppliedBonus = null;
        m_OnFloorTouchCallback = null;
        m_OnPlatformTouchCallback = null;
    }

    Platform m_Platform;

    void UpdatePhysics(float deltaTime){

        MainLogic.GetMainLogic().GetLevel().GetWorldBounds(out m_VcTemp.x, out m_VcTemp.y, out m_VcTemp.z);

        if (m_Transform.position.y < -m_VcTemp.y + GetHalfHeight()){
            OnFloorTouch();
            return;
        }

        m_Platform = MainLogic.GetMainLogic().GetLevel().GetPlatform();

        if (PhysicsManager.TryCollide(this, m_Platform)){
            OnPlatformTouch();
            return;
        }
    }
}
