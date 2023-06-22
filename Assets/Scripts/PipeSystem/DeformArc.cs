using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class DeformArc : MonoBehaviour {
    [SerializeField] private Mesh sourceMesh;
    [SerializeField, Range(0f, 360f)] public float rotation;
    [SerializeField] public float curveRadius;

    public float Length => sourceMesh.bounds.max.z;
    [HideInInspector] public float offset;
    [HideInInspector] public int index;

    private Matrix4x4 postTransform = Matrix4x4.identity;
    private Matrix4x4 preTransform = Matrix4x4.identity;
    private Mesh mesh;
    private Vector3[] vertices, normals;
    void Awake(){
        if(!sourceMesh) return;
        vertices = new Vector3[sourceMesh.vertices.Length];
        normals = new Vector3[sourceMesh.normals.Length];
        mesh = new Mesh(){ name = "Pipe" };
        GenerateMesh();
        mesh.triangles = sourceMesh.triangles;
        GetComponent<MeshFilter>().mesh = mesh;
    }
    void Destroy(){
        mesh?.Clear();
        GetComponent<MeshFilter>().mesh = mesh = null;
    }

    public void GenerateMesh(){
        if(!mesh || !sourceMesh) return;

        postTransform = Matrix4x4.Rotate(Quaternion.AngleAxis(rotation, Vector3.forward)) *
            Matrix4x4.Translate(new Vector3(0f, curveRadius, 0f));
        preTransform = Matrix4x4.Translate(new Vector3(0f, -curveRadius, 0f)) *
            Matrix4x4.Rotate(Quaternion.AngleAxis(-rotation, Vector3.forward));

        for(int i = 0; i < vertices.Length; i++){
            var matrix = GetTransform(sourceMesh.vertices[i].z);
            Vector3 position = new Vector3(sourceMesh.vertices[i].x, sourceMesh.vertices[i].y, 0f);
            vertices[i] = matrix.MultiplyPoint(position);
            normals[i] = matrix.MultiplyVector(sourceMesh.normals[i]);
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.RecalculateBounds();
    }

    public Matrix4x4 GetTransform(float distance){
        if(curveRadius == 0f) return Matrix4x4.identity;
        float angle = distance / curveRadius;
        return (
            postTransform *
            Matrix4x4.Rotate(Quaternion.AngleAxis(-angle * Mathf.Rad2Deg, new Vector3(1f, 0f, 0f))) *
            preTransform
        );
    }
}
