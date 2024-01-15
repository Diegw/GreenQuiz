using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable] public class QuestionData
{
    public QuestionData()
    {
        _questionName = "Question";
    }
    public Question Question => _question;

    private string _questionName;
    [FoldoutGroup("@_questionName"), SerializeField, HideLabel, OnValueChanged(nameof(SetQuestionName),true)] 
    private Question _question;

    public void SetQuestionName()
    {
        _questionName = $"Question {_question.ID+1}";
    }
}
