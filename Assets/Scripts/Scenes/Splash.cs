using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

public class Splash : MonoBehaviour
{
    public static Action<EDirection> OnSceneFinishedEvent;

    [SerializeField] private Image _logo = null;
    [SerializeField] private bool _hasSceneFinished = false;
    private SettingsSplash _settingsSplash = null;
    private bool _isExecutionReady = false;
    private bool _isSplashReady = false;

    private void OnEnable()
    {
        SettingsManager.OnSetUpReadyEvent += SetSplashSettings;
        ExecutionManager.OnFirstFrameEvent += AfterFirstFrame;
        ExecutionManager.OnSetUpReadyEvent += OnExecutionReady;
    }

    private void OnDisable()
    {
        SettingsManager.OnSetUpReadyEvent -= SetSplashSettings;
        ExecutionManager.OnFirstFrameEvent -= AfterFirstFrame;
        ExecutionManager.OnSetUpReadyEvent -= OnExecutionReady;
    }
    
    private void SetSplashSettings()
    {
        _settingsSplash = SettingsManager.Splash;
        if(_settingsSplash == null)
        {
            Debug.LogError("Splash Settings is null");
        }
    }

    private void AfterFirstFrame()
    {
        Timing.RunCoroutine(Coroutine_Splash());
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
        OnSplashReady();
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

    private void OnExecutionReady()
    {
        _isExecutionReady = true;
        TryToContinue();
    }

    private void OnSplashReady()
    {
        _isSplashReady = true;
        TryToContinue();
    }

    private void TryToContinue()
    {
        if(!IsReady() || _hasSceneFinished)
        {
            return;
        }
        // _hasSceneFinished = true;
        // OnSceneFinishedEvent?.Invoke(EDirection.NEXT);
    }

    private bool IsReady()
    {
        if(!_isExecutionReady || !_isSplashReady)
        {
            return false;
        }
        return true;
    }
}