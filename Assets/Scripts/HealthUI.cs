using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    [Header("BAR")]
    public Slider HealthSlider;
    public Gradient color;
    public Image filling;

    public void MaxHealth(float HealthMax)
    {
        HealthSlider.maxValue = HealthMax;
        HealthSlider.value = HealthMax;
        filling.color = color.Evaluate(1);
    }
    public void MinHealth(float HealthMin)
    {
        HealthSlider.minValue = HealthMin;
        HealthSlider.value = HealthMin;
        filling.color = color.Evaluate(1);
    }
    public void HealthController(float health)
    {
        HealthSlider.value = health;
        filling.color = color.Evaluate(HealthSlider.normalizedValue);
    }

}
