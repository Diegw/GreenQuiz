using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

public class Splash : MonoBehaviour
{
    public static Action<EDirection> OnSceneFinishedEvent;

    [SerializeField] private bool _hasSplashFinished = false;
    [SerializeField] private Transform _logosHolder = null;
    private Image[] _logos = null;
    private SettingsSplash _settingsSplash = null;

    private void Awake()
    {
        SetLogos();
        SetSplashSettings();
        Timing.RunCoroutine(Coroutine_Splash());
    }

    private void SetLogos()
    {
        if (_logosHolder == null)
        {
            Debug.LogError("Splash - LogosHolder is null");
            return;
        }
        Image[] images = _logosHolder.GetComponentsInChildren<Image>();
        if (images == null || images.Length <= 0)
        {
            Debug.LogError("Splash - Couldn't find any logo images");
            return;
        }
        _logos = images;
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
        if(_settingsSplash == null)
        {
            yield break;
        }
        ToggleLogos(0);
        yield return Timing.WaitForSeconds(_settingsSplash.DeveloperSeconds);
        ToggleLogos(1);
        yield return Timing.WaitForSeconds(_settingsSplash.PublisherSeconds);
        TryToContinue();
    }

    private void ToggleLogos(int logoIndex)
    {
        for (int i = 0; i < _logos.Length; i++)
        {
            _logos[i].gameObject.SetActive(logoIndex == i);
        }
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