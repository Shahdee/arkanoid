using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObject : MonoBehaviour
{
    RectTransform _RectTransform;
    public RectTransform m_RectTransform
    {
        get
        {
            if (_RectTransform == null)
                _RectTransform = transform as RectTransform;
            
            return _RectTransform;
        }
        private set{_RectTransform = value;}
    }

    public void Show(bool show){
        if (gameObject.activeSelf != show)
            gameObject.SetActive(show);
    }

    public bool isShown(){
        return gameObject.activeSelf;
    }

}
