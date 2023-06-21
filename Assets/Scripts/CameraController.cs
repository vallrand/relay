using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;

    void LateUpdate(){
        transform.position = target.transform.position + target.transform.rotation * offset;
        transform.rotation = target.transform.rotation;
    }
}
