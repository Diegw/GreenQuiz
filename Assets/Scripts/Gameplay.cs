using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MEC;

public class Gameplay : MonoBehaviour
{
    public static Action<EDirection> OnSceneFinishedEvent;
    public static Action<float, float> OnTimerChangedEvent;
    // public static Action<Question, Choice> OnChoiceSelectedEvent;
    public static Action OnMatchEndedEvent;

    [Title("Timer")]
    [TabGroup("DEBUG"), SerializeField] private bool _hasMatchEnded = false;
    [Title("Timer")]
    [TabGroup("DEBUG"), SerializeField] private float _timer = 0f;
    [TabGroup("DEBUG"), SerializeField] private float _maxTime = 1f;
    [Title("Questions")]
    [TabGroup("DEBUG"), SerializeField] private int _totalQuestions = 0;
    [TabGroup("DEBUG"), SerializeField] private int _questionsAnswered = 0;
    [TabGroup("DEBUG"), SerializeField] private List<Question> _questions = new List<Question>();
    [Title("Current Question")]
    [TabGroup("DEBUG"), HideLabel, SerializeField] private Question _question = null;
    private CoroutineHandle _timerCoroutine = default;
    private SettingsQuestion _settingsQuestion;

#region INITIALIZE
    private void Awake()
    {
        _settingsQuestion = SettingsManager.Question;
        // _maxTime = _settingsQuestion.TimePerQuestion;
        // _totalQuestions = _settingsQuestion.QuestionsPerMatch;
        // _questions = _settingsQuestion.GetQuestions(GameManager.Category, GameManager.Mode, GameManager.Course);
        UpdateGameplay();
        ResetTimer();
        
    }

    private void OnEnable()
    {
        GameplayUI.OnChoicePressedEvent += SelectChoice;
    }

    private void OnDisable()
    {
        GameplayUI.OnChoicePressedEvent -= SelectChoice;
    }

    private void OnDestroy()
    {
        Timing.KillCoroutines(_timerCoroutine);
    }
#endregion

#region TIMER
    private void ResetTimer()
    {
        if(HasMatchEnded())
        {
            return;
        }
        SetTimer(_maxTime);

        Timing.KillCoroutines(_timerCoroutine);
        _timerCoroutine = Timing.RunCoroutine(Timer_Coroutine());
    }

    private IEnumerator<float> Timer_Coroutine()
    {
        while(_timer > 0 && !_hasMatchEnded)
        {
            yield return Timing.WaitForSeconds(1f);
            SetTimer(_timer - 1);
        }
        if(!_hasMatchEnded)
        {
            SelectChoice(-1);
        }
    }

    private void SetTimer(float value)
    {
        _timer = value;
        OnTimerChangedEvent?.Invoke(value, _maxTime);
    }

#endregion

#region QUESTIONS
    private void SelectChoice(int choiceIndex)
    {
        if(HasMatchEnded())
        {
            Timing.KillCoroutines(_timerCoroutine);
            return;
        }
        _questionsAnswered++;
        ChoiceSelectedEvent(choiceIndex);
        UpdateGameplay();
        ResetTimer();
    }

    private void ChoiceSelectedEvent(int choiceIndex)
    {
        // Choice choice = new Choice();
        // if(_question != null && _question.Choices != null && choiceIndex >= 0  && choiceIndex < _question.Choices.Count)
        // {
        //     choice = _question.Choices[choiceIndex];
        // }
        // OnChoiceSelectedEvent?.Invoke(_question, choice);
    }

    private void UpdateGameplay()
    {
        if(HasMatchEnded())
        {
            return;
        }
        if(_questions == null || _questions.Count <= 0)
        {
            Debug.LogError("Couldnt show because Questions List is empty");
            return;
        }
        if(_questionsAnswered >= _questions.Count)
        {
            Debug.LogError($"Couldnt show question ({_questionsAnswered+1}) because Questions List doesnt has question index ({_questionsAnswered})");
            return;
        }
        _question = new Question(_questions[_questionsAnswered]);

        if(_question.Choices == null)
        {
            return;
        }
        // _categoryImage.sprite = _settingsQuestion.GetCategorySprite(GameManager.Category);
        // _questionNumber.text = $"Pregunta {_questionsAnswered+1}/{_totalQuestions}";
        // _questionDescription.text = _question.Description;
        // for (int i = 0; i < _choiceButtons.Count; i++)
        // {
        //     if(i < _question.Choices.Count)
        //     {
        //         _choiceButtons[i].Display.text = _question.Choices[i].Description;
        //     }
        // }
    }
#endregion

    private bool HasMatchEnded()
    {
        if(_hasMatchEnded)
        {
            return true;
        }
        if(_questionsAnswered >= _totalQuestions)
        {
            _hasMatchEnded = true;
            OnMatchEndedEvent?.Invoke();
            return true;
        }
        return false;
    }
}