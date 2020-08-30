using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

[System.Serializable]
public class QuestionEntity
{
    #region Public Variables
    public int id;
    public string question;
    public string[] answers;
    public int correctAnswer;
    public int playerSelection;
    #endregion

    #region Constructor
    public QuestionEntity(int id, string question, string[] answers, int correctAnswer)
    {
        this.id = id;
        this.question = question;
        this.answers = answers;
        this.correctAnswer = correctAnswer;
        this.playerSelection = -1;

        if(correctAnswer > answers.Length)
            Debug.LogError("Error: Problem with question entity");
    }
    #endregion

    #region Helping Functions
    public bool IsCorrect()
    {
        if (playerSelection == correctAnswer)
            return true;
        return false;
    }
    #endregion

}
