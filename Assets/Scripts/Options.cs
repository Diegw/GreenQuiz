using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public static event Action<bool> OnResumeButtonEvent;
    
    [SerializeField] private Button _resumeButton = null;
    [SerializeField] private Button _soundButton = null;
    [SerializeField] private Button _musicButton = null;
    [SerializeField] private Button _creditsButton = null;
    [SerializeField] private Button _exitButton = null;
    [SerializeField] private GameObject _credits = null;
    [SerializeField] private GameObject _exitConfirmation = null;
    [SerializeField] private Button _exitYesButton = null;
    [SerializeField] private Button _exitNoButton = null;
    [SerializeField] private Button _creditsReturnButton = null;
    [FormerlySerializedAs("_audioSource")] [SerializeField] private AudioSource _musicAudioSource = null;
    private bool _isMusicEnabled = true;

    private void Awake()
    {
        _isMusicEnabled = PlayerPrefs.GetInt("Music") > 0;
        if (_musicAudioSource && _musicAudioSource.mute != !_isMusicEnabled)
        {
            _musicAudioSource.mute = !_isMusicEnabled;
        }
        SetGameObjectActive(_credits, false);
        SetGameObjectActive(_exitConfirmation, false);
    }

    private void OnEnable()
    {
        Subscriptions();
    }

    private void OnDisable()
    {
        Unsubscriptions();
    }
    
    private void Subscriptions()
    {
        Subscription(_resumeButton, Resume);
        Subscription(_soundButton, Sound);
        Subscription(_musicButton, Music);
        Subscription(_creditsButton, Credits);
        Subscription(_exitButton, ExitConfirmation);
        Subscription(_exitYesButton, Exit);
        Subscription(_exitNoButton, ExitReturn);
        Subscription(_creditsReturnButton, Credits);
    }
    
    private void Unsubscriptions()
    { 
        Unsubscription(_resumeButton, Resume);
        Unsubscription(_soundButton, Sound);
        Unsubscription(_musicButton, Music);
        Unsubscription(_creditsButton, Credits);
        Unsubscription(_exitButton, ExitConfirmation);
        Unsubscription(_exitYesButton, Exit);
        Unsubscription(_exitNoButton, ExitReturn);
        Unsubscription(_creditsReturnButton, Credits);
    }

    private void Subscription(Button button, UnityAction action)
    {
        if (button == null)
        {
            return;
        }
        button.onClick.AddListener(action);
    }

    private void Unsubscription(Button button, UnityAction action)
    {
        if (button == null)
        {
            return;
        }
        button.onClick.RemoveListener(action);
    }
    
    private void Resume()
    {
        OnResumeButtonEvent?.Invoke(false);
        AudioManager.PlaySfx();
    }
    
    private void Sound()
    {
        
        AudioManager.PlaySfx();
    }
    
    private void Music()
    {
        _isMusicEnabled = !_isMusicEnabled;
        if (_musicAudioSource)
        {
            _musicAudioSource.mute = !_isMusicEnabled;
        }
        PlayerPrefs.SetInt("Music",_isMusicEnabled?1:0);
        AudioManager.PlaySfx();
    }

    private void Credits()
    {
        if (_credits == null)
        {
            return;
        }
        SetGameObjectActive(_credits, !_credits.activeSelf);
        AudioManager.PlaySfx();
    }

    private void ExitConfirmation()
    {
        SetGameObjectActive(_exitConfirmation, true);
        AudioManager.PlaySfx();
    }

    private void ExitReturn()
    {
        SetGameObjectActive(_exitConfirmation, false);
        AudioManager.PlaySfx();
    }
    
    private void Exit()
    {
        AudioManager.PlaySfx();
        Application.Quit();
    }
    
    private void SetGameObjectActive(GameObject objectToSet, bool newState)
    {
        if (objectToSet == null || objectToSet.activeSelf == newState)
        {
            return;
        }
        objectToSet.SetActive(newState);
    }
}