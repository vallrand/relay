using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UpgradeOption : MonoBehaviour {
    [SerializeField] private Text title;
    [SerializeField] private Text description;
    [SerializeField] private Button button;
    private Upgrade upgrade = null;

    public UnityEvent<Upgrade> OnUpgradeSelected = new UnityEvent<Upgrade>();

    public void SetUpgrade(Upgrade upgrade){
        this.upgrade = upgrade;
        title.text = $"{upgrade.title} -> {upgrade.quantity}";
        description.text = upgrade.description;
    }

    public void SelectUpgrade(){
        if(!upgrade) return;
        OnUpgradeSelected.Invoke(upgrade);
    }
}
