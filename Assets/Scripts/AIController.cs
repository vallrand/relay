using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PipeTransform))]
public class AIController : MonoBehaviour {
    [SerializeField] private int collisionDamage;
    [SerializeField] private GameObject shipModel;
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private float horizontalMovement;

    [HideInInspector] public Vector2 velocity;
    [HideInInspector] public PipeTransform target;

    private Health health;
    private Weapon weapon;
    private PipeTransform targetTransform;
    void Awake(){
        targetTransform = GetComponent<PipeTransform>();
        health = GetComponent<Health>();
        weapon = GetComponent<Weapon>();
        health?.OnChanged.AddListener(OnHealthChange);

        if(horizontalMovement != 0f) velocity.x = horizontalMovement * (Random.value < 0.5f ? 1f : -1f);
    }

    private void OnHealthChange(){
        if(health.value > 0) return;
        StartCoroutine(FreezeFrame(0.04f));
        StartCoroutine(ExplosionCoroutine());
    }

    void Update(){
        targetTransform.position += Time.deltaTime * velocity;

        if(target && weapon){
            weapon.LaunchProjectile(transform.parent, target.Difference(targetTransform));
        }

        if(targetTransform.IsOutsideOfBounds)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other){
        if(!other.CompareTag("Player")) return;
        var health = other.GetComponent<Health>();
        if(!health) return;

        health.DealDamage(collisionDamage);
        StartCoroutine(FreezeFrame(0.1f));
        StartCoroutine(ExplosionCoroutine());
    }

    IEnumerator ExplosionCoroutine(){
        foreach(Collider collider in GetComponents<Collider>()) collider.enabled = false;

        explosionParticles.Play();

        for(float duration = explosionParticles.main.duration, elapsed = 0f; elapsed < duration; elapsed += Time.deltaTime){
            if(shipModel)
                shipModel.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsed / duration);
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator FreezeFrame(float duration){
        Time.timeScale = 0;
        for(float elapsed = 0f; elapsed < duration; elapsed += Time.unscaledDeltaTime){
             yield return null;
        }
        Time.timeScale = 1;
    }
}
