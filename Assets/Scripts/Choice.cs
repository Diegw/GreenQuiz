using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable] public class Choice : MonoBehaviour
{
    public Choice(){}
    public Choice(Choice choice = null)
    {
        if(choice != null)
        {
            _isCorrect = choice.IsCorrect;
            _description = choice.Description;
        }
    }
    public bool IsCorrect => _isCorrect;
    public string Description => _description;

    private bool _hasToHide = false;
    [SerializeField, HideIf("@_hasToHide")] private bool _isCorrect = false;
    [SerializeField, TextArea(1, 10)] private string _description = "";

    public void SetHide(Choice choice)
    {
        _hasToHide = choice != null && choice != this;
    }
}