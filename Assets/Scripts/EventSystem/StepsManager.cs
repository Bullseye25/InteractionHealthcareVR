using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StepsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Step> steps = new List<Step>();
    //[SerializeField] private TextMeshPro description3DText;

    [Header("Settings")]
    [SerializeField] private bool autoStart = true;

    private int currentStep = -1;

    public static StepsManager Instance;

    [SerializeField] private bool grabActiveSteps;

    private void Awake()
    {
        GrabActiveSteps();
        Instance = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);
        if (autoStart)
            NextStep();
    }

    [ContextMenu("Next Step")]
    public void NextStep()
    {
        if(currentStep >= steps.Count)
        {
            Debug.LogWarning($"There is no step {currentStep}. There is only {steps.Count} steps.");
            return;
        }

        if(currentStep != -1)
            steps[currentStep].OnStepEnd?.Invoke();

        currentStep++;
        Debug.LogWarning($"Current step is now Step {currentStep}.");

        if (currentStep < steps.Count)
        {
            steps[currentStep].OnStepStart?.Invoke();
            //description3DText.text = steps[currentStep].description;
        }
    }

    public void ResetSteps()
    {
        currentStep = -1;
        NextStep();
    }

    private void GrabActiveSteps()
    {
        if(grabActiveSteps == true)
        foreach(Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                steps.Add(child.GetComponent<Step>());
            }
        }
    }
}
