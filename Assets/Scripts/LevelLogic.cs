using UnityEngine.Events;
using UnityEngine;

// class is responsible for starting and ending curr level
// also moving forward to next level 

// win - kill all enemies 
// lose - fall on the floor 

public class LevelLogic: IUpdatable
{
    UnityAction<int> m_OnGameStartCallback;

    UnityAction<int> m_OnLevelStartCallback;

    int m_CurrentLevel = 0;

    const int MAX_LEVELS = 3;

    public int GetCurrentLevel(){
        return m_CurrentLevel;
    }

    int m_CurrentScores = 0;

    public int GetCurrScores(){
        return m_CurrentScores;
    }

#region Callbacks

    // game start
    public void AddGameStartListener(UnityAction<int> listener){
        m_OnGameStartCallback += listener;
    }

    public void RemoveGameStartListener(UnityAction<int> listener){
        m_OnGameStartCallback -= listener;
    }

    public void OnGameStart(int level){
        if (m_OnGameStartCallback != null)
            m_OnGameStartCallback(level);
    }

    // level start  
    public void AddLevelStartListener(UnityAction<int> listener){
        m_OnLevelStartCallback += listener;
    }

    public void RemoveLevelStartListener(UnityAction<int> listener){
        m_OnLevelStartCallback -= listener;
    }

    public void OnLevelStart(int level){
        if (m_OnLevelStartCallback != null)
            m_OnLevelStartCallback(level);
    }

    // score change 
    UnityAction<int> m_OnScoreChangeCallback;

    public void AddScoreChangeListener(UnityAction<int> listener){
        m_OnScoreChangeCallback += listener;
    }

    public void RemoveScoreChangeListener(UnityAction<int> listener){
        m_OnScoreChangeCallback -= listener;
    }

    void OnScoreChange(){
        if (m_OnScoreChangeCallback != null)
            m_OnScoreChangeCallback(m_CurrentScores);
    }

    //level end 
    UnityAction m_OnLevelEndCallback;

    public void AddLevelEndListener(UnityAction listener){
        m_OnLevelEndCallback += listener;
    }

    public void RemoveLevelEndListener(UnityAction listener){
        m_OnLevelEndCallback -= listener;
    }

    public void OnLevelEnd(){
        if (m_OnLevelEndCallback != null)
            m_OnLevelEndCallback();
    }

#endregion

    public LevelLogic(){
        MainLogic.GetMainLogic().GetLevel().AddEnemyDieListener(EnemyDied);
        MainLogic.GetMainLogic().GetLevel().AddFloorTouchListener(FloorTouched);        
    }

    void EnemyDied(){

        AddScore();

        if (MainLogic.GetMainLogic().GetLevel().GetEnemiesLeft() == 0)
            LevelEnd();
    }

    void FloorTouched(){
        LevelEnd();
    }

    public void StartGame(int level){
        m_CurrentLevel = level;

        GenerateLevelData();

        ResetScores();

        OnGameStart(m_CurrentLevel);
    }

    public void RestartCurrLevel(){

        GenerateLevelData();

        ResetScores();

        OnLevelStart(m_CurrentLevel);
    }

    public void MoveNext(){
        m_CurrentLevel++;

        if (m_CurrentLevel > MAX_LEVELS - 1)
            m_CurrentLevel = 0;

        GenerateLevelData();

        ResetScores();

        OnLevelStart(m_CurrentLevel);
    }

    void GenerateLevelData(){

        // Debug.Log("GenerateLevelData " + m_CurrentLevel);

        MainLogic.GetMainLogic().GetLevel().Generate(m_CurrentLevel);
    }

    // either won or lost
    void LevelEnd(){

        OnLevelEnd();
    }

    int m_ScoreDelta = 1;

    void AddScore(){
        m_CurrentScores += m_ScoreDelta;
        m_CurrentScores = Mathf.Max(0, m_CurrentScores);

        OnScoreChange();
    }

    void ResetScores(){
        m_CurrentScores = 0;
    }

    public void UpdateMe(float deltaTime){

    }
}
