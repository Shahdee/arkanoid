using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 
public class WinLevelComplete : WinViewBase
{
    public Text m_Level;
    public Text m_Scores;

    public Button m_BtnRestart;
    public Button m_BtnNext;

    protected override void InInit(){

        m_BtnRestart.onClick.AddListener(RestartClick);
        m_BtnNext.onClick.AddListener(NextClick);
        
    }

    protected override WinControllerBase CreateController(){
        return new WinLevelCompleteController(this);;
    }

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

    void SetScores(int level){
        m_Scores.text = (level + 1).ToString();
    }

    void RestartClick(){
        (m_Controller as WinLevelCompleteController).SendRestart();
    }

    void NextClick(){
        (m_Controller as WinLevelCompleteController).SendStartNext();
    }

    protected override void OnHide(){

    }

    public override void UpdateMe(float deltaTime){
      
    }
}
