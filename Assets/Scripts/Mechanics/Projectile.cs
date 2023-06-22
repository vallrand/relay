using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PipeTransform))]
public class Projectile : MonoBehaviour {
    [SerializeField] private ParticleSystem particles;
    [SerializeField] public float duration;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] public bool friendly;
    [SerializeField] public int collisionDamage;

    public event UnityAction<Collider> OnHit;
    public Vector2 velocity;
    private float elapsed;

    private PipeTransform targetTransform;
    void Awake(){
        targetTransform = GetComponent<PipeTransform>();
    }

    void Update(){
        targetTransform.position += velocity * Time.deltaTime;
        elapsed += Time.deltaTime;
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, Mathf.Clamp01((elapsed - duration) / fadeOutDuration));
        if(targetTransform.IsOutsideOfBounds || duration > 0f && elapsed > duration + fadeOutDuration)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player") == friendly) return;
        var health = other.GetComponent<Health>();
        if(!health || health.value <= 0) return;
        if(duration > 0f && elapsed > duration) return;

        GetComponent<Collider>().isTrigger = false;
        health.DealDamage(collisionDamage);
        OnHit?.Invoke(other);

        velocity = Vector2.zero;
        particles.Play();
        Destroy(gameObject, particles.main.duration);
    }
}
