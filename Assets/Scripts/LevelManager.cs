using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour {
    [SerializeField] private PipeSystem pipeSystem;
    [SerializeField] private PlayerController player;
    [SerializeField] private EnemyManager enemy;
    [SerializeField] private Slider levelProgressBar;
    [SerializeField] private Text levelText;

    [Header("Level")]
    [SerializeField] private float startingDistance;
    [SerializeField] private float addedDistance;

    public UnityEvent<int> OnLevelCompleted = new UnityEvent<int>();
    public UnityEvent OnGameOver = new UnityEvent();
    private int level;
    private float distanceOffset;

    public void StartGame(){
        level = 0;
        distanceOffset = 0f;
        levelText.text = $"Level {level}";

        pipeSystem.gameObject.SetActive(true);
        player.enabled = true;
        player.transform2d.position = Vector2.zero;

        player.health.OnChanged.AddListener(OnHealthChange);
        Time.timeScale = 1f;
    }

    private void OnHealthChange(){
        if(player.health.value > 0 || !player.enabled) return;
        player.enabled = false;
        StartCoroutine(GameOver());
    }
    
    void Update(){
        if(!player.enabled) return;
        float travelledDistance = pipeSystem.distance - distanceOffset;
        float targetDistance = startingDistance + level * addedDistance;
        levelProgressBar.value = Mathf.Clamp01(travelledDistance / targetDistance);

        if(travelledDistance >= targetDistance) CompleteLevel();
    }

    private void CompleteLevel(){
        OnLevelCompleted.Invoke(level);
        player.enabled = false;
        Time.timeScale = 0f;
    }

    public void NextLevel(){
        level++;
        levelText.text = $"Level {level}";
        player.enabled = true;
        Time.timeScale = 1f;
        distanceOffset = pipeSystem.distance;

        player.health.Refresh();
    }

    private IEnumerator GameOver(){
        yield return new WaitForSeconds(0.1f);
        pipeSystem.gameObject.SetActive(false);

        OnGameOver.Invoke();
    }
}
