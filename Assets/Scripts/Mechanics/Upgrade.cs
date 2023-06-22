using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Gameplay/Upgrade")]
public class Upgrade : ScriptableObject {
    [System.Serializable]
    public enum UpgradeType {
        Linear,
        Inverse
    };

    [SerializeField] public string title;
    [SerializeField, TextArea] public string description;
    [SerializeField] public UpgradeType type;
    [SerializeField] public float startingValue;
    [SerializeField] public float addedValue;
    [SerializeField] public float inverseCoefficent;
    [Header("Requirements")]
    [SerializeField] public int minLevel;

    public UnityEvent<int> OnChanged = new UnityEvent<int>();

    [HideInInspector] public int quantity;
    public float CalculateMultiplier(){
        switch(type){
            case UpgradeType.Linear:
                return startingValue + addedValue * quantity;
            case UpgradeType.Inverse:
                float value = startingValue + addedValue * quantity;
                return inverseCoefficent / (inverseCoefficent + value);
            default: return 0f;
        }
    }

    public void Reset(){
        quantity = 0;
        OnChanged.Invoke(quantity);
    }
    public void LevelUp(){
        quantity++;
        OnChanged.Invoke(quantity);
    }
}
