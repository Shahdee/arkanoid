using UnityEngine;
using UnityEngine.Events;

public class PhysicsBody : MonoBehaviour
{
    GObject _GObject;

    public GObject m_GObject{
        
        get{
            if (_GObject == null)
                _GObject = GetComponent<GObject>();

            return _GObject;
        }
        set{_GObject = value;}
    }

    public UnityAction<PhysicsBody> m_OnCollisionCallback;

    public void AddCollisionListener(UnityAction<PhysicsBody> listener){
        m_OnCollisionCallback += listener;
    }

    public void RemoveCollisionListener(UnityAction<PhysicsBody> listener){
        m_OnCollisionCallback -= listener;
    }

    void OnCollide(PhysicsBody body){

        m_GObject.CollisionOccure(body);

        if (m_OnCollisionCallback != null)
            m_OnCollisionCallback(body);
    }

    public void Init(){
        MainLogic.GetMainLogic().GetPhysics().Register(this);   
    }

    bool m_Activated = false;

    public bool isActivated(){
        return m_Activated;
    }

    public void Activate(){
        m_Activated = true;
    }

    // to remove it from physics updates
    public void Deactivate(){
        m_Activated = false;
    }

    public void ClearForBuffer(){
        RemoveListeners();
        Deactivate();
    }

    void RemoveListeners(){
        m_OnCollisionCallback = null;
    }

    public void Collide(PhysicsBody body){

        OnCollide(body);
    }

    public void PrintCoords(){
        Debug.Log("name " + name + " x: " + GetX() + " y: " + GetY() + " w: " + GetWidth() + " h: " + GetHeight());
    }

    public float GetWidth(){
        return m_GObject.GetWidth();
    }

    public float GetHeight(){
        return m_GObject.GetHeight();
    }

    public float GetX(){
        return m_GObject.GetX();
    }

    public float GetY(){
        return m_GObject.GetY();
    }
}
