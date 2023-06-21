using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSystem : MonoBehaviour {
    [SerializeField] private DeformArc[] sectionPrefabs;
    [SerializeField] private int visibleSegments;
    [SerializeField] public float distance;
    [SerializeField] public float innerRadius;
    [Header("Randomization")]
    [SerializeField] private float minCurveRadius;
    [SerializeField] private float maxCurveRadius;

    private DeformArc[] sections;
    private int first;
    private int last => (first + sections.Length - 1) % sections.Length;

    void OnEnable(){
        sections = new DeformArc[visibleSegments];
        for(int i = 0; i < visibleSegments; i++)
            sections[i] = GenerateNextSection(i > 0 ? sections[i - 1] : null);
    }
    void OnDisable(){
        //TODO cleanup
    }

    void OnValidate(){

    }

    private DeformArc GenerateNextSection(DeformArc previous){
        var prefab = sectionPrefabs[Random.Range(0, sectionPrefabs.Length)];

        prefab.gameObject.SetActive(false);
        var section = Instantiate<DeformArc>(prefab, Vector3.zero, Quaternion.identity, transform);
        section.curveRadius = Random.Range(minCurveRadius, maxCurveRadius);
        section.rotation = Random.Range(0f, 360f);
        section.gameObject.SetActive(true);

        if(previous){
            section.offset = previous.offset + previous.Length;
            var matrix = previous.GetTransform(previous.Length);
            matrix = Matrix4x4.TRS(previous.transform.localPosition, previous.transform.localRotation, previous.transform.localScale) * matrix;
            section.transform.localPosition = matrix.GetPosition();
            section.transform.localRotation = Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
        }

        return section;
    }

    public Matrix4x4 GetTransform(float distance){
        //TODO replace for loop with lookup table
        foreach(var section in sections)
            if(section.offset <= distance && distance < section.offset + section.Length){
                var matrix = section.GetTransform(distance - section.offset);
                matrix = Matrix4x4.TRS(section.transform.localPosition, section.transform.localRotation, section.transform.localScale) * matrix;
                return matrix;
            }
        return Matrix4x4.identity;
    }

    public void TransformPosition(Vector2 position, Transform targetTransform){
        //TODO replace for loop with lookup table
        float distance = position.y;
        foreach(var section in sections)
            if(section.offset <= distance && distance < section.offset + section.Length){
                distance -= section.offset;

                float angle = position.x / innerRadius;
                float x = innerRadius * Mathf.Sin(angle);
                float y = innerRadius * Mathf.Cos(angle);

                var matrix = section.GetTransform(distance);
            }
    }

    void Update(){
        if(sections.Length <= first || !sections[first]) return;
        while(sections[first].offset + sections[first].Length <= distance){
            //TODO use object pool
            Destroy(sections[first].gameObject);
            sections[first] = GenerateNextSection(sections[last]);

            first = (first + 1) % sections.Length;
        }
    }
}
