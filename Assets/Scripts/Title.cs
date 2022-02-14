using System;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviourCustom
{
    public static Action<EDirection> OnSceneFinishedEvent;

    [SerializeField] private Image _background = null;
    [SerializeField] private Image _logo = null;
    [SerializeField] private ButtonCustom _continueButton = null;
    private bool _wasPressed = false;

    private void Awake()
    {
        if(AreThereNullReferences(_background, _logo, _continueButton))
        {
            return;
        }
        Settings();
    }

    private void Settings()
    {
        //get settings
        //use settings
    }

    private void OnEnable()
    {
        ContinueButton();
    }

    private void ContinueButton()
    {
        if (_continueButton == null || _continueButton.Button == null)
        {
            Debug.LogError("Null reference");
            return;
        }
        _continueButton.Button.onClick.AddListener(Continue);
    }

    private void Continue()
    {
        _wasPressed = true;
        OnSceneFinishedEvent?.Invoke(EDirection.NEXT);
        _wasPressed = false;
    }
}