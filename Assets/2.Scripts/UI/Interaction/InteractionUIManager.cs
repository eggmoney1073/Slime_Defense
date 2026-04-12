using UnityEngine;

public class InteractionUIManager : SingletonGameobject<InteractionUIManager>
{
    [SerializeField] private LevelUpController _controller;
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), "Show"))
        {
            _controller.ShowUI();
        }

        if (GUI.Button(new Rect(10, 70, 100, 50), "Hide"))
        {
            _controller.HideUI();
        }
    }
}
