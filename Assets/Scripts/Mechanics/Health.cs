using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {
    [SerializeField] private Upgrade upgrade;
    [SerializeField] private Upgrade regenerationUpgrade;
    [SerializeField] private int startingValue;

    public int maxValue => startingValue + (upgrade ? (int) upgrade.CalculateMultiplier() : 0);
    public int value { get; private set; }
    public float Percent => (float) value / maxValue;

    public UnityEvent OnChanged = new UnityEvent();
    public UnityEvent OnDamaged = new UnityEvent();
    
    void OnEnable(){
        Refresh();
    }

    public void Refresh(){
        value = maxValue;
        OnChanged.Invoke();
    }

    public void DealDamage(int damage){
        var prevValue = value;
        value = Mathf.Min(value - damage, maxValue);
        OnChanged.Invoke();
        if(value < prevValue) OnDamaged.Invoke();
    }

    public void Regenerate(){
        if(!regenerationUpgrade) return;
        int restore = (int) regenerationUpgrade.CalculateMultiplier();
        DealDamage(-restore);
    }
}
