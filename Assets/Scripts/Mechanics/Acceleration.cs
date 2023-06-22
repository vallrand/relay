using UnityEngine;
using UnityEngine.Events;

public class Acceleration : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] public int levels;
    [SerializeField] public float minVelocity;
    [SerializeField] public float maxVelocity;
    [SerializeField] public AnimationCurve velocityCurve;

    public UnityEvent OnChange = new UnityEvent();

    public float Percent => (float) sublevel / GetNthFibbonaciNumber(2 + level);
    public int level { get; private set; }
    private int sublevel;
    public float Fraction => (float) level / (levels - 1);
    public float Velocity => 
    minVelocity + maxVelocity * velocityCurve.Evaluate(Fraction);

    void OnEnable(){
        level = sublevel = 0;
        OnChange.Invoke();
    }
    public void LevelUp(){
        sublevel++;
        if(sublevel >= GetNthFibbonaciNumber(2 + level)){
            sublevel = 0;
            level++;
        }
        OnChange.Invoke();
    }
    public void Reset(){
        sublevel = 0;
        level = Mathf.Max(0, level - 1);
        OnChange.Invoke();
    }

    //Taken from https://github.com/iamrajiv/Nth-Fibonacci
    public static int GetNthFibbonaciNumber(int n){
        int[] lastTwo = {0, 1};
		int counter = 3;
		while (counter <= n) {
			int nextFib = lastTwo[0] + lastTwo[1];
			lastTwo[0] = lastTwo[1];
			lastTwo[1] = nextFib;
			counter++;
		}
		return n > 1 ? lastTwo[1] : lastTwo[0];
    }
}
