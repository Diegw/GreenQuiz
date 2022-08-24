using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

public class Splash : MonoBehaviour
{
    public static Action<EDirection> OnSceneFinishedEvent;

    [SerializeField] private Image _logo = null;
    [SerializeField] private bool _hasSplashFinished = false;
    private SettingsSplash _settingsSplash = null;

    private void Awake()
    {
        SetSplashSettings();
        Timing.RunCoroutine(Coroutine_Splash());
    }

    private void SetSplashSettings()
    {
        _settingsSplash = SettingsManager.Splash;
        if(_settingsSplash == null)
        {
            Debug.LogError("Splash Settings is null");
        }
    }

    private IEnumerator<float> Coroutine_Splash()
    {
        while(_settingsSplash == null)
        {
            yield return Timing.WaitForOneFrame;
        }
        SetLogo(_settingsSplash.GetDeveloperLogo());
        yield return Timing.WaitForSeconds(_settingsSplash.GetDeveloperSeconds());
        SetLogo(_settingsSplash.GetPublisherLogo());
        yield return Timing.WaitForSeconds(_settingsSplash.GetPublisherSeconds());
        TryToContinue();
    }

    private void SetLogo(Sprite newSprite)
    {
        if(_logo == null)
        {
            Debug.LogError("Splash - Logo Image is null");
            return;
        }
        _logo.sprite = newSprite;
    }

    private void TryToContinue()
    {
        if(_hasSplashFinished)
        {
            return;
        }
        _hasSplashFinished = true;
        OnSceneFinishedEvent?.Invoke(EDirection.NEXT);
    }
}