using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : SingletonGameobject<GameFlowManager>
{
    public enum GameState
    {
        Ready,
        Playing,
        Pause,
        LevelUp,
        GameOver,
        Clear
    }

    public GameState CurrentGameState { get { return _currentGameState; } }
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

    private void Start()
    {
        _currentGameState = GameState.Playing;
        StartCoroutine(ReadyGame(1f));
        SoundManager.Instance.PlayBGM(SoundManager.BGMType.Gameplay);
    }

    IEnumerator ReadyGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeGameState(GameState.Ready);
    }
}
