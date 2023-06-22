using UnityEngine;

public class PipeTransform : MonoBehaviour {
    [SerializeField] private bool manual;
    public Vector2 position;
    private PipeSystem pipeSystem;

    public bool IsOutsideOfBounds => position.y < pipeSystem.minDistance || position.y > pipeSystem.maxDistance;

    protected virtual void Awake(){
        pipeSystem = GetComponentInParent<PipeSystem>();
        RecalculatePosition();
    }

    protected virtual void LateUpdate(){
        RecalculatePosition();
    }

    private void RecalculatePosition(){
        if(manual || !pipeSystem) return;
        var matrix = pipeSystem.GetTransform(position);
        transform.localPosition = matrix.GetPosition();
        transform.localRotation = Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
    }

    public Vector2 Difference(PipeTransform other){
        float height = 2f * Mathf.PI * pipeSystem.innerRadius;
        var res = new Vector2(
            ShortestDifference(position.x, other.position.x, height),
            position.y - other.position.y
        );
        return res;
    }
    public static float ShortestDifference(float a, float b, float m){
        float halfm = m * 0.5f;
        float diff = ( a - b + halfm ) % m - halfm;
        return diff < -halfm ? diff + m : diff;
    }
}
