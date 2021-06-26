using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeCounter : MonoBehaviour
{

    public Text text;
    public Button minusButton;
    public Button plusButton;

    public int value = 5;

    const int minValue = 2;
    const int maxValue = 50;


    private void Awake()
    {
        text.text = value.ToString();
        minusButton.onClick.AddListener(() =>
        {
            value = Mathf.Clamp(value - 1, minValue, maxValue);
            text.text = value.ToString();
        });
        plusButton.onClick.AddListener(() =>
        {
            value = Mathf.Clamp(value + 1, minValue, maxValue);
            text.text = value.ToString();
        });
    }

}
