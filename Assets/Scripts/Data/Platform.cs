using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// my super hero

// receive input 
// move 
// recieve bonus 
// collide with ball 

// has life 
// can die - if misses the ball 

// has physics events 

// if bonus (+1 ball) time is elapsed => remove a ball from queue 

// upon level start, input initiates first impuls 

// TODO - walls could also be platforms instead of surfaces 

public class Platform : GObject
{
    public Ball m_Ball;
    Queue<Ball> m_AdditionalBalls = new Queue<Ball>();
    
    List<AppliedBonus> m_AppliedBonuses = new List<AppliedBonus>();

    const float DEFAULT_WIDTH = 3;
    const float MAX_WIDTH = 5;
    const float SPEED = 20;  // [units]

    public UnityAction m_OnBallTouchFloorCallback;

    public void AddBallTouchFloorListener(UnityAction listener){
        m_OnBallTouchFloorCallback += listener;
    }

    public void RemoveBallTouchFloorListener(UnityAction listener){
        m_OnBallTouchFloorCallback -= listener;
    }

    void OnBallTouchFloor(){
        if (m_OnBallTouchFloorCallback != null)
            m_OnBallTouchFloorCallback();
    }

    // I would prefer to have 1 place from where Registration is invoked - TODO future 
    public override void Init(){
        m_Ball.PreInit();        
        m_Ball.AddFloorTouchListener(BallTouchedFloor);
    }

    public void Reset(){
        ClearBonuses();
        SetBall(m_Ball);
    }

    Vector3 m_VcTemp = new Vector3();
    Vector3 m_VcTemp2 = new Vector3();

    void SetBall(Ball ball){

        if (ball == null) return;

        m_VcTemp.x = m_Transform.position.x;
        m_VcTemp.y = m_Transform.position.y + GetHeight()/2 + ball.GetHeight()/2;
        ball.Put(m_VcTemp.x, m_VcTemp.y);    
        ball.Reset(); 

        // ball.m_PhysBody.PrintCoords();
    }

    void SetPlatformWidth(float width){
        m_VcTemp = m_Transform.localScale;
        m_VcTemp.x = width;
        m_Transform.localScale = m_VcTemp;
    }

    void SetBallsSpeed(float speed){

        m_Ball.SetSpeed(speed);

        foreach(Ball b in m_AdditionalBalls)
            b.SetSpeed(speed);
    }

    // platform size 
    // return balls to buffer
    void ClearBonuses(){
        for (int i=0; i<m_AppliedBonuses.Count; i++){
            m_AppliedBonuses[i].RemoveTimeElapseListener(BonusTimeUp);
        }
        m_AppliedBonuses.Clear();

        ResetPlatformWidth();

        ClearBalls();        
    }

    void ResetPlatformWidth(){
        m_VcTemp = m_Transform.localScale;
        m_VcTemp.x = DEFAULT_WIDTH;
        m_Transform.localScale = m_VcTemp;
    }

    void ClearBalls(){
        foreach(Ball b in m_AdditionalBalls){
            b.Reset();
            MainLogic.GetMainLogic().GetEntityManager().ReturnToPool(b);
        }
        m_AdditionalBalls.Clear();
    }

    // received bonus rules (when bonuses overlap)
    // V+ -> apply to all balls and inc elapse time 
    // V- -> apply to all balls and inc elapse time 
    // V- cancels V+  
    // V+ cancels V- 
    // +1 ball -> +1 ball 
    // platform inc -> inc elapse time 
    public void ApplyBonus(AppliedBonus bonus){
        // subscribe to elapse event 
        // apply bonus to platform or balls 
        // when event is thrown 
             // remove it 

        int index = GetBonusIndex(bonus.GetBonusType());

        // Debug.LogError("ApplyBonus " + bonus.GetBonusType());

        switch(bonus.GetBonusType()){
            case Bonus.BonusType.PlatfromIncrease:
                if (index >= 0){
                    if (m_AppliedBonuses[index].isActivated())
                        m_AppliedBonuses[index].ActivateBonusTime();
                    else
                        m_AppliedBonuses[index].ActivateBonus(true);
                }
                else
                {
                    m_AppliedBonuses.Add(bonus);
                    bonus.ActivateBonus(true);
                    bonus.AddTimeElapseListener(BonusTimeUp);
                }
                SetPlatformWidth(MAX_WIDTH);
            break;

            case Bonus.BonusType.Acceleration:
                HandleSpeedBonuses(bonus, Bonus.BonusType.Deacceleration);
            break;

            case Bonus.BonusType.Deacceleration:
                HandleSpeedBonuses(bonus, Bonus.BonusType.Acceleration);
            break;

            case Bonus.BonusType.Ball:

                GObject gobj = MainLogic.GetMainLogic().GetEntityManager().GetEntity(ObjectType.Ball);
                gobj.m_Transform.SetParent(MainLogic.GetMainLogic().GetLevel().m_TrsLevelObjects);

                Ball ball = gobj as Ball;
                ball.AddFloorTouchListener(BallTouchedFloor);
                SetBall(ball);

                int idx = GetBonusIndex(Bonus.BonusType.Acceleration);
                if (idx >= 0 && m_AppliedBonuses[idx].isActivated()){
                    ball.SetSpeed(Ball.INC_SPEED);
                }
                else{
                    idx = GetBonusIndex(Bonus.BonusType.Deacceleration);
                    if (idx >= 0 && m_AppliedBonuses[idx].isActivated()){
                        ball.SetSpeed(Ball.DEC_SPEED);
                    }
                }
                m_AdditionalBalls.Enqueue(ball);
                m_AppliedBonuses.Add(bonus);
                bonus.ActivateBonus(true);
                bonus.AddTimeElapseListener(BonusTimeUp);                    
            break;
        }
    }

