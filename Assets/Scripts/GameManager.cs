using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Private Variables
    private StepsManager stepsManager;

    [SerializeField] private string[] scenarioTag;
    #endregion

    #region Public Variables
    public static GameManager Instance;
    #endregion

    #region Unity Callbacks
    private void Awake() 
    {
        Instance = this;
    }

    private void Start()
    {
        stepsManager = StepsManager.Instance;
    }
    #endregion

    #region Event Functions

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
    #endregion

}
