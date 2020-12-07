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
        CHECK_ICON = "Check.Icon",
        ANSWERS_HOLDER_PATH = "Answer.Holder/ScrollView/Viewport/Content",
        SELECTION_BUTTON_IMAGE = "Button.Image";

    private QCMCollection qcmEntity;
    [SerializeField] private GameObject quizPrefab, answerPrefab, suivant, close;
    [SerializeField] protected Transform questionHolder;
    [SerializeField] private Sprite defaultButtonSprite, SelectionButtonSprite, CorrectAnswerSprite;
    [SerializeField] private Color valadtionColor;
    private GameObject[] quizzes;
    protected int currentQuestion = 0;
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

        for (int i = 0; i < qcmEntity.questions.Length; i++)
        {
            #region Create Quiz Prefab
            var questionEntity = qcmEntity.questions[i];
            questionEntity.playerSelection = -1;
            questionEntity.ManageAnswers();
            var quiz = Instantiate(quizPrefab, quizPrefab.transform.position, quizPrefab.transform.rotation);
            quiz.transform.SetParent(questionHolder);
            var defaultRect = quizPrefab.GetComponent<RectTransform>();
            var rect = quiz.GetComponent<RectTransform>();
            rect = defaultRect;
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
                _answerPrefab.transform.GetComponent<Button>().onClick.AddListener(() => { OnSelectAnswer(_answerPrefab.transform); });
                #endregion
            }
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

    private void OnSelectAnswer(Transform parent)
    {

        var selectionImg = parent.Find(SELECTION_BUTTON_IMAGE).GetComponent<Image>();
        var _ans = parent.Find(ANSWER_TEXT_TITLE).GetComponent<TextMeshProUGUI>();

        if (selectionImg.sprite != SelectionButtonSprite)
        {
            selectionImg.sprite = SelectionButtonSprite;
            _ans.fontStyle = FontStyles.Bold;
        }
        else
        {
            selectionImg.sprite = defaultButtonSprite;
            _ans.fontStyle = FontStyles.Normal;
        }
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

        foreach (Transform answer in answersHolder)
        {
            answer.GetComponent<Button>().interactable = false;
        }

        var correctAnsHolder = answersHolder.GetChild(correctAnswerIndex);

        correctAnsHolder.GetComponent<Button>().transition = Selectable.Transition.ColorTint;
        correctAnsHolder.Find(SELECTION_BUTTON_IMAGE).GetComponent<Image>().sprite = CorrectAnswerSprite;
        correctAnsHolder.Find(SELECTION_BUTTON_IMAGE).GetComponent<Image>().color = valadtionColor;
        correctAnsHolder.Find(ANSWER_TEXT_TITLE).GetComponent<TextMeshProUGUI>().color = Color.white;
        correctAnsHolder.Find(ANSWER_TEXT_TITLE).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        correctAnsHolder.Find(CHECK_ICON).gameObject.SetActive(true);
		correctAnsHolder.Find("Bullet").gameObject.SetActive(false);

		if ((currentQuestion + 1) >= qcmEntity.questions.Length)
        {
            close.SetActive(true);
        }
        else
        {
            suivant.SetActive(true);
        }
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
