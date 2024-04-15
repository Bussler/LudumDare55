using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatMenu : MonoBehaviour
{
    public static StatMenu Instance;

    public Slider healthSlider;
    public Slider bloodSlider;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public void SetHealth(float health)
    {
        healthSlider.value = health;
    }

    public void SetMaxHealth(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void SetBlood(float blood)
    {
        bloodSlider.value = blood;
    }

    public void SetMaxBlood(float maxBlood)
    {
        bloodSlider.maxValue = maxBlood;
        bloodSlider.value = 0;
    }

}
