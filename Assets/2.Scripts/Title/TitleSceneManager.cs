using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.PlayBGM(SoundManager.BGMType.MainMenu);
    }
}