    void HandleSpeedBonuses(AppliedBonus bonus, Bonus.BonusType bonusToDeactivate){

        int deactIdx = GetBonusIndex(bonusToDeactivate);
        if (deactIdx >= 0 && m_AppliedBonuses[deactIdx].isActivated()){
            m_AppliedBonuses[deactIdx].ActivateBonus(false);
        }

        int actIdx = GetBonusIndex(bonus.GetBonusType());

        if (actIdx >= 0){
            if (m_AppliedBonuses[actIdx].isActivated())
                m_AppliedBonuses[actIdx].ActivateBonusTime();
            else
                m_AppliedBonuses[actIdx].ActivateBonus(true);
        }
        else{
            m_AppliedBonuses.Add(bonus);
            bonus.ActivateBonus(true);
            bonus.AddTimeElapseListener(BonusTimeUp);
        }

        if (bonus.GetBonusType() == Bonus.BonusType.Acceleration)
            SetBallsSpeed(Ball.INC_SPEED);
        else
            SetBallsSpeed(Ball.DEC_SPEED);
    }

    int GetBonusIndex(Bonus.BonusType bType){
        for (int i=0; i<m_AppliedBonuses.Count; i++){
            if (m_AppliedBonuses[i].GetBonusType() == bType)
                return i;
        }
        return -1;
    }

    public override void UpdateMe(float deltaTime){

        UpdateMove(deltaTime);

        UpdateBonuses(deltaTime);

        UpdateBalls(deltaTime);
    }

    float m_MoveDeltaX = 0;

    void UpdateMove(float deltaTime){

        m_MoveDeltaX = MainLogic.GetMainLogic().GetInputManager().GetHorizontal();

        if (m_MoveDeltaX > 0 || m_MoveDeltaX < 0){
            MainLogic.GetMainLogic().GetLevel().GetWorldBounds(out m_VcTemp.x, out m_VcTemp.y, out m_VcTemp.z);

            m_VcTemp2 = m_Transform.position;
            m_VcTemp2.x += (m_MoveDeltaX * SPEED * deltaTime);
            m_VcTemp2.x = Mathf.Clamp(m_VcTemp2.x, -m_VcTemp.x + GetWidth()/2, m_VcTemp.x - GetWidth()/2);
            m_Transform.position = m_VcTemp2;

            CheckRestingBalls();
        }
    }

    void CheckRestingBalls(){
        if (m_Ball.isResting())
            m_Ball.StartMove(Mathf.Sign(m_MoveDeltaX),1);

        foreach(Ball b in m_AdditionalBalls){
            if (b.isResting())
                b.StartMove(Mathf.Sign(m_MoveDeltaX),1);
        }
    }

    void UpdateBonuses(float deltaTime){
        foreach(AppliedBonus bonus in m_AppliedBonuses)
            bonus.UpdateMe(deltaTime);
    }

    void BonusTimeUp(AppliedBonus bonus){

        Debug.LogError("BonusTimeUp " + bonus.GetBonusType());

        switch(bonus.GetBonusType()){
            case Bonus.BonusType.Acceleration:
                SetBallsSpeed(Ball.DEFAULT_SPEED);
            break;

            case Bonus.BonusType.Deacceleration:
                SetBallsSpeed(Ball.DEFAULT_SPEED);
            break;

            case Bonus.BonusType.Ball:
                Ball b = m_AdditionalBalls.Dequeue();   
                MainLogic.GetMainLogic().GetEntityManager().ReturnToPool(b);                
            break;

            case Bonus.BonusType.PlatfromIncrease:
                SetPlatformWidth(DEFAULT_WIDTH);
            break;
        }
    }

    void BallTouchedFloor(){
        OnBallTouchFloor();
    }

    void UpdateBalls(float deltaTime){

        m_Ball.UpdateMe(deltaTime);

        foreach(Ball b in m_AdditionalBalls)
            b.UpdateMe(deltaTime);
    }
}
