using UnityEngine;

public class PauseController : MonoBehaviour
{
    public bool IsPaused { get; private set; } = false;
    public void OnPauseButtonClicked()
    {
        GameFlowManager.Instance.ChangeGameState(GameFlowManager.GameState.Pause);
        IsPaused = true;
    }

    public void Resume()
    {
        IsPaused = false;
    }
}
