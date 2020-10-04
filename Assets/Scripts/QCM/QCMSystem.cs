using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class QCMSystem : MonoBehaviour
{
    public UnityEvent OnProceed;

    #region Private Variables
    private const string
        QUESTION_TEXT_TITLE = "Question.Holder",
        ANSWER_TEXT_TITLE = "Answer.Text",
        SELECTION_BUTTON_TITLE = "Selection.Button",
        ANSWERS_HOLDER_PATH = "Answer.Holder/ScrollView/Viewport/Content";

    private QCMCollection qcmEntity;
    [SerializeField] private GameObject quizPrefab, answerPrefab, next, back;
    [SerializeField] protected Transform questionHolder;
    [SerializeField] private Sprite defaultButtonSprite, SelectionButtonSprite;
    private GameObject[] quizzes;
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
    public void ApplyQCM(QCMCollection _qcmEntity)
    {
        qcmEntity = _qcmEntity;
    }

    public void GenerateQCM()
    {
        if (qcmEntity == null)
            return;

        quizzes = new GameObject[qcmEntity.questions.Length];

        for(int i = 0; i < qcmEntity.questions.Length; i++)
        {
            #region Create Quiz Prefab
            var questionEntity = qcmEntity.questions[i];
            questionEntity.playerSelection = -1;
            var quiz = Instantiate(quizPrefab, quizPrefab.transform.position, quizPrefab.transform.rotation);
            quiz.transform.SetParent(questionHolder);
            var rect = quiz.GetComponent<RectTransform>();
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;
            quizzes[i] = quiz;
            quiz.SetActive(i == 0 ? true : false);
            #endregion

            #region Apply Question
            var question = quiz.transform.Find(QUESTION_TEXT_TITLE).GetComponent<TextMeshProUGUI>();
            question.text = questionEntity.question;
            #endregion

            var answersHolder = quiz.transform.Find(ANSWERS_HOLDER_PATH);

            for (int index = 0; index < questionEntity.answers.Length; index++)
            {
                #region Create Answer Prefab
                string answerEntity = questionEntity.answers[index];
                var _answerPrefab = Instantiate(answerPrefab, answerPrefab.transform.position, answerPrefab.transform.rotation);
                _answerPrefab.transform.SetParent(answersHolder);
                _answerPrefab.SetActive(true);
                #endregion

                #region Apply Answer
                var answer = _answerPrefab.transform.Find(ANSWER_TEXT_TITLE).GetComponent<TextMeshProUGUI>();
                answer.text = answerEntity;
                #endregion

                #region Prepare the Selection Button
                var selectionButton = _answerPrefab.transform.Find(SELECTION_BUTTON_TITLE);
                selectionButton.GetComponentInChildren<TextMeshProUGUI>().text = (index + 1).ToString();
                selectionButton.name = index.ToString();
                selectionButton.parent.GetComponent<Button>().onClick.AddListener(() => { OnSelectAnswer(answersHolder, selectionButton); });
                #endregion
            }
        }


        if (quizzes.Length > 1)
        {
            back.SetActive(true);
            next.SetActive(true);
        }
    }

    public void ClearQCM()
    {
        if (quizzes == null)
            return;

        foreach(var question in quizzes)
        {
            //TODO: Apply object-pooling and reuse the already created gameobjects(questions) 
            Destroy(question);
        }
    }

    private void OnSelectAnswer(Transform parent, Transform button)
    {
        //Update QCM with player selection
        //qcmEntity.questions[currentQuestion].playerSelection = int.Parse(button.name);

        //foreach(Transform _button in parent)
        //{
        //    var buttonImage = _button.GetChild(0).GetComponent<Image>();
        //    buttonImage.color = defaultButton;
        //}

        button.transform.parent.GetChild(0).GetComponent<Image>().sprite = SelectionButtonSprite;
    }

    public void Next()
    {
        if ((currentQuestion + 1) >= qcmEntity.questions.Length)
            return;

        quizzes[currentQuestion].SetActive(false);
        currentQuestion++;
        quizzes[currentQuestion].SetActive(true);
    }

    public void Back()
    {
        if ((currentQuestion - 1) < 0)
            return;

        quizzes[currentQuestion].SetActive(false);
    
        if(currentQuestion!=0)
            currentQuestion--;
        
        quizzes[currentQuestion].SetActive(true);
    }

    public void GetResult()
    {
        var correctAnswerIndex = qcmEntity.questions[currentQuestion].correctAnswer;

        var question = quizzes[currentQuestion];

        var answersHolder = question.transform.Find(ANSWERS_HOLDER_PATH);

        foreach(Transform answer in answersHolder)
        {
            answer.GetChild(0).GetComponent<Image>().sprite = defaultButtonSprite;
        }

        answersHolder.GetChild(correctAnswerIndex).GetChild(0).GetComponent<Image>().sprite = SelectionButtonSprite;
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
