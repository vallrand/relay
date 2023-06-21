using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorusTransform : MonoBehaviour {
    public Vector2 position;
    private PipeSystem pipeSystem;

    void Awake(){
        pipeSystem = GetComponentInParent<PipeSystem>();
    }

    void Update(){
        
    }

    void LateUpdate(){
        
    }
}
