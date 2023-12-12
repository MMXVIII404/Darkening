using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    public Slider batterySlider;
    private float decreaseRatePerSecond;
    public Image fill;
    public GameObject gameoverPanel;
    public FadeWarning fadeWarning;
    void Start()
    {
        decreaseRatePerSecond = 1 / 300f; // 在5分钟内减至0'
        fill.color = Color.green;
    }

    void Update()
    {
        if (fadeWarning.fadingDone)
        {
            if (batterySlider.value > 0)
            {
                batterySlider.value -= decreaseRatePerSecond * Time.deltaTime;
            }
            if (batterySlider.value < 0.2)
            {
                fill.color = Color.red;
            }
            if (batterySlider.value <= 0.01)
            {
                gameoverPanel.SetActive(true);
            }
        }
    }

    public void UseBattery(float usage)
    {
        batterySlider.value -= usage;
    }
}
