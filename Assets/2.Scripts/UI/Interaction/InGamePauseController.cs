using UnityEngine;

public class InGamePauseController : MonoBehaviour
{
    public bool IsPaused { get; private set; } = false;
    public void OnPauseButtonClicked()
    {
        GameFlowManager.Instance.ChangeGameState(GameFlowManager.GameState.Pause);
        IsPaused = true;
    }
}