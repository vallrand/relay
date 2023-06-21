using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private TorusTransform targetTransform;
    [Header("Movement")]
    [SerializeField] private float startingVelocity;
    [SerializeField] private float maximumVelocity;
    [SerializeField] private AnimationCurve velocityCurve;

    private PipeSystem pipeSystem;
    private float velocity;
    
    void OnEnable(){
        pipeSystem = GetComponentInParent<PipeSystem>();
        velocity = startingVelocity;
    }

    void Update(){
        targetTransform.position.x += velocity * Time.deltaTime;
        pipeSystem.distance = targetTransform.position.x;
        var matrix = pipeSystem.GetTransform(pipeSystem.distance);

        transform.localPosition = matrix.GetPosition();
        transform.localRotation = Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
    }

    void LateUpdate(){

    }
}
