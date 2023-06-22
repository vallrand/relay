using UnityEngine;
using UnityEngine.Events;

public class UpgradeScreen : MonoBehaviour {
    [SerializeField] private Upgrade[] upgrades;
    [SerializeField] private UpgradeOption[] options;

    public UnityEvent OnClose = new UnityEvent();

    public void OnGameStart(){
        foreach(var upgrade in upgrades) upgrade.Reset();
    }

    private void SelectUpgrade(Upgrade upgrade){
        upgrade.LevelUp();

        foreach(var option in options) option.OnUpgradeSelected.RemoveAllListeners();
        gameObject.SetActive(false);
        OnClose.Invoke();
    }
    public void Open(int level){
        foreach(var option in options) option.OnUpgradeSelected.AddListener(SelectUpgrade);
        gameObject.SetActive(true);

        Shuffle(upgrades);
        for(int i = 0, j = 0; i < upgrades.Length && j < options.Length; i++){
            var upgrade = upgrades[i];
            if(level < upgrade.minLevel) continue;
            options[j].SetUpgrade(upgrade);
            j++;
        }
    }

    private static void Shuffle<T>(T[] array){
        for(int i = array.Length - 1; i > 0; i--){
            int j = Random.Range(0, i);
            T temp = array[j];
            array[j] = array[i - 1];
            array[i - 1] = temp;
        }
    }
}
