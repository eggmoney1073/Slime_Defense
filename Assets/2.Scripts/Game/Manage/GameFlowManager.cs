using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : SingletonGameobject<GameFlowManager>
{
    public enum GameState
    {
        Ready,
        Playing,
        Pause,
        GameOver,
        Clear
    }

    public GameState CurrentGameState { get { return _currentGameState; } }
    [SerializeField]
    private GameState _currentGameState;
    private Dictionary<GameState, System.Action> _gameStateActions = new Dictionary<GameState, System.Action>();


    public void ChangeGameState(GameState newState)
    {
        _currentGameState = newState;

        if (_gameStateActions.ContainsKey(newState))
        {
            _gameStateActions[newState]?.Invoke();
        }
    }


    public void SubscribeGameState(GameState state, System.Action action)
    {
        if (_gameStateActions.ContainsKey(state))
        {
            _gameStateActions[state] += action;
        }
        else
        {
            _gameStateActions[state] = action;
        }
    }

    public void UnsubscribeGameState(GameState state, System.Action action)
    {
        if (_gameStateActions.ContainsKey(state))
        {
            _gameStateActions[state] -= action;
        }
    }

    protected override void OnAwake()
    {
        _currentGameState = GameState.Ready;
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM(SoundManager.BGMType.Gameplay);
    }
}
