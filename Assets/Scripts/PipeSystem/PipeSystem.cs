using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;

// Inspired by https://catlikecoding.com/unity/tutorials/swirly-pipe/
public class PipeSystem : MonoBehaviour {
    [SerializeField] private DeformArc[] sectionPrefabs;
    [SerializeField] private int visibleSegments;
    [SerializeField] public float distance;
    [SerializeField] private float distanceOffset;
    [SerializeField] public float innerRadius;
    [Header("Randomization")]
    [SerializeField] private float minCurveRadius;
    [SerializeField] private float maxCurveRadius;

    private ObjectPool<DeformArc>[] pools;
    private DeformArc[] sections;
    private int first;
    private int last => (first + sections.Length - 1) % sections.Length;
    public float minDistance => sections[first].offset;
    public float maxDistance => sections[last].offset + sections[last].Length;

    public UnityEvent<DeformArc> OnSectionGenerated = new UnityEvent<DeformArc>();

    void OnEnable(){
        pools = new ObjectPool<DeformArc>[sectionPrefabs.Length];
        for(int i = 0; i < pools.Length; i++){
            var prefab = sectionPrefabs[i];
            prefab.gameObject.SetActive(false);
            prefab.index = i;
            pools[i] = new ObjectPool<DeformArc>(
                () => Instantiate<DeformArc>(prefab, Vector3.zero, Quaternion.identity, transform),
                (instance) => instance.gameObject.SetActive(true),
                (instance) => instance.gameObject.SetActive(false)
            );
        }

        sections = new DeformArc[visibleSegments];
        for(int i = 0; i < visibleSegments; i++)
            sections[i] = GenerateNextSection(i > 0 ? sections[i - 1] : null);
        distance = 0f;
        first = 0;
    }
    void OnDisable(){
        for(int i = 0; i < sections.Length; i++){
            if(sections[i]) pools[sections[i].index].Release(sections[i]);
            sections[i] = null;
        }
        //TODO cleanup
    }

    private DeformArc GenerateNextSection(DeformArc previous){
        var section = pools[Random.Range(0, pools.Length)].Get();

        //TODO use direct mesh API and recalculate in a background job
        if(section.curveRadius == 0f){
            section.curveRadius = Random.Range(minCurveRadius, maxCurveRadius);
            section.rotation = Random.Range(0f, 360f);

            section.GenerateMesh();
        }

        if(previous){
            section.offset = previous.offset + previous.Length;
            var matrix = previous.GetTransform(previous.Length);
            matrix = Matrix4x4.TRS(previous.transform.localPosition, previous.transform.localRotation, previous.transform.localScale) * matrix;
            section.transform.localPosition = matrix.GetPosition();
            section.transform.localRotation = Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
        }

        OnSectionGenerated?.Invoke(section);
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

    public Matrix4x4 GetTransform(Vector2 position){
        return GetTransform(position.y) * 
        Matrix4x4.Rotate(Quaternion.AngleAxis(Mathf.Rad2Deg * position.x / innerRadius, Vector3.forward)) *
        Matrix4x4.Translate(new Vector3(0f, -innerRadius, 0f));
    }

    void Update(){
        if(sections.Length <= first || !sections[first]) return;
        while(sections[first].offset + sections[first].Length <= distance - distanceOffset){

            pools[sections[first].index].Release(sections[first]);
            var section = sections[first] = GenerateNextSection(sections[last]);
            first = (first + 1) % sections.Length;
        }
    }
}
