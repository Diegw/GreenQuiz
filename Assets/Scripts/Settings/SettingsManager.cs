using System;
using UnityEngine;

public class SettingsManager : MonoBehaviour, IManager
{
    public static Action OnSetUpReadyEvent;
    
    public static SettingsSplash Splash
    {
        get
        {
            if(_instance == null || _instance._settingsSplash == null)
            {
                return null;
            }
            return _instance._settingsSplash;
        }
    }
    public static SettingsTitle Title
    {
        get
        {
            if(_instance == null || _instance._settingsTitle == null)
            {
                return null;
            }
            return _instance._settingsTitle;
        }
    }
    public static SettingsScenes Scenes
    {
        get
        {
            if(_instance == null || _instance._settingsScenes == null)
            {
                return null;
            }
            return _instance._settingsScenes;
        }
    }
    public static SettingsMenu Menu
    {
        get
        {
            if(_instance == null || _instance._settingsMenu == null)
            {
                return null;
            }
            return _instance._settingsMenu;
        }
    }
    public static SettingsQuestion Question
    {
        get
        {
            if(_instance == null || _instance._settingsQuestion == null)
            {
                return null;
            }
            return _instance._settingsQuestion;
        }
    }

    private static SettingsManager _instance = null;
    [SerializeField] private SettingsSplash _settingsSplash = null;
    [SerializeField] private SettingsTitle _settingsTitle = null;
    [SerializeField] private SettingsScenes _settingsScenes = null;
    [SerializeField] private SettingsMenu _settingsMenu = null;
    [SerializeField] private SettingsQuestion _settingsQuestion = null;

    public void Contruct()
    {
        _instance = this;
        OnSetUpReadyEvent?.Invoke();
    }

    public void Activate()
    {
    }

    public void Deactivate()
    {
    }
}