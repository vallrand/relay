using UnityEngine;
using UnityEngine.UI;

public class CooldownIndicator : MonoBehaviour {
    [SerializeField] private Weapon weapon;
    [SerializeField] private Image bar;

    void Update(){
        if(weapon.gameObject.activeInHierarchy)
            bar.fillAmount = 1f - weapon.CooldownPercent;
        else
            bar.fillAmount = 0f;
    }
}
