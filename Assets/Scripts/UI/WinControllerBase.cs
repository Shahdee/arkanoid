using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// concrete view instantiate concrete controller 

public class WinControllerBase
{
    protected WinViewBase m_View;

    public WinControllerBase(){
        
    }

    public WinControllerBase(WinViewBase view){
        m_View = view;
    }

    
   
}
