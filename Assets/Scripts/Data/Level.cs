using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

// visual representation of level
// generates entities for curr level
// responds to interaction between entities 

public class Level : MonoBehaviour, IInitable, IUpdatable
{
    public Platform m_Platform;

    public Surface[] m_Surfaces;

    public Transform m_TrsLevelObjects;

    List<GObject> m_AllEnemies = new List<GObject>();
    List<GObject> m_AllFlyingBonuses = new List<GObject>();

    public List<GObject> GetAllEnemies(){
        return m_AllEnemies;
    }

    public Platform GetPlatform(){
        return m_Platform;
    }

    int m_TotalEnemiesOnLevel;

    public int GetEnemiesLeft(){
        return m_AllEnemies.Count;
    }

    Vector3 m_VcTemp = new Vector3();

    Vector3 m_WorldBounds;

    public Vector3 WorldBounds{

        private set{m_WorldBounds = value;}

        get{return m_WorldBounds;}
    }

    public void GetWorldBounds(out float x, out float y, out float z){
        x = m_WorldBounds.x;
        y = m_WorldBounds.y;
        z = m_WorldBounds.z;
    }

    public void Init(){

        m_Platform.PreInit();
        // m_Platform.m_PhysBody.AddCollisionListener(OnPlatformCollision);
        m_Platform.AddBallTouchFloorListener(OnFloorTouch);

        CalcWorldBounds();
        InitSurfaces();
    }

    void CalcWorldBounds(){

        m_VcTemp.x = Screen.width;
        m_VcTemp.y = Screen.height;
        m_VcTemp.z = Camera.main.transform.position.z;

        m_WorldBounds = Camera.main.ScreenToWorldPoint(m_VcTemp);

        Debug.Log("m_WorldBounds " + m_WorldBounds);
    }

    void InitSurfaces(){
        for (int i=0; i<m_Surfaces.Length; i++){            
            m_Surfaces[i].PreInit();
        }
    }

#region callbacks 

    // level generated 
    UnityAction m_OnLevelGenerateCallback;

    public void AddLevelGenerateListener(UnityAction listener){
        m_OnLevelGenerateCallback += listener;
    }
    
    public void RemoveLevelGenerateListener(UnityAction listener){
        m_OnLevelGenerateCallback -= listener;
    }

    void OnLevelGenerate(){
        if (m_OnLevelGenerateCallback!=null)
            m_OnLevelGenerateCallback();
    } 

    // enemy die 
    UnityAction m_OnEnemyDieCallback;

    public void AddEnemyDieListener(UnityAction listener){
        m_OnEnemyDieCallback += listener;
    }
    
    public void RemoveEnemyDieListener(UnityAction listener){
        m_OnEnemyDieCallback -= listener;
    }

    void OnEnemyDeath(){
        if (m_OnEnemyDieCallback!=null)
            m_OnEnemyDieCallback();
    } 

    // floor touch 
    UnityAction m_OnFloorTouchCallback;

    public void AddFloorTouchListener(UnityAction listener){
        m_OnFloorTouchCallback += listener;
    }
    
    public void RemoveFloorTouchListener(UnityAction listener){
        m_OnFloorTouchCallback -= listener;
    }

    void OnFloorTouch(){
        if (m_OnFloorTouchCallback!=null)
            m_OnFloorTouchCallback();
    } 


#endregion

    public void Generate(int level){

        SetupPlatform();

        switch(level){
            case 0:
                GenerateEnemies(false, false);
            break;

            case 1: 
                GenerateEnemies(true, false);
            break;

            case 2: 
                GenerateEnemies(true, true);
            break;
        }

        OnLevelGenerate();
    }

    void SetupPlatform(){

        m_VcTemp.x = 0;
        m_VcTemp.y = - m_WorldBounds.y + m_Platform.GetHeight()/2;

        m_Platform.Put(m_VcTemp.x, m_VcTemp.y);       
        m_Platform.Reset();
    }

    const float c_EnemyProbability = 0.35f;
    const float m_EnemyGapTotal = 2;

