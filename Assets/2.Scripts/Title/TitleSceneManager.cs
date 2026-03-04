using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    GameObject _touchToStartUI;
    [SerializeField]
    GameObject _startButton;
    [SerializeField]
    Camera _titleCamera;

    void Start()
    {
        _startButton.SetActive(true);
        _touchToStartUI.SetActive(true);
    }

    /// <summary>
    /// 화면 클릭시 작동
    /// </summary>
    public void TouchToStart()
    {
        //LoadingSystem.LoadAddressableLoadingScene();
        SceneManager.LoadScene(LoadingSystem.SceneName.Scene_DownLoad.ToString());
    }
}
