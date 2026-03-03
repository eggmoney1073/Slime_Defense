using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PopUpManager : SingletonDontDestroyOnLoad<PopUpManager>
{
    public enum PopUpType
    {
        Options,
        //Pause,
        //Confirm,
        Count
    }

    Stack<PopUpBase> _activePopUpStack = new Stack<PopUpBase>();
    PopUpBase[] _popUps = new PopUpBase[(int)PopUpType.Count];

    const string _popUpResourcePathBase = "Assets/4.Prefabs/Popups/PopUp_";

    public void ClosePopup()
    {
        _activePopUpStack.Pop().Hide();
    }

    public void OpenPopup(PopUpType popUpType)
    {
        _popUps[(int)popUpType].Show();
    }

    void InstancePopups()
    {
        for(int i = 0; i < (int)PopUpType.Count; i++)
        {
            int index = i;

            string popUpPath = _popUpResourcePathBase + ((PopUpType)index).ToString() + ".prefab";

            Addressables.LoadAssetAsync<GameObject>(popUpPath).Completed += handle =>
            {
                if(handle.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogError($"Failed to load popup at path: {popUpPath}");
                    return;
                }

                if(handle.Result == null)
                {
                    Debug.LogError($"Loaded popup is null at path: {popUpPath}");
                    return;
                }
                
                GameObject popUpInstance = Instantiate(handle.Result, transform);
                PopUpBase popUp = popUpInstance.GetComponent<PopUpBase>();

                if(popUp == null)
                {
                    Debug.LogError($"PopUpBase component not found on popup prefab at path: {popUpPath}");
                    return;
                }

                _popUps[index] = popUp;
                _popUps[index].Initialize();
                _popUps[index].Hide();
            };
        }
    }



    void Start()
    {

            LoadingSystem.InitializeAddressables(() =>
            {
                InstancePopups();
            });
            return;
        
        //InstancePopups();
    }

    void Update()
    {
        
    }
}
