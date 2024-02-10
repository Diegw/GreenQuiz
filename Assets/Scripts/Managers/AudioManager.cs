using UnityEngine;

public class AudioManager : MonoBehaviour, IManager
{
    private static AudioManager _instance = null;
    [SerializeField] private AudioSource _musicAudioSource = null;
    [SerializeField] private AudioSource _sfxAudioSource = null;
    private bool _isSfxEnabled = true;
    private bool _isMusicEnabled = true;

    public void Contruct()
    {
        _instance = this;
    }
    
    public void Activate()
    {
        string musicState = PlayerPrefs.GetString("Music");
        Debug.LogError(musicState);
        if (_musicAudioSource)
        {
            if (musicState is "Disabled")
            {
                _isMusicEnabled = false;
            }
            _musicAudioSource.mute = !_isMusicEnabled;
        }
        
        string sfxState = PlayerPrefs.GetString("Sfx");
        if (_sfxAudioSource)
        {
            if (sfxState is "Disabled")
            {
                _isSfxEnabled = false;
            }
            _sfxAudioSource.mute = !_isSfxEnabled;
        }
    }

    public void Deactivate()
    {
    }

    public static void ToggleMusic()
    {
        if (_instance == null)
        {
            return;
        }
        bool newState = !_instance._isMusicEnabled;
        _instance._isMusicEnabled = newState;
        if (_instance._musicAudioSource)
        {
            _instance._musicAudioSource.mute = !newState;
        }
        PlayerPrefs.SetString("Music", newState? "Enabled" : "Disabled");
    }
    
    public static void ToggleSfx()
    {
        if (_instance == null)
        {
            return;
        }
        bool newState = !_instance._isSfxEnabled;
        _instance._isSfxEnabled = newState;
        if (_instance._sfxAudioSource)
        {
            _instance._sfxAudioSource.mute = !newState;
        }
        PlayerPrefs.SetString("Sfx", newState? "Enabled" : "Disabled");
    }
    
    public static void PlaySfx()
    {
        if (_instance != null && _instance._sfxAudioSource)
        {
            _instance._sfxAudioSource.Play();
        }
    }
}