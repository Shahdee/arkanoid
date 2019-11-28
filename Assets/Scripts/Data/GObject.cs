using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    
public class GObject : MonoBehaviour, IInitable, IUpdatable
{
    public SpriteRenderer m_Renderer;

    public PhysicsBody m_PhysBody;

    Transform _Transform;
    public Transform m_Transform
    {
        get
        {
            if (_Transform == null)
                _Transform = transform;
            
            return _Transform;
        }
        private set{_Transform = value;}
    }

    public enum ObjectType{
        Platform,
        Ball,
        Enemy,
        Bonus,    
        Surface,
    }

    public ObjectType m_ObjectType;

    public ObjectType GetObjType(){
        return m_ObjectType;
    }
    

    static Vector3 m_VcTemp = new Vector3();


    // happens once in lifetime of script 
    public void PreInit(){
        if (m_PhysBody != null)
            m_PhysBody.Init();

        Init();
    }

    public virtual void Init(){
        
    }

    public virtual void UpdateMe(float deltaTime){
        
    }

    public void Put(float x, float y){
        m_VcTemp.x = x;
        m_VcTemp.y = y;
        m_VcTemp.z = 0;

        m_Transform.position = m_VcTemp;
    }

    public float GetHalfWidth(){
        return m_Renderer.bounds.size.x/2;
    }

    public float GetHalfHeight(){
        return m_Renderer.bounds.size.y/2;
    }

    public float GetWidth(){
        return m_Renderer.bounds.size.x;
    }

    public float GetHeight(){
        return m_Renderer.bounds.size.y;
    }

    public float GetX(){
        return m_Transform.position.x;
    }

    public float GetY(){
        return m_Transform.position.y;
    }

    public void PreClearForBuffer(){
        m_PhysBody.ClearForBuffer();

        ClearForBuffer();
    }

    public virtual void ClearForBuffer(){

    }

    public virtual void CollisionOccure(PhysicsBody body){
        // implement in derived
    }
}
