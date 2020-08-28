using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private StepsManager stepsManager;

    public static GameManager Instance;

    [SerializeField]
    private string[] scenarioTag;

    private void Awake() 
    {
        Instance = this;
    }

    private void Start()
    {
        stepsManager = StepsManager.Instance;
    }

    public void ChooseScenario(string scenarioID)
    {
        foreach(Transform child in stepsManager.transform)
        {
            if(child.tag == scenarioID)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
