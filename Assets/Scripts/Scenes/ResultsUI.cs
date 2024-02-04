using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class ResultsUI : SerializedMonoBehaviour
{
    public static Action OnContinueButtonPressedEvent;

    [TabGroup("REFERENCES"), SerializeField] private RectTransform _UI = null;
    [TabGroup("REFERENCES"), SerializeField] private TMP_Text _titleDisplay = null;
    [TabGroup("REFERENCES"), SerializeField] private TMP_Text _stadisticsDisplay = null;
    [TabGroup("REFERENCES"), SerializeField] private Image _stadisticsImage = null;
    [TabGroup("REFERENCES"), SerializeField] private Image _categoryImage = null;
    [TabGroup("REFERENCES"), SerializeField] private ButtonCustom _linkPrincipal = null;
    [TabGroup("REFERENCES"), SerializeField] private ButtonCustom _linkSecondary1 = null;
    [TabGroup("REFERENCES"), SerializeField] private ButtonCustom _linkSecondary2 = null;
    [TabGroup("REFERENCES"), SerializeField] private ButtonCustom _continueButton = null;
    [SerializeField] private Dictionary<EMenuCategory, Animator> _animators = new Dictionary<EMenuCategory, Animator>();
    private EMenuCourse _secondaryCourse1 = EMenuCourse.NONE;
    private EMenuCourse _secondaryCourse2 = EMenuCourse.NONE;
    
    private void Awake()
    {
        // _settingsQuestion = SettingsManager.Question;
        if(isThereNullReference())
        {
            return;
        }
        ToggleResultsUI(false);
    }

    private void OnEnable()
    {
        Results.OnResultsDataReadyEvent += ShowResults;
        AddButtonListener(_linkPrincipal, LinkPrincipalButton);
        AddButtonListener(_linkSecondary1, LinkSecondaryButton1);
        AddButtonListener(_linkSecondary2, LinkSecondaryButton2);
        AddButtonListener(_continueButton, ContinueButton);
    }

    private void OnDisable()
    {
        Results.OnResultsDataReadyEvent -= ShowResults;
        RemoveButtonListener(_linkPrincipal, LinkPrincipalButton);
        RemoveButtonListener(_linkSecondary1, LinkSecondaryButton1);
        RemoveButtonListener(_linkSecondary2, LinkSecondaryButton2);
        RemoveButtonListener(_continueButton, ContinueButton);
    }

    private void AddButtonListener(ButtonCustom buttonCustom, UnityAction action)
    {
        if(buttonCustom == null || buttonCustom.Button == null)
        {
            return;
        }
        buttonCustom.Button.onClick.AddListener(action);
    }

    private void RemoveButtonListener(ButtonCustom buttonCustom, UnityAction action)
    {
        if(buttonCustom == null || buttonCustom.Button == null)
        {
            return;
        }
        buttonCustom.Button.onClick.RemoveListener(action);
    }

    private void ShowResults(ResultsData resultsData)
    {
        if(resultsData == null || isThereNullReference())
        {
            return;
        }
        _titleDisplay.text = "Resultados";
        // _categoryImage.sprite = _settingsQuestion.GetCategorySprite(GameManager.Category);
        _stadisticsImage.fillAmount = (float)resultsData.CorrectAnswers / (float)resultsData.TotalQuestions;
        _stadisticsDisplay.text = $"Preguntas correctas {resultsData.CorrectAnswers} de {resultsData.TotalQuestions}";
        _linkPrincipal.Display.text = SettingsManager.Menu.GetCourseName(GameManager.Course);
        _secondaryCourse1 = resultsData.Course1;
        _secondaryCourse2 = resultsData.Course2;
        _linkSecondary1.Display.text = SettingsManager.Menu.GetCourseName(_secondaryCourse1);
        _linkSecondary2.Display.text = SettingsManager.Menu.GetCourseName(_secondaryCourse2);
        SetAnimationCategory();
        ToggleResultsUI(true);
    }

    private void LinkPrincipalButton()
    {
        Application.OpenURL(SettingsManager.Question.GetCourseUrl(GameManager.Course));
        AudioManager.PlaySfx();
    }
    
    private void LinkSecondaryButton1()
    {
        Application.OpenURL(SettingsManager.Question.GetCourseUrl(_secondaryCourse1));
        AudioManager.PlaySfx();
    }

    private void LinkSecondaryButton2()
    {
        Application.OpenURL(SettingsManager.Question.GetCourseUrl(_secondaryCourse2));
        AudioManager.PlaySfx();
    }
    
    private void ContinueButton()
    {
        //@TODO: maybe check something else before continue
        if(isThereNullReference())
        {
            return;
        }
        _continueButton.Button.interactable = false;
        OnContinueButtonPressedEvent?.Invoke();
        AudioManager.PlaySfx();
    }

    private void ToggleResultsUI(bool stateToSet)
    {
        if(_UI == null || _UI.gameObject.activeSelf == stateToSet)
        {
            return;
        }
        _UI.gameObject.SetActive(stateToSet);
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

    private bool isThereNullReference()
    {
        // if(_settingsQuestion == null)
        // {
        //     Debug.LogError("Settings is null");
        //     return true;
        // }
        if(_UI == null)
        {
            Debug.LogError("Results UI isnt assign");
            return true;
        }
        if(_titleDisplay == null)
        {
            Debug.LogError("Results Title isnt assign");
            return true;
        }
        if(_categoryImage == null)
        {
            Debug.LogError("Results Category Image isnt assign");
            return true;
        }
        if(_stadisticsDisplay == null)
        {
            Debug.LogError("Results Stadistics Display isnt assign");
            return true;
        }
        if(_stadisticsImage == null)
        {
            Debug.LogError("Results Stadistics Image isnt assign");
            return true;
        }
        if(_linkPrincipal == null)
        {
            Debug.LogError("Results Link Princiapl isnt assign");
            return true;
        }
        if(_continueButton == null || _continueButton.Button == null)
        {
            Debug.LogError("Results Continue Button isnt assign");
            return true;
        }
        return false;
    }
}
