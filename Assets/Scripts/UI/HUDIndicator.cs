using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDIndicator : MonoBehaviour {
    [SerializeField] private Acceleration acceleration;
    [SerializeField] private Health health;

    [SerializeField] private Text healthText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Text speedText;
    [SerializeField] private Image speedBar;

    void OnEnable(){
        health?.OnChanged.AddListener(OnHealthChange);
        acceleration?.OnChange.AddListener(OnSpeedChange);
    }
    void OnDisable(){
        health?.OnChanged.RemoveListener(OnHealthChange);
        acceleration?.OnChange.RemoveListener(OnSpeedChange);
    }

    void OnSpeedChange(){
        speedText.text = $"x{acceleration.level}";
        speedBar.fillAmount = acceleration.Percent;
    }
    void OnHealthChange(){
        healthText.text = $"{health.value}/{health.maxValue}";
        healthBar.fillAmount = health.Percent;
    }
}
