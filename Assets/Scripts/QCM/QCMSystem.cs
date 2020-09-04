using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class QCMSystem : MonoBehaviour
{
    public UnityEvent OnProceed;

    #region Private Variables
    private QCMCollection qcmList;
    [SerializeField] private GameObject quizPrefab;
    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private Transform questionHolder;
    [SerializeField] private TextMeshProUGUI result;
    private GameObject[] quiz;
    private int currentQuestion = 0;
    private GameObject tempObjHolder;
    private TransitionManager transition;
    #endregion

    #region Unity Callbacks
    protected virtual void Start()
    {
        transition = TransitionManager.Instance;
    }
    #endregion

    #region Event Functions
    public void ApplyQCM(QCMCollection qcm)
    {
        qcmList = qcm;
    }

    public void GenerateQCM()
    {
        if (qcmList == null)
            return;

        quiz = new GameObject[qcmList.questions.Length];

        for(int i = 0; i < qcmList.questions.Length; i++)
        {
            var question = qcmList.questions[i];

            var _quiz = Instantiate(quizPrefab, quizPrefab.transform.position, quizPrefab.transform.rotation);
            _quiz.transform.SetParent(questionHolder);
            var rect = _quiz.GetComponent<RectTransform>();
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;
            quiz[i] = _quiz;

            if (i != 0)
                _quiz.SetActive(false);
            else
                _quiz.SetActive(true);

            var questionArea = _quiz.transform.Find("Question.Holder").GetComponent<TextMeshProUGUI>();
            questionArea.text = question.question;

            var answerHolder = _quiz.transform.Find("Answer.Holder/ScrollView/Viewport/Content");

            for (int selectables = 0; selectables < question.answers.Length; selectables++)
            {
                string answer = question.answers[selectables];
                var ans = Instantiate(answerPrefab, answerPrefab.transform.position, answerPrefab.transform.rotation);
                ans.transform.SetParent(answerHolder);
                ans.SetActive(true);

                var ansText = ans.transform.Find("Answer.Text").GetComponent<TextMeshProUGUI>();
                ansText.text = answer;

                var ansButton = ans.transform.Find("Selection.Button");
                ansButton.GetComponentInChildren<TextMeshProUGUI>().text = (selectables + 1).ToString();
                var button = ansButton.GetComponent<Button>();
                button.name = selectables.ToString();
                button.onClick.AddListener(() => { OnSelectAnswer(answerHolder, button); });

            }
        }
    }

    public void ClearQCM()
    {
        if (quiz == null)
            return;

        foreach(var question in quiz)
        {
            //TODO: Apply object-pooling and reuse the already created gameobjects(questions) 
            Destroy(question);
        }
    }

    private void OnSelectAnswer(Transform parent, Button button)
    {
        qcmList.questions[currentQuestion].playerSelection = int.Parse(button.name);

        foreach(Transform _button in parent)
        {
            var buttonImage = _button.transform.GetChild(0).GetComponent<Image>();
            buttonImage.color = Color.white;
        }

        button.image.color = Color.green;
    }

    public void Next()
    {
        if ((currentQuestion + 1) >= qcmList.questions.Length)
            return;

        quiz[currentQuestion].SetActive(false);
        currentQuestion++;
        quiz[currentQuestion].SetActive(true);
    }

    public void Back()
    {
        if ((currentQuestion - 1) < 0)
            return;

        quiz[currentQuestion].SetActive(false);
    
        if(currentQuestion!=0)
            currentQuestion--;
        
        quiz[currentQuestion].SetActive(true);
    }

    public void GetResult()
    {
        int totalQuestions = qcmList.questions.Length;
        int correctAnswers = 0;

        foreach(var answer in qcmList.questions)
        {
            if (answer.IsCorrect())
                correctAnswers++;
        }

        result.text = "Result: " + correctAnswers + " / " + totalQuestions;
    }

    public void OnRetry()
    {
        do
        {
            Back();
        }
        while (currentQuestion != 0);
    }

    public void Proceed()
    {
        OnProceed?.Invoke();
    }

    public void Proceed(bool value)
    {
        OnProceed.AddListener(() =>
        {
            transition.gameObject.SetActive(true);
            transition.FadeIn(value);
            OnProceed.RemoveAllListeners();
        });
    }

    public void SetGameObject(GameObject obj)
    {
        tempObjHolder = obj;
    }

    public GameObject GetGameObject()
    {
        return tempObjHolder;
    }
    #endregion
}
