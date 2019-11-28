using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

// move 
    // it's always moving unless initial resting state and when we collide with floor 

// bounce 
    // enemies 
    // platform 
    // floor 
// deal damage to enemies 

public class Ball : GObject
{

    // impulse in direction / move direction 
    // acceleration 
    // damage dealed 

    int m_Damage; 
    const int DEFAULT_DAMAGE = 1;
    float m_Speed = DEFAULT_SPEED;

    public const float DEFAULT_SPEED = 5;
    public const float INC_SPEED = 10;
    public const float DEC_SPEED = 3;

    bool m_Resting = true;

    public bool isResting(){
        return m_Resting;
    }

     // On floor touch 
    public UnityAction m_OnFloorTouchCallback;

    public void AddFloorTouchListener(UnityAction listener){
        m_OnFloorTouchCallback += listener;
    }

    public void RemoveFloorTouchListener(UnityAction listener){
        m_OnFloorTouchCallback -= listener;
    }

    void OnFloorTouch(){
        if (m_OnFloorTouchCallback != null)
            m_OnFloorTouchCallback();
    }

    Vector3 m_MoveDir = new Vector3();
    static Vector3 m_VcTemp = new Vector3();
    static Vector3 m_VcTemp2 = new Vector3();
    static Vector3 m_VcTemp3 = new Vector3();
    static float m_Value;

    public void SetSpeed(float speed){
        m_Speed = speed;
    }

    void Impulse(Vector2 direction){

    }

    public int GetDamage(){
        return m_Damage;
    }

    public void Reset(){
        SetSpeed(DEFAULT_SPEED);
        m_Damage = DEFAULT_DAMAGE;
        m_Resting = true;
    }

    public override void UpdateMe(float deltaTime){

        if (m_Resting) return;

        m_VcTemp = m_Transform.position;

        m_VcTemp.x += m_Speed * deltaTime * m_MoveDir.x;
        m_VcTemp.y += m_Speed * deltaTime * m_MoveDir.y;
        
        m_Transform.position = m_VcTemp;

        // Debug.Log("2 UpdateMe " + m_Transform.position + " deltaTime " + deltaTime);

        UpdatePhysics(deltaTime);
    }

    public void StartMove(float dx, float dy){
        m_Resting = false;
        Impulse(dx, dy);
    }

    void PushOut(GObject gobj){

        
    }

    void Bounce(GObject entity){

        m_VcTemp.x = m_Transform.position.x - entity.GetX();
        m_VcTemp.y = m_Transform.position.y - entity.GetY();

        m_VcTemp2.x = Mathf.Abs(m_VcTemp.x) - entity.GetWidth();
        m_VcTemp2.y = Mathf.Abs(m_VcTemp.y) - entity.GetHeight();

        // Debug.Log("m_VcTemp" + m_VcTemp);
        // Debug.Log("m_VcTemp2 " + m_VcTemp2);

        if (m_VcTemp2.y > m_VcTemp2.x){
            // Debug.Log("x");
            Impulse(m_MoveDir.x, m_MoveDir.y * (-1));
        }
        else{
            // Debug.Log("y");
            Impulse(m_MoveDir.x * (-1), m_MoveDir.y);                    
        }

        // Debug.Log("_._ m_MoveDir" + m_MoveDir);

        m_VcTemp.Normalize();

        m_Value = Mathf.Abs(Mathf.Max(m_VcTemp2.x, m_VcTemp2.y));

        // Debug.Log("m_Value" + m_Value);

        m_VcTemp3 = m_Transform.position;

        m_VcTemp3.x += m_VcTemp.x * m_Value;
        m_VcTemp3.y += m_VcTemp.y * m_Value;

        // Debug.Log("m_VcTemp3" + m_VcTemp3);
        
        m_Transform.position = m_VcTemp3;
    }

    void PushOutWall(){

    }

    void Impulse(float dx, float dy){
        m_MoveDir.x = dx;
        m_MoveDir.y = dy;        
    }

    List<GObject> m_Enemies;
    Platform m_Platform;
    GObject[] m_Surfaces;

