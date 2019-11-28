using UnityEngine;

public interface IUpdatable{
    void UpdateMe(float deltaTime);
}

public interface IInitable{
    void Init();
}