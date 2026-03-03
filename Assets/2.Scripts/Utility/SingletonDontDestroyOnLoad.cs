using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonDontDestroyOnLoad<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _uniqueInstance;

    public static T Instance
    {
        get
        {
            return _uniqueInstance;
        }
    }

    protected virtual void Awake()
    {
        T instance = GetComponent<T>();
        if (_uniqueInstance != null && _uniqueInstance != instance)
        {
            Destroy(gameObject);
            return;
        }

        _uniqueInstance = instance;
        DontDestroyOnLoad(gameObject);
        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }
}
