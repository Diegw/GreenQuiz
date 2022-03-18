using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    public static Action<int> OnChoicePressedEvent;

    [TabGroup("REFERENCES"), SerializeField] private TMP_Text _timerDisplay = null;
    [TabGroup("REFERENCES"), SerializeField] private Image[] _timerImages = null;
    [Title("Question")]
    [TabGroup("REFERENCES"), SerializeField] private Image _categoryImage = null;
    [TabGroup("REFERENCES"), SerializeField] private TMP_Text _questionNumber = null;
    [TabGroup("REFERENCES"), SerializeField] private TMP_Text _questionDescription = null;
    [TabGroup("REFERENCES"), SerializeField] private List<ButtonCustom> _choiceButtons = new List<ButtonCustom>();

    private void OnEnable()
    {
        Gameplay.OnTimerChangedEvent += UpdateTimerUI;
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
        foreach (Image timerImage in _timerImages)
        {
            float fill = time / maxTime;
            timerImage.fillAmount = fill;
        }
    }

    private bool IsThereNullReference()
    {
        if(_timerDisplay == null || _questionNumber == null || _questionDescription == null)
        {
            Debug.LogError("Some Text Reference isnt assign");
            return true;
        }
        if(_categoryImage == null || _timerImages == null || _timerImages.Length <= 0)
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

    private void SendChoice(int choiceIndex)
    {
        OnChoicePressedEvent?.Invoke(choiceIndex);
    }
}