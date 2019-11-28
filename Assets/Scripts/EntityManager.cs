using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour, IInitable
{
    // instantiate objects  
    // keep them in pool 

    public GameObject m_Enemy; 
    public GameObject m_Ball; 
    public GameObject m_Bonus; 
    public Transform m_TrsPoolObjects;
    List<GObject> m_ObjectsInPool = new List<GObject>();

    public void Init(){

    }

    // pool
    public GObject GetEntity(GObject.ObjectType oType){

        GObject gobj = GetFromPool(oType);

        if (gobj != null){            
            gobj.gameObject.SetActive(true);
            gobj.m_PhysBody.Activate();  
            return gobj;
        }
        else{
            GameObject gameObject = null;

            switch(oType){
                case GObject.ObjectType.Enemy:
                    gameObject = GameObject.Instantiate(m_Enemy, Vector3.zero, Quaternion.identity);
                break;

                 case GObject.ObjectType.Ball:
                    gameObject = GameObject.Instantiate(m_Ball, Vector3.zero, Quaternion.identity);
                break;

                case GObject.ObjectType.Bonus:
                    gameObject = GameObject.Instantiate(m_Bonus, Vector3.zero, Quaternion.identity);
                break;
            }
            if (gameObject != null){
                gobj = gameObject.GetComponent<GObject>();
                gobj.PreInit();
            }
        }
        return gobj;
    }

    public void ReturnToPool(GObject obj){

        // Debug.Log("ReturnToPool " + obj);

        m_ObjectsInPool.Add(obj);
        obj.m_Transform.SetParent(m_TrsPoolObjects);

        obj.PreClearForBuffer();

        obj.gameObject.SetActive(false);
    }

    GObject GetFromPool(GObject.ObjectType oType){

        int index = -1;

        for (int i=0; i<m_ObjectsInPool.Count; i++){
            if (m_ObjectsInPool[i].m_ObjectType == oType){
                index = i;
                break;
            }
        }

        if (index >= 0){
            GObject gobj = m_ObjectsInPool[index];
            m_ObjectsInPool.RemoveAt(index);
            return gobj;
        }
        return null;
    }
}
