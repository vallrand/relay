using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class DeformArc : MonoBehaviour {
    [SerializeField] private Mesh sourceMesh;
    [SerializeField, Range(0f, 360f)] public float rotation;
    [SerializeField] public float curveRadius;
    [HideInInspector] public float offset;
    public float Length => sourceMesh.bounds.max.z;

    void OnEnable(){
        GenerateMesh();
    }
    void OnDisable(){
        //TODO cleanup
    }

    private void GenerateMesh(){
        if(!sourceMesh) return;
        //TODO reuse mesh
        Mesh mesh = new Mesh(){ name = "Pipe" };
        Vector3[] vertices = new Vector3[sourceMesh.vertices.Length];
        Vector3[] normals = new Vector3[sourceMesh.normals.Length];

        for(int i = 0; i < vertices.Length; i++){
            var matrix = GetTransform(sourceMesh.vertices[i].z);
            Vector3 position = new Vector3(sourceMesh.vertices[i].x, sourceMesh.vertices[i].y, 0f);
            vertices[i] = matrix.MultiplyPoint(position);
            normals[i] = matrix.MultiplyVector(sourceMesh.normals[i]);
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = sourceMesh.triangles;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    public Matrix4x4 GetTransform(float distance){
        if(curveRadius == 0f) return Matrix4x4.identity;
        float angle = distance / curveRadius;
        return (
            Matrix4x4.Rotate(Quaternion.AngleAxis(rotation, Vector3.forward)) *
            Matrix4x4.Translate(new Vector3(0f, curveRadius, 0f)) *
            //TODo cache first part
            Matrix4x4.Rotate(Quaternion.AxisAngle(new Vector3(1f, 0f, 0f), -angle)) *
            Matrix4x4.Translate(new Vector3(0f, -curveRadius, 0f))
        );
    }
}
