using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour {
    public UnityEvent OnStartGame = new UnityEvent();

    public void OpenMenu(){
        gameObject.SetActive(true);
    }
    public void Play(){
        gameObject.SetActive(false);
        OnStartGame.Invoke();
    }
    public void Exit(){
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