    void UpdatePhysics(float deltaTime){

        if (m_Resting) return;

        MainLogic.GetMainLogic().GetLevel().GetWorldBounds(out m_VcTemp.x, out m_VcTemp.y, out m_VcTemp.z);

        if (m_Transform.position.y < -m_VcTemp.y + GetHalfHeight()){
            OnFloorTouch();
            return;
        }

        m_Platform = MainLogic.GetMainLogic().GetLevel().GetPlatform();

        if (PhysicsManager.TryCollide(this, m_Platform)){
            Impulse(m_MoveDir.x, m_MoveDir.y*(-1));
            return;
        }

        m_Enemies = MainLogic.GetMainLogic().GetLevel().GetAllEnemies();

        for (int i=0; i<m_Enemies.Count; i++){

            if (PhysicsManager.TryCollide(this, m_Enemies[i])){

                Enemy en = m_Enemies[i] as Enemy;
                en.DealDamage(GetDamage());

                Bounce(en);
                return;
            }
        }

        m_Surfaces = MainLogic.GetMainLogic().GetLevel().m_Surfaces;

        for (int i=0; i<m_Surfaces.Length; i++){
            if (PhysicsManager.TryCollide(this, m_Surfaces[i])){
                Bounce(m_Surfaces[i]);
            }
        }          
    }

    public override void ClearForBuffer(){
        m_OnFloorTouchCallback = null;
    }

//     if(ballPosition.y <= brickPosition.y - (brickHeight/2))
//   //Hit was from below the brick

// if(ballPosition.y >= brickPosition.y + (brickHeight/2))
//   //Hit was from above the brick

// if(ballPostion.x < brickPosition.x)
//   //Hit was on left

// if(ballPostion.x > brickPosition.x)
//   //Hit was on right

    // Surface m_TmpSurface;

    // here only things that bounce 
    // public override void CollisionOccure(PhysicsBody body){

    //     if (m_Resting) return;

    //     switch(body.m_GObject.GetObjType()){

    //         case GObject.ObjectType.Enemy:
    //         case GObject.ObjectType.Platform:

    //         case GObject.ObjectType.Surface:

    //             Debug.LogWarning("OnCollision " + body);

    //             // Debug.Log("_ m_MoveDir" + m_MoveDir);

    //             m_VcTemp.x = m_Transform.position.x - body.GetX();
    //             m_VcTemp.y = m_Transform.position.y - body.GetY();

    //             m_VcTemp2.x = Mathf.Abs(m_VcTemp.x) - body.GetWidth();
    //             m_VcTemp2.y = Mathf.Abs(m_VcTemp.y) - body.GetHeight();

    //             // Debug.Log("m_VcTemp" + m_VcTemp);
    //             // Debug.Log("m_VcTemp2 " + m_VcTemp2);

    //             if (m_VcTemp2.y > m_VcTemp2.x){
    //                 Debug.Log("x");
    //                 Impulse(m_MoveDir.x, m_MoveDir.y * (-1));
    //             }
    //             else{
    //                 Debug.Log("y");
    //                 Impulse(m_MoveDir.x * (-1), m_MoveDir.y);                    
    //             }

    //             // Debug.Log("_._ m_MoveDir" + m_MoveDir);

    //             m_VcTemp.Normalize();

    //             m_Value = Mathf.Abs(Mathf.Max(m_VcTemp2.x, m_VcTemp2.y));

    //             // Debug.Log("m_Value" + m_Value);

    //             m_VcTemp3 = m_Transform.position;

    //             m_VcTemp3.x += m_VcTemp.x * m_Value;
    //             m_VcTemp3.y += m_VcTemp.y * m_Value;

    //             // Debug.Log("m_VcTemp3" + m_VcTemp3);
                
    //             m_Transform.position = m_VcTemp3;

    //         break;

    //         // case GObject.ObjectType.Surface:

    //             // m_TmpSurface = (Surface)(body.m_GObject);

    //             // switch(m_TmpSurface.GetSurfaceType()){
                    
    //             //     case Surface.SurfaceType.Ceiling:
    //             //         Impulse(m_MoveDir.x, m_MoveDir.y*(-1));
    //             //     break;

    //             //     case Surface.SurfaceType.WallLeft:
    //             //     case Surface.SurfaceType.WallRight:
    //             //         Impulse(m_MoveDir.x*(-1), m_MoveDir.y);                       
    //             //     break;
    //             // }
    //         // break;
    //     }
    // }
}
