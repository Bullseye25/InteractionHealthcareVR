using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionEntity
{
    public int id;
    public string question;
    public string[] answers;
    public int correctAnswer;
    public int playerSelection;

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

    public bool IsCorrect()
    {
        if (playerSelection == correctAnswer)
            return true;
        return false;
    }
}
