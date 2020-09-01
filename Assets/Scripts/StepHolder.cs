using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepHolder : MonoBehaviour
{
    private TransitionManager transition;
    private StepsManager stepsManager;
    [SerializeField] GameObject[] objToEnable;

    private void Start()
    {
        transition = TransitionManager.Instance;
        stepsManager = StepsManager.Instance;
    }

    public void Scenario1Step1(GameObject eMailPanel)
    {
        Debug.LogWarning("***Scenario1 Step1***");

        transition.OnFadeOut.AddListener(() => 
        {
            eMailPanel.SetActive(true);
            transition.ClearFadeOutListeners();
        });
    }

    public void Scenario1Step2(AudioMediaPlayer mediaPlayer)
    {
        Debug.LogWarning("***Scenario1 Step2***");

        mediaPlayer.OnProceed.AddListener(() =>
        {
            transition.gameObject.SetActive(true);
            transition.FadeIn();
            transition.OnFadeIn.AddListener(() =>
            {
                foreach (var obj in objToEnable)
                { obj.SetActive(true); }
                transition.ClearFadeInListeners();
            });
        });
    }

    public void Scenario1Step3(GameObject QCM)
    {
        transition.OnFadeIn.AddListener(() =>
        {
            foreach (var obj in objToEnable)
            { obj.SetActive(false); }
            transition.ClearFadeInListeners();
        });

        transition.OnFadeOut.AddListener(() =>
        {
            QCM.SetActive(true);
            transition.ClearFadeOutListeners();
        });
    }
}
