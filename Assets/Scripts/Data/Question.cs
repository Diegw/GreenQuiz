using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable] public class Question
{
    public Question(){}
    public Question(Question question = null)
    {
        if(question != null)
        {
            _ID = question.ID;
            _description = question.Description;
            _choices = new List<Choice>(question.Choices);
        }
    }
    public int ID => _ID;
    public string Description => _description;
    public List<Choice> Choices => _choices;

    [SerializeField] private int _ID = -1;
    [SerializeField, TextArea(1, 10)] private string _description = "";
    [SerializeField, OnValueChanged(nameof(SetHide),true)] private List<Choice> _choices = new List<Choice>();
    
    public void SetID(int newID)
    {
        _ID = newID;
    }

    private void SetHide()
    {
        Choice correctChoice = _choices.Find(choice => choice.IsCorrect);
        foreach (Choice choice in _choices)
        {
            choice.SetHide(correctChoice);
        }
    }
}