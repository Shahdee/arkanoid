
using UnityEngine;
using UnityEngine.Events;

public class MainLogic : MonoBehaviour
{
    public GUILogic m_GUILogic;
    public EntityManager m_EntityManager;
    public InputManager m_InputManager;
    public Level m_Level;

    LevelLogic m_LevelLogic;
    PhysicsManager m_PhysManager;

    public enum GameStates{
        Menu,
        Play,
        Over,
    }

    GameStates m_CurrGameState = GameStates.Menu;

    public UnityAction<GameStates> m_OnGameStateChangeCallback;
    public void AddGameStateChangeListener(UnityAction<GameStates> listener){
        m_OnGameStateChangeCallback += listener;
    }

    public void RemoveGameStateChangeListener(UnityAction<GameStates> listener){
        m_OnGameStateChangeCallback -= listener;
    }

    void OnGameStateChange(){
        if (m_OnGameStateChangeCallback != null)
            m_OnGameStateChangeCallback(m_CurrGameState);
    }

    void SetGameState(GameStates state){

        if (m_CurrGameState != state){
            m_CurrGameState = state;

            OnGameStateChange();
        }
    }
    
    public EntityManager GetEntityManager(){
        return m_EntityManager;
    }

    public InputManager GetInputManager(){
        return m_InputManager;
    }

    public LevelLogic GetLevelLogic(){
        return m_LevelLogic;
    }

    public Level GetLevel(){
        return m_Level;
    }

    public PhysicsManager GetPhysics(){
        return m_PhysManager;
    }

    static MainLogic m_Logic;

    public static MainLogic GetMainLogic(){
        return m_Logic;
    }

    void Start()
    {
        m_Logic = this; 

        m_LevelLogic = new LevelLogic();
        m_PhysManager = new PhysicsManager();

        m_InputManager.Init();
        m_Level.Init();
        m_EntityManager.Init();
        m_GUILogic.Init();

        m_LevelLogic.AddGameStartListener(GameStarted);
        m_LevelLogic.AddLevelStartListener(LevelStarted);
        m_LevelLogic.AddLevelEndListener(LevelEnded);
    }

#region Level events 
    void GameStarted(int level){
        SetGameState(GameStates.Play);
    }

    void LevelStarted(int level){
        SetGameState(GameStates.Play);
    }

    void LevelEnded(){
        SetGameState(GameStates.Over);
    }
#endregion

    public void StartGame(){
        m_LevelLogic.StartGame(0);
    }

    public void RestartLevel(){
        m_LevelLogic.RestartCurrLevel();
    }

    public void MoveNext(){
        m_LevelLogic.MoveNext(); 
    }

    float deltaTime = 0;

    void FixedUpdate(){
        
    }

    void Update(){
        deltaTime = Time.deltaTime;

        UpdateState(deltaTime);
    }

    void UpdateState(float deltaTime){
        switch(m_CurrGameState){
            case GameStates.Menu:
                UpdateMenu(deltaTime);
            break;

            case GameStates.Play:
                UpdatePlay(deltaTime);
            break;

            case GameStates.Over:
                UpdateOver(deltaTime);
            break;
        }
    }

    void UpdateMenu(float deltaTime){

        m_GUILogic.UpdateMe(deltaTime);
    }

    void UpdatePlay(float deltaTime){

        m_InputManager.UpdateMe(deltaTime);

        m_LevelLogic.UpdateMe(deltaTime);
        m_Level.UpdateMe(deltaTime);

        // m_PhysManager.UpdateMe(deltaTime);        

        m_GUILogic.UpdateMe(deltaTime);
    }

    void UpdateOver(float deltaTime){

        m_GUILogic.UpdateMe(deltaTime);
    }
}
