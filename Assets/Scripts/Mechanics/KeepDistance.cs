using UnityEngine;

public class KeepDistance : MonoBehaviour {
    [SerializeField] private float minDistance;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float acceleration;

    private PipeTransform transform2d;
    private AIController controller;

    void Awake(){
        transform2d = GetComponent<PipeTransform>();
        controller = GetComponent<AIController>();
    }
    void Update(){
        if(!controller.target) return;

        float distance = controller.target.position.y - transform2d.position.y;
        if(distance > minDistance){
            controller.velocity.y = Mathf.Min(maxVelocity, controller.velocity.y + Time.deltaTime * acceleration);
        }else{
            controller.velocity.y = Mathf.Max(0f, controller.velocity.y - Time.deltaTime * acceleration);
        }
    }
}
