using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;

        SetHealth(health);
    }

    public void SetHealth(int health)
    {
        int adjustedHealth = health >= 0 ? health : 0;

        slider.value = adjustedHealth;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
