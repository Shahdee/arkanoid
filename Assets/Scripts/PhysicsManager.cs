using UnityEngine;
using System.Collections.Generic;

// every gobject on scene has physics body 
// balls are active scene participants. they collide with other scene objects 
// when objects collide, they are added to curr collision list 
// and collision event is invoked for both of them 
// collision list is cleared every update | what about its performance? 
// if enemy is dead, it's marked as deactivated (and it's also in the pool )
// deactivated objects are not deleted from the list. they just don't participate in physics simulation 
// if the same objects collide 2 times in curr update, we need to check if it's already deactivated to prevent damaging something that is already dead

public class PhysicsManager : IUpdatable
{

   List<PhysicsBody> m_Bodies = new List<PhysicsBody>(); 
    
    // I would prefer to have 1 place from where Registration is invoked - TODO future 
   public void Register(PhysicsBody body){
       if (! m_Bodies.Contains(body)){
            m_Bodies.Add(body);
            body.Activate();
            // Debug.LogError("Register " + body);
       }
   }

   public void Unregister(PhysicsBody body){
       if (m_Bodies.Contains(body)){
            m_Bodies.Remove(body);
            // Debug.Log("Unregister " + body);
       }
   }


    public void UpdateMe(float delta){

        for (int i=0; i<m_Bodies.Count; i++){

            if (m_Bodies[i].m_GObject.GetObjType() != GObject.ObjectType.Ball) continue;

            if (! m_Bodies[i].isActivated()) continue;

            for (int j=i+1; j<m_Bodies.Count; j++){

                if (! m_Bodies[j].isActivated()) continue;

                if (TryCollide(m_Bodies[i], m_Bodies[j])){

                    m_Bodies[i].Collide(m_Bodies[j]);
                    m_Bodies[j].Collide(m_Bodies[i]);
                }
            }
        }
    }

    static Vector2 m_VcTemp = new Vector2();

    static bool TryCollide(PhysicsBody b1, PhysicsBody b2){

        // b1.PrintCoords();
        // b2.PrintCoords();

        m_VcTemp.x = Mathf.Abs(b1.GetX() - b2.GetX()) - b2.GetWidth();
        m_VcTemp.y = Mathf.Abs(b1.GetY() - b2.GetY()) - b2.GetHeight();

        if (m_VcTemp.x > 0 || m_VcTemp.y > 0)
            return false;

        // if ( Mathf.Abs(b1.GetX() - b2.GetX()) > b1.GetWidth()/2 + b2.GetWidth()/2) return false;
        // if ( Mathf.Abs(b1.GetY() - b2.GetY()) > b1.GetHeight()/2 + b2.GetHeight()/2) return false;
        return true;
    }

    public static bool TryCollide(GObject b1, GObject b2){

        // b1.m_PhysBody.PrintCoords();
        // b2.m_PhysBody.PrintCoords();

        m_VcTemp.x = Mathf.Abs(b1.GetX() - b2.GetX()) - b2.GetWidth();
        m_VcTemp.y = Mathf.Abs(b1.GetY() - b2.GetY()) - b2.GetHeight();

        if (m_VcTemp.x > 0 || m_VcTemp.y > 0)
            return false;

        // if ( Mathf.Abs(b1.GetX() - b2.GetX()) > b1.GetWidth()/2 + b2.GetWidth()/2) return false;
        // if ( Mathf.Abs(b1.GetY() - b2.GetY()) > b1.GetHeight()/2 + b2.GetHeight()/2) return false;
        return true;
    }
}
