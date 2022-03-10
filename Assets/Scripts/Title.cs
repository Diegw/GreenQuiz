using System;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviourCustom
{
    public static Action<EDirection> OnSceneFinishedEvent;

    [SerializeField] private Image _background = null;
    [SerializeField] private Image _logo = null;
    [SerializeField] private ButtonCustom _continueButton = null;
    private SettingsTitle _settingsTitle = null;
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
        _settingsTitle = SettingsManager.Title;
        if(_settingsTitle == null)
        {
            Debug.LogError("Title settings is null");
            return;
        }
        
        if(_background == null || _logo == null)
        {
            return;
        }
        _background.sprite = _settingsTitle.BackgroundSprite.Sprite;
        _background.color = _settingsTitle.BackgroundSprite.Color;

        _logo.sprite = _settingsTitle.LogoSprite.Sprite;
        _logo.color = _settingsTitle.LogoSprite.Color;
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