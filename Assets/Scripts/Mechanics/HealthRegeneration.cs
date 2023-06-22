using UnityEngine;

public class HealthRegeneration : MonoBehaviour {
    [SerializeField] private Upgrade upgrade;
    [SerializeField] private int startingValue;
    [SerializeField] private int addedValue;
    [SerializeField] private Health health;

    public void Regenerate(){
        int restore = startingValue + upgrade.quantity * addedValue;
        health.DealDamage(-restore);
    }
}
