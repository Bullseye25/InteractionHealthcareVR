using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Step : MonoBehaviour
{
    [Header("Settings")]
    public string description;

    [Header("Events")]
    public UnityEvent OnSceneStart;
    public UnityEvent OnStepStart;
    public UnityEvent OnStepEnd;

    // Start is called before the first frame update
    void Start()
    {
        OnSceneStart?.Invoke();
    }

}
