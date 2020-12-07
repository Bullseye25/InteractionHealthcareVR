using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitThenDo : MonoBehaviour
{
    [SerializeField] private UnityEngine.Events.UnityEvent OnWaitEnd;

    private float currentWaitTime = 0;
    private bool isStarted = false;

    public void Wait(float _time)
    {
        currentWaitTime = _time;
        isStarted = true;
    }


    private void Update()
    {
        if (isStarted)
        {
            if (currentWaitTime <= 0)
            {
                OnWaitEnd?.Invoke();
                isStarted = false;
            }
            else
                currentWaitTime -= Time.deltaTime;
        }
    }
}
