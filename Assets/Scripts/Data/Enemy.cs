using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

// has life 
// can die 
// might contain bonus 

// life count and bonus existance both depend on level being played 

public class Enemy : GObject
{
    public GameObject[] m_FaceObjects;
    List<Bonus.BonusType> m_Bonuses = new List<Bonus.BonusType>();

    int _Health = 0;
    public int Health{

        get{return _Health;}
        private set{
            _Health = value;
            UpdateColor(_Health);
        }
    }

    public const int DEFAULT_HEALTH = 1;
    public const int MAX_HEALTH = 3;
    public const float WIDTH = 2;
    public const float HEIGHT = 1;

    // Died 
    public UnityAction<Enemy> m_OnDieCallback;

    public void AddDieListener(UnityAction<Enemy> listener){
        m_OnDieCallback += listener;
    }

    public void RemoveDieListener(UnityAction<Enemy> listener){
        m_OnDieCallback -= listener;
    }

    void OnDie(){
        if (m_OnDieCallback != null)
            m_OnDieCallback(this);
    }

    public void Reset(){
        SetBonusFace(false);
    }

    void SetBonusFace(bool bonus){
        m_FaceObjects[0].SetActive(! bonus);
        m_FaceObjects[1].SetActive(bonus);
    }

    public void SetDefaultHealth(){
        Health = DEFAULT_HEALTH;
    }

    public void RandomizeHealth(){
        Health = UnityEngine.Random.Range(0, MAX_HEALTH+1);
    }

    public void DealDamage(int damage){

        Health = Mathf.Max(0, Health-damage);
        
        if (Health == 0)
            OnDie();
    }

    void UpdateColor(int health){
        switch(health){

            case 0:
                m_Renderer.color = Color.grey;
            break;

            case 1:
                m_Renderer.color = Color.cyan;
            break;

            case 2:
                m_Renderer.color = Color.red;
            break;

            case 3:
                m_Renderer.color = Color.magenta;
            break;
        }
    }

    const float c_BonusChance = 0.35f; 

    public void RandomizeBonus(){

        float bonusRan = UnityEngine.Random.Range(0, 1f);

        if (bonusRan <= c_BonusChance){

            int enumCount = Enum.GetValues( typeof( Bonus.BonusType ) ).Length;
            int typeRan = UnityEngine.Random.Range(0, enumCount);
            // Bonus.BonusType bType = Bonus.BonusType.Deacceleration; // test

            m_AppliedBonus = new AppliedBonus((Bonus.BonusType)typeRan);
            // m_AppliedBonus = new AppliedBonus(bType); // test

            SetBonusFace(true);
        }
        else{
            SetBonusFace(false);
            m_AppliedBonus = null;
        }
    }

    AppliedBonus m_AppliedBonus = null;

    public AppliedBonus GetBonus(){
        return m_AppliedBonus;
    }

    public bool HasBonus(){
        return (m_AppliedBonus != null);
    }

    public override void ClearForBuffer(){
        RemoveListeners();

        m_AppliedBonus = null; // TODO future - I might handle it differently. Instead of deleting it -> could also reset and buffer
    }

    void RemoveListeners(){
        m_OnDieCallback = null;
    }  
}
