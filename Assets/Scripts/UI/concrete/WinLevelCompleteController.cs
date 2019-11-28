using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLevelCompleteController : WinControllerBase
{
    public WinLevelCompleteController(WinViewBase view) : base(view){
       
    }

    public void SendStartNext(){
        MainLogic.GetMainLogic().MoveNext();
    }

    public void SendRestart(){
        MainLogic.GetMainLogic().RestartLevel();
    }
}
