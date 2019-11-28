using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 
public class WinGameplay : WinViewBase
{
    public Text m_Level;
    public Text m_Scores;

    protected override void InInit(){
        MainLogic.GetMainLogic().GetLevelLogic().AddScoreChangeListener(SetScores);
    }

    protected override WinControllerBase CreateController(){
        return new WinGameplayController(this);;
    }

    // when the game starts 
    protected override void OnShow(){
        InitUI();
    }

    void InitUI(){
        int level = MainLogic.GetMainLogic().GetLevelLogic().GetCurrentLevel();
        int currScores = MainLogic.GetMainLogic().GetLevelLogic().GetCurrScores();

        SetLevel(level);
        SetScores(currScores);
    }

    void SetLevel(int level){
        m_Level.text = (level + 1).ToString();
    }

    void SetScores(int scores){
        m_Scores.text = (scores).ToString();
    }


    protected override void OnHide(){

    }

    public override void UpdateMe(float deltaTime){
      
    }
}
