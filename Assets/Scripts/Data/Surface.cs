using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : GObject
{
    // collides with ball 
    // wall or floor 
    // stretches to fit the world boundaries 

    public enum SurfaceType{
        Floor,
        WallLeft,
        WallRight,
        Ceiling
    }

    public SurfaceType m_SurfaceType;

    public SurfaceType GetSurfaceType(){
        return m_SurfaceType;
    }

    static Vector3 m_VcTemp;
    static Vector3 m_VcTemp2;

    // I would prefer to have 1 place from where Registration is invoked - TODO future 
    public override void Init(){
        Stretch();
    }

    void Stretch(){

        MainLogic.GetMainLogic().GetLevel().GetWorldBounds(out m_VcTemp.x, out m_VcTemp.y, out m_VcTemp.z);

        switch(m_SurfaceType){
            case SurfaceType.WallLeft: // stretch vertically
            case SurfaceType.WallRight: 

                // scale 
                m_VcTemp2 = m_Transform.localScale;
                m_VcTemp2.y = m_VcTemp.y*2;
                m_Transform.localScale = m_VcTemp2;

                //position
                m_VcTemp2 = m_Transform.position;
                m_VcTemp2.y = 0;

                if (m_SurfaceType == SurfaceType.WallLeft)                                        
                    m_VcTemp2.x = -m_VcTemp.x - m_Renderer.bounds.size.x/2;
                else
                    m_VcTemp2.x = m_VcTemp.x + m_Renderer.bounds.size.x/2;                
                m_Transform.position = m_VcTemp2;
                
            break;

            case SurfaceType.Floor: // stretch horizontally
            
                // scale 
                m_VcTemp2 = m_Transform.localScale;
                m_VcTemp2.x = m_VcTemp.x*2;
                m_Transform.localScale = m_VcTemp2;

                //position 
                m_VcTemp2 = m_Transform.position;
                m_VcTemp2.x = 0;
                m_VcTemp2.y = -m_VcTemp.y - m_Renderer.bounds.size.y/2;
                m_Transform.position = m_VcTemp2;

            break;

            case SurfaceType.Ceiling: // stretch horizontally
            
                // scale 
                m_VcTemp2 = m_Transform.localScale;
                m_VcTemp2.x = m_VcTemp.x*2;
                m_Transform.localScale = m_VcTemp2;

                //position 
                m_VcTemp2 = m_Transform.position;
                m_VcTemp2.x = 0;
                m_VcTemp2.y = m_VcTemp.y + m_Renderer.bounds.size.y/2;
                m_Transform.position = m_VcTemp2;

            break;
        }
    }
}
