using System;
using UnityEngine;
using UnityEngine.Events;

public class MenuUI : MonoBehaviour
{
    public static Action<EButtonType> OnButtonPressedEvent;

    [SerializeField] private ButtonCustom _backButton = null;
    [SerializeField] private ButtonCustom _continueButton = null;

    private void OnEnable()
    {
        AddButtonListener(_backButton, BackButton);
        AddButtonListener(_continueButton, ContinueButton);
    }

    private void OnDisable()
    {
        RemoveButtonListener(_backButton, BackButton);
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

    private void BackButton()
    {
        ButtonPressed(EButtonType.BACK);
    }

    private void ContinueButton()
    {
        ButtonPressed(EButtonType.CONTINUE);
    }

    private void ButtonPressed(EButtonType buttonType)
    {
        OnButtonPressedEvent?.Invoke(buttonType);
    }
}