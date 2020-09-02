using UnityEngine;
using UnityEngine.Playables;

public class StepHolder : MonoBehaviour
{
    private TransitionManager transition;
    private StepsManager stepsManager;
    [SerializeField] GameObject patient;

    private void Start()
    {
        transition = TransitionManager.Instance;
        stepsManager = StepsManager.Instance;
    }

    public void Scenario1Step1(GameObject eMailPanel)
    {
        transition.OnFadeOut.AddListener(() => 
        {
            eMailPanel.SetActive(true);
            transition.ClearFadeOutListeners();
        });
    }

    public void Scenario1Step2(AudioMediaPlayer mediaPlayer)
    {
        mediaPlayer.OnProceed.AddListener(() =>
        {
            transition.gameObject.SetActive(true);
            transition.FadeIn();
            transition.OnFadeIn.AddListener(() =>
            {
                patient.SetActive(true);
                transition.ClearFadeInListeners();
            });

            mediaPlayer.ClearProceed();
        });
    }

    public void Scenario1Step3(AudioMediaPlayer mediaPlayer)
    {
        mediaPlayer.OnProceed.AddListener(() =>
        {
            var pd = FindObjectOfType<PlayableDirector>();
            pd.GetComponent<BoxCollider>().enabled = true;
            mediaPlayer.ClearProceed();
        });
    }

    public void Scenario1Step4(QCMSystemExt QCM)
    {
        transition.OnFadeIn.AddListener(() =>
        {
            patient.SetActive(false); 
            transition.ClearFadeInListeners();
        });

        transition.OnFadeOut.AddListener(() =>
        {
            QCM.OnProceed.AddListener(() => 
            {
                transition.gameObject.SetActive(true);
                transition.FadeIn();
                transition.OnFadeIn.AddListener(() => 
                {
                    var ExamReport = transform.parent.transform.Find("Report.Panel.2");
                    ExamReport.gameObject.SetActive(true);
                });
                QCM.OnProceed.RemoveAllListeners();
            });

            QCM.gameObject.SetActive(true);
            transition.ClearFadeOutListeners();
        });
    }
}
