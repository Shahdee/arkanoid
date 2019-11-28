using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMainController : WinControllerBase
{
    public WinMainController(WinViewBase view) : base(view){
       
    }

    public void SendStart(){
        MainLogic.GetMainLogic().StartGame();
    }
}
