using Unity.Entities;
using UnityEngine;

public class TimeManager : SingletonGameobject<TimeManager>
{
    private float _gameTimeScale = 1f;
    private GameFlowManager _gameFlowManager;


    public void SetPause() => RequestTimeScaleChange(0f);
    public void SetTimeScaleNormal() => RequestTimeScaleChange(1f);
    public void SetTimeScaleFast() => RequestTimeScaleChange(2f);
    public void Resume() => RequestTimeScaleChange(_gameTimeScale);

    private void RequestTimeScaleChange(float targetTimeScale)
    {
        if (targetTimeScale > 0.3f)
        {
            _gameTimeScale = targetTimeScale;
        }

        World defaultWorld = World.DefaultGameObjectInjectionWorld;
        if (defaultWorld == null) return;

        EntityManager entityManager = defaultWorld.EntityManager;

        Entity requestEntity = entityManager.CreateEntity();
        entityManager.AddComponentData(requestEntity, new TimeScaleChangeRequest
        {
            targetTimeScale = targetTimeScale
        });
    }

    private void Start()
    {
        _gameFlowManager = GameFlowManager.Instance;
        if (_gameFlowManager != null)
        {
            _gameFlowManager.SubscribeGameState(GameFlowManager.GameState.Ready, SetPause);
            _gameFlowManager.SubscribeGameState(GameFlowManager.GameState.Playing, Resume);
            _gameFlowManager.SubscribeGameState(GameFlowManager.GameState.Pause, SetPause);
            _gameFlowManager.SubscribeGameState(GameFlowManager.GameState.GameOver, SetPause);
            _gameFlowManager.SubscribeGameState(GameFlowManager.GameState.Clear, SetPause);
        }
    }

    private void OnDestroy()
    {
        if (_gameFlowManager != null)
        {
            _gameFlowManager.UnsubscribeGameState(GameFlowManager.GameState.Ready, SetPause);
            _gameFlowManager.UnsubscribeGameState(GameFlowManager.GameState.Playing, Resume);
            _gameFlowManager.UnsubscribeGameState(GameFlowManager.GameState.Pause, SetPause);
            _gameFlowManager.UnsubscribeGameState(GameFlowManager.GameState.GameOver, SetPause);
            _gameFlowManager.UnsubscribeGameState(GameFlowManager.GameState.Clear, SetPause);
        }
    }
}