    // upper half of the screen is dedicated to enemies 
    void GenerateEnemies(bool randomizeHealth, bool randomizeBonuses){

        float enemyCurrProbability = 0;

        GObject entity = null;

        ReturnOldObjectsToPool();

        float worldSizeX = m_WorldBounds.x * 2;
        float worldSizeY = m_WorldBounds.y;

        //possible num of enemies in matrix taking into account gaps between and screen size
        float enemiesOnXTotalWidth = Mathf.Floor(worldSizeX);
        float enemiesOnYTotalHeight = Mathf.Floor(worldSizeY);

        float borderX = worldSizeX - enemiesOnXTotalWidth;
        float borderY = worldSizeY - enemiesOnYTotalHeight;

        float enemySpaceX = (enemiesOnXTotalWidth - m_EnemyGapTotal) / Enemy.WIDTH;
        float enemySpaceY = (enemiesOnYTotalHeight - m_EnemyGapTotal) / Enemy.HEIGHT;

        float enemiesOnX = Mathf.Floor(enemySpaceX);
        float enemiesOnY = Mathf.Floor(enemySpaceY);

        borderX += (enemySpaceX - enemiesOnX);
        borderY += (enemySpaceY - enemiesOnY);

        float gapX = m_EnemyGapTotal/(enemiesOnX - 1);
        float gapY = m_EnemyGapTotal/(enemiesOnY - 1);

        // Debug.Log("worldSizeX " + worldSizeX + " enemiesOnX " + enemiesOnX + " borderX " + borderX);
        // Debug.Log("worldSizeY " + worldSizeY + " enemiesOnY " + enemiesOnY + " borderY " + borderY);

        // Debug.Log("gapX " + gapX + " enemiesOnXTotalWidth " + enemiesOnXTotalWidth);
        // Debug.Log("gapY " + gapX + " enemiesOnYTotalHeight " + enemiesOnYTotalHeight);

        for (int i=0; i<enemiesOnX; i++){
            for (int j=0; j<enemiesOnY; j++){

                // if (j>0) break; // test 

                enemyCurrProbability = UnityEngine.Random.Range(0f, 1f);
                if (enemyCurrProbability <= c_EnemyProbability){

                    entity = MainLogic.GetMainLogic().GetEntityManager().GetEntity(GObject.ObjectType.Enemy);
                    // Debug.Log("put enemy " + entity);

                     if (entity != null){

                        m_VcTemp.x = borderX/2 + gapX*i + Enemy.WIDTH*i + Enemy.WIDTH/2 - m_WorldBounds.x;
                        m_VcTemp.y = (-1)*(borderY/2 + gapY*j + Enemy.HEIGHT*j + Enemy.HEIGHT/2) + m_WorldBounds.y;

                        entity.Put(m_VcTemp.x, m_VcTemp.y);

                        entity.m_Transform.SetParent(m_TrsLevelObjects);
                        m_AllEnemies.Add(entity);

                        Enemy enemy = entity as Enemy;
                        enemy.Reset();
                        
                        enemy.AddDieListener(OnEnemyDie);
                        
                        if (randomizeHealth)
                           enemy.RandomizeHealth();
                        else
                            enemy.SetDefaultHealth();

                        if (randomizeBonuses){
                            enemy.RandomizeBonus();
                        }
                    }
                }
            }
        }
        m_TotalEnemiesOnLevel = m_AllEnemies.Count;
    }

    void ReturnOldObjectsToPool(){
        ReturnEnemiesToPool();
        ReturnBonusesToPool();
    }

    void ReturnEnemiesToPool(){
        for (int i=0; i<m_AllEnemies.Count; i++){
            MainLogic.GetMainLogic().GetEntityManager().ReturnToPool(m_AllEnemies[i]);
        }
        m_AllEnemies.Clear();
    }

     void ReturnBonusesToPool(){
        for (int i=0; i<m_AllFlyingBonuses.Count; i++){
            MainLogic.GetMainLogic().GetEntityManager().ReturnToPool(m_AllFlyingBonuses[i]);
        }
        m_AllFlyingBonuses.Clear();
    }

    void GetRidOfEnemy(GObject enemy){
        if (m_AllEnemies.Contains(enemy))
            m_AllEnemies.Remove(enemy);
        
        MainLogic.GetMainLogic().GetEntityManager().ReturnToPool(enemy);
    }

    void GetRidOfBonus(GObject bonus){
        if (m_AllFlyingBonuses.Contains(bonus))
            m_AllFlyingBonuses.Remove(bonus);
        
        MainLogic.GetMainLogic().GetEntityManager().ReturnToPool(bonus);
    }

    // void OnPlatformCollision(PhysicsBody body){

    //     switch(body.m_GObject.GetObjType()){

    //         case GObject.ObjectType.Bonus:

    //             Debug.LogError("OnPlatformCollision " + body);

    //             Bonus bonus = (body.m_GObject as Bonus);
    //             m_Platform.ApplyBonus(bonus.GetBonus());

    //             GetRidOfBonus(body.m_GObject);

    //         break;
    //     }
    // }

    void BonusTouchPlatform(Bonus bonus){

        // Debug.LogError("BonusTouchPlatform " + bonus);

        m_Platform.ApplyBonus(bonus.GetBonus());
        GetRidOfBonus(bonus);
    }

    void BonusTouchFloor(Bonus bonus){

        // Debug.LogError("BonusTouchFloor " + bonus);

        GetRidOfBonus(bonus);
    }

    // void OnFloorCollision(PhysicsBody body){

    //     switch(body.m_GObject.GetObjType()){
    //         case GObject.ObjectType.Ball: 
    //             OnFloorTouch();
    //         break;

    //         case GObject.ObjectType.Bonus:
                
    //             // Debug.LogError("OnFloorCollision " + body);

    //             GetRidOfBonus(body.m_GObject);
    //         break;
    //     }
    // }

    void OnEnemyDie(Enemy enemy){                   
        if (enemy.HasBonus())
            DispatchBonus(enemy);

        GetRidOfEnemy(enemy);
        OnEnemyDeath();
    }

    void DispatchBonus(Enemy enemy){
        GObject entity = MainLogic.GetMainLogic().GetEntityManager().GetEntity(GObject.ObjectType.Bonus);
        if (entity != null){

            entity.m_Transform.SetParent(m_TrsLevelObjects);
            entity.m_Transform.position = enemy.m_Transform.position;

            Bonus bonus = entity as Bonus;
            bonus.SetBonus(enemy.GetBonus());
            bonus.AddFloorTouchListener(BonusTouchFloor);
            bonus.AddPlatformTouchListener(BonusTouchPlatform);
            m_AllFlyingBonuses.Add(bonus);
        }
    }

    public void UpdateMe(float deltaTime){

        m_Platform.UpdateMe(deltaTime);
        
        UpdateBonuses(deltaTime);
    }

    void UpdateBonuses(float deltaTime){
        for(int i=0; i<m_AllFlyingBonuses.Count; i++){
            m_AllFlyingBonuses[i].UpdateMe(deltaTime);
        }
    }
}
