using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyIntEvent : UnityEvent<int>
{
}

public class LevelTimer : MonoBehaviour, ITimer
{
    public static LevelTimer Instance { get; private set; }

    public int maxTime;
    protected int remainingTime;

    public UnityEvent started = new UnityEvent();
    public UnityEvent stopped = new UnityEvent();
    public MyIntEvent ticked = new MyIntEvent();

    public bool IsRunning { get; private set;}

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator tick()
    {
        while (IsRunning)
        {
            yield return new WaitForSecondsRealtime(1);

            remainingTime--;

            ticked.Invoke(remainingTime);

            if (remainingTime == 0)
            {
                stop();
            }
        }
    }

    public void start()
    {
        IsRunning = true;

        StartCoroutine(tick());

        started.Invoke();
    }

    public void stop()
    {
        IsRunning = false;

        StopCoroutine(tick());

        stopped.Invoke();
    }

    public void reset()
    {
        remainingTime = maxTime;
    }
}
