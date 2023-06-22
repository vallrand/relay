using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    [System.Serializable]
    public class EnemyPack {
        public AIController[] prefabs;
        public int weight;
        public int minDifficulty;
        public int maxDifficulty;
        public bool Validate(int difficulty){
            if(minDifficulty > 0 && difficulty < minDifficulty) return false;
            if(maxDifficulty > 0 && difficulty > maxDifficulty) return false;
            return true;
        }
    }

    [SerializeField] private EnemyPack[] packs;
    [SerializeField] private PipeSystem pipeSystem;
    [SerializeField] private PipeTransform playerTransform;
    private int difficulty;

    void OnDisable(){
        difficulty = 0;
    }

    private EnemyPack GetRandomPack(){
        int totalWeight = 0;
        foreach(var pack in packs)
            if(pack.Validate(difficulty))
                totalWeight += pack.weight;
        int randomWeight = Random.Range(0, totalWeight);
        for(int i = 0, weight = 0; i < packs.Length; i++){
            var pack = packs[i];
            if(!pack.Validate(difficulty)) continue;
            weight += pack.weight;
            if(randomWeight < weight) return pack;
        }
        return null;
    }

    public void SpawnEnemies(DeformArc section){
        difficulty++;
        var pack = GetRandomPack();
        if(pack == null) return;
        foreach(var prefab in pack.prefabs){
            var instance = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            instance.target = playerTransform;
            instance.GetComponent<PipeTransform>().position = new Vector2(
                Random.Range(0f, 2f * Mathf.PI * pipeSystem.innerRadius),
                section.offset + Random.Range(0f, section.Length)
            );
        }
    }
}
