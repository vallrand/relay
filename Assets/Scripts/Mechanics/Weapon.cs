using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour {
    [SerializeField] private Upgrade rateUpgrade;
    [SerializeField] private Upgrade rangeUpgrade;
    [SerializeField] private Upgrade damageUpgrade;

    [SerializeField] private float cooldown;
    [SerializeField] private float velocity;
    [SerializeField] private int addedDamage;
    [SerializeField] private Projectile projectilePrefab;
    
    [SerializeField] public UnityEvent OnKill = new UnityEvent();

    private float CalculatedCooldown => cooldown * (rateUpgrade ? rateUpgrade.CalculateMultiplier() : 1f);
    public float CooldownPercent => Mathf.Clamp01(elapsed / CalculatedCooldown);

    private float elapsed;

    void Update(){
        elapsed += Time.deltaTime;
    }

    public bool LaunchProjectile(Transform parent){
        return LaunchProjectile(parent, Vector2.up);
    }

    public bool LaunchProjectile(Transform parent, Vector2 direction){
        if(elapsed <= CalculatedCooldown) return false;
        elapsed = 0f;

        var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation, parent);
        projectile.GetComponent<PipeTransform>().position = GetComponent<PipeTransform>().position;

        var acceleration = GetComponent<Acceleration>();
        float initialVelocity = (acceleration ? acceleration.Velocity : 0f) + velocity;
        if(rangeUpgrade) initialVelocity += rangeUpgrade.CalculateMultiplier() * velocity;
        projectile.velocity = initialVelocity * direction;
        projectile.collisionDamage += addedDamage;
        if(damageUpgrade) projectile.collisionDamage += (int) damageUpgrade.CalculateMultiplier();
        projectile.OnHit += OnProjectileHit;

        return true;
    }

    private void OnProjectileHit(Collider other){
        var health = other.GetComponent<Health>();
        if(health.value <= 0) OnKill.Invoke();
    }
}
