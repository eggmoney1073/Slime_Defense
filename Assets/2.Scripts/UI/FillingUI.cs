using UnityEngine;

public class FillingUI : MonoBehaviour
{
    [SerializeField] private RectTransform _backgroundRect;
    [SerializeField] private RectTransform _fillingRect;

    [SerializeField] private float _fillingAmount;

    public void SetFillingAmountValue(float amount)
    {
        _fillingAmount = amount;
    }

    private void Update()
    {
        SpinningFillingArea();
        SetUIFillingAmount(_fillingAmount);
    }

    void SetUIFillingAmount(float amount)
    {
        if (amount >= 1)
        {
            amount = 1;
        }
        else if (amount <= 0)
        {
            amount = 0;
        }

        _fillingRect.localScale = new Vector3(amount, amount, 1);
    }

    void SpinningFillingArea()
    {
        _fillingRect.Rotate(0, 0, 100 * Time.deltaTime);
    }
}
