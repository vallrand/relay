using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, @InputActions.IPlayerActions {
    [SerializeField] public PipeTransform transform2d;
    [SerializeField] private Acceleration acceleration;
    [SerializeField] public Health health;
    [SerializeField] private Weapon weapon;

    [SerializeField] private Vector2 horizontalVelocity;
    [SerializeField] private float smoothness = 1f;

    [Header("Floating")]
    [SerializeField] private float floatingHeight;
    [SerializeField] private float floatingFrequency;

    private PipeSystem pipeSystem;
    private Vector2 velocity = Vector2.zero;
    private Vector2 direction;
    private bool action;

    void Awake(){
        pipeSystem = GetComponentInParent<PipeSystem>();
    }

    void Update(){
        float horizontal = Mathf.Lerp(horizontalVelocity.x, horizontalVelocity.y, acceleration.Fraction);
        Vector2 targetVelocity = acceleration.Velocity * new Vector2(direction.x * horizontal, 1f);
        velocity = velocity + smoothness * (targetVelocity - velocity) * Time.deltaTime;

        transform2d.position += Time.deltaTime * velocity;


        pipeSystem.distance = transform2d.position.y;
        var matrix = pipeSystem.GetTransform(pipeSystem.distance);

        transform.localPosition = matrix.GetPosition();
        transform.localRotation = Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));

        float angle = pipeSystem.innerRadius > 0f ? transform2d.position.x / pipeSystem.innerRadius : 0f;
        transform.localRotation = transform.localRotation * Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);

        float floating = floatingHeight * Mathf.Sin(Time.time * floatingFrequency);

        transform2d.transform.localPosition = new Vector3(0f, -pipeSystem.innerRadius + floating, 0f);

        if(action) weapon?.LaunchProjectile(transform.parent);
    }

    public void OnMove(InputAction.CallbackContext context){
        direction = context.ReadValue<Vector2>();
        direction.x = Mathf.Clamp(direction.x, -1f, 1f);
    }

    public void OnMoveTo(InputAction.CallbackContext context){
        var target = context.ReadValue<Vector2>();
        if(!Camera.main) return;
        target = Camera.main.ScreenToViewportPoint(target) * 2f - Vector3.one;
        if(Mathf.Abs(target.x) > 1f) return;
        float deadzone = 0.5f;
        if(target.x < -deadzone) direction.x = -1f;
        else if(target.x > deadzone) direction.x = 1f;
        else direction.x = 0f;
    }

    public void OnFire(InputAction.CallbackContext context){
        action = context.ReadValueAsButton();
    }
}
