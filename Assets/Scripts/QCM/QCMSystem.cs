using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QCMSystem : MonoBehaviour
{
    [SerializeField] private QuestionEntity[] questions;
    [SerializeField] private GameObject quizPrefab;
    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private Transform questionHolder;
    [SerializeField] private TextMeshProUGUI result;
    private GameObject[] quiz;
    private int currentQuestion = 0;
    /*    public delegate void Demo();
        public Demo demo;*/

    private void GenerateQCM()
    {
        quiz = new GameObject[questions.Length];

        for(int i = 0; i < questions.Length; i++)
        {
            var question = questions[i];

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

/*                if(selectables == 0)
                    demo += () => { OnSelectAnswer(answerHolder, button, selectables); };*/
            }
        }
    }

    private void OnSelectAnswer(Transform parent, Button button)
    {
        questions[currentQuestion].playerSelection = int.Parse(button.name);

        foreach(Transform _button in parent)
        {
            var buttonImage = _button.transform.GetChild(0).GetComponent<Image>();
            buttonImage.color = Color.white;
        }

        button.image.color = Color.green;
    }

    public void Next()
    {
        if ((currentQuestion + 1) >= questions.Length)
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
        int totalQuestions = questions.Length;
        int correctAnswers = 0;

        foreach(var answer in questions)
        {
            if (answer.IsCorrect())
                correctAnswers++;
        }

        result.text = "Result: " + correctAnswers + " / " + totalQuestions;
    }

    protected virtual void Start()
    {
        GenerateQCM();
    }

/*    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Next();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Back();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            demo();

            for (int i =0; i < questions.Length; i++)
            {
                questions[i].playerSelection = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetResult();
        }
    }*/
}
