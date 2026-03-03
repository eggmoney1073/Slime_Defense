using System;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    void Start()
    {
        InputController.Instance.UIInput.OnConfirmAction += TestOnEnter;
    }

    void TestOnEnter()
    {
        Debug.Log("Enter Key Pressed");
    }

    void OnDestroy()
    {
        InputController.Instance.UIInput.OnConfirmAction -= TestOnEnter;
    }
}
