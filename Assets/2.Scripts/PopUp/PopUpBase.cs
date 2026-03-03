using UnityEngine;

public abstract class PopUpBase : MonoBehaviour
{
    public virtual void Initialize()
    {

    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
