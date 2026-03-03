using UnityEngine;

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
        _startButton.SetActive(false);
        _touchToStartUI.SetActive(false);

        LoadingSystem.InitializeAddressables(AddressablesInitializeOnComplete);        
    }

    /// <summary>
    /// 화면 클릭시 작동
    /// </summary>
    public void TouchToStart()
    {
        LoadingSystem.LoadAddressableLoadingScene();
    }


    /// <summary>
    /// 초기화 완료 콜백
    /// </summary>
    void AddressablesInitializeOnComplete()
    {
        _startButton.SetActive(true);
        _touchToStartUI.SetActive(true);
    }
}
