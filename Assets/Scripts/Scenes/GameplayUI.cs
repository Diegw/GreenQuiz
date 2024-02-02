using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class GameplayUI : SerializedMonoBehaviour
{
    public static Action<int> OnChoicePressedEvent;

    [TabGroup("REFERENCES"), SerializeField] private RectTransform _UI = null;
    [TabGroup("REFERENCES"), SerializeField] private TMP_Text _timerDisplay = null;
    [TabGroup("REFERENCES"), SerializeField] private Image _timerImage = null;
    [Title("Question")]
    [TabGroup("REFERENCES"), SerializeField] private Image _categoryImage = null;
    [TabGroup("REFERENCES"), SerializeField] private TMP_Text _questionNumber = null;
    [TabGroup("REFERENCES"), SerializeField] private TMP_Text _questionDescription = null;
    [TabGroup("REFERENCES"), SerializeField] private List<ButtonCustom> _choiceButtons = new List<ButtonCustom>();
    [SerializeField] private Dictionary<EMenuCategory, Animator> _animators = new Dictionary<EMenuCategory, Animator>();

    private void Awake()
    {
        ToggleGameplayUI(true);
        SetAnimationCategory();
    }
    
    private void SetAnimationCategory()
    {
        foreach (var animator in _animators)
        {
            if (animator.Key == EMenuCategory.NONE || animator.Value == null)
            {
                continue;
            }
            animator.Value.gameObject.SetActive(animator.Key == GameManager.Category);
        }
    }

    private void OnEnable()
    {
        Gameplay.OnTimerChangedEvent += UpdateTimerUI;
        Gameplay.OnQuestionChangedEvent += UpdateQuestionUI;
        Gameplay.OnMatchEndedEvent += delegate{ToggleGameplayUI(false);};
        if(IsThereNullReference())
        {
            return;
        }
        for (int i = 0; i < _choiceButtons.Count; i++)
        {
            int index = i;
            _choiceButtons[index].Button.onClick.AddListener(() => SendChoice(index));
        }
    }

    private void OnDisable()
    {
        Gameplay.OnTimerChangedEvent -= UpdateTimerUI;
        Gameplay.OnQuestionChangedEvent -= UpdateQuestionUI;
        Gameplay.OnMatchEndedEvent -= delegate{ToggleGameplayUI(false);};
        if(IsThereNullReference())
        {
            return;
        }
        for (int i = 0; i < _choiceButtons.Count; i++)
        {
            int index = i;
            _choiceButtons[index].Button.onClick.RemoveListener(() => SendChoice(index));
        }
    }

    private void UpdateTimerUI(float time, float maxTime)
    {
        if(IsThereNullReference())
        {
            return;
        }
        _timerDisplay.text = Mathf.Clamp(time, 0, time).ToString();
        float fill = time / maxTime;
        _timerImage.fillAmount = fill;
    }

    private void UpdateQuestionUI(int questionsAnswer, int totalQuestions, Question question)
    {
        if(IsThereNullReference() || question == null)
        {
            return;
        }
        // _categoryImage.sprite = _settingsQuestion.GetCategorySprite(GameManager.Category);
        _questionNumber.text = $"Pregunta {questionsAnswer+1}/{totalQuestions}";
        _questionDescription.text = question.Description;
        for (int i = 0; i < _choiceButtons.Count; i++)
        {
            if(i < question.Choices.Count)
            {
                _choiceButtons[i].Display.text = question.Choices[i].Description;
            }
        }
    }
    
    private void SendChoice(int choiceIndex)
    {
        OnChoicePressedEvent?.Invoke(choiceIndex);
        AudioManager.PlaySfx();
    }

    private void ToggleGameplayUI(bool stateToSet)
    {
        if(_UI == null || _UI.gameObject.activeSelf == stateToSet)
        {
            return;
        }
        _UI.gameObject.SetActive(stateToSet);
    }

    private bool IsThereNullReference()
    {
        if(_timerDisplay == null || _questionNumber == null || _questionDescription == null)
        {
            Debug.LogError("Some Text Reference isnt assign");
            return true;
        }
        if(_categoryImage == null || _timerImage == null)
        {
            Debug.LogError("Some Image Reference isnt assign");
            return true;
        }
        if(_choiceButtons == null || _choiceButtons.Count <= 0)
        {
            Debug.LogError("Some Image Reference isnt assign");
            return true;
        }
        return false;
    }
}