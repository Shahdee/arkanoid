using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGameplayController : WinControllerBase
{
    public WinGameplayController(WinViewBase view) : base(view){
       
    }

    public void SendHeroMoveUp(){
        // m_MainLogic.SendHeroMove(Orientation.North);
    }

    public void SendHeroMoveDown(){
        // m_MainLogic.SendHeroMove(Orientation.South);
    }

    public void SendHeroMoveLeft(){
        // m_MainLogic.SendHeroMove(Orientation.West);
    }

    public void SendHeroMoveRight(){
        // m_MainLogic.SendHeroMove(Orientation.East);
    }

    // public void SendMoveNext(){
    //     m_MainLogic.MoveNext();
    // }

    // public void SendRestartLevel(){
    //     m_MainLogic.RestartLevel();
    // }


}
