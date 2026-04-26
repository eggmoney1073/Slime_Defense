using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public enum GameState
    {
        Pause,
        Playing,
        GameOver,
        Clear
    }

    public static GameState CurrentGameState { get; private set; }

    public void ChangeGameState(GameState newState)
    {
        CurrentGameState = newState;
    }
}
