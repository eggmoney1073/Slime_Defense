using UnityEngine;

public class PauseController : MonoBehaviour
{
    public bool IsPaused { get; private set; } = false;
    public void OnPauseButtonClicked()
    {
        TimeManager.Instance.SetPause();
        IsPaused = true;
    }

    public void Resume()
    {
        IsPaused = false;
    }
}
