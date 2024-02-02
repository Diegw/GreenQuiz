using System;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviourCustom
{
    public static Action<EDirection> OnSceneFinishedEvent;

    [SerializeField] private Image _logo = null;
    [SerializeField] private ButtonCustom _continueButton = null;
    private bool _hasSceneFinished = false;

    protected override void Awake()
    {
        AreThereNullReferences(_logo, _continueButton);
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
        if(_hasSceneFinished)
        {
            return;
        }
        _hasSceneFinished = true;
        OnSceneFinishedEvent?.Invoke(EDirection.NEXT);
        AudioManager.PlaySfx();
    }
}