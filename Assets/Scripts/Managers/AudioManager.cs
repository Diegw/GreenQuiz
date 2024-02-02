using UnityEngine;

public class AudioManager : MonoBehaviour, IManager
{
    [SerializeField] private AudioSource _sfxAudioSource = null;
    private static AudioManager _instance = null;
    
    public void Activate()
    {
        
    }

    public void Contruct()
    {
        _instance = this;
    }

    public void Deactivate()
    {
    }
    
    public static void PlaySfx()
    {
        if (_instance != null && _instance._sfxAudioSource)
        {
            _instance._sfxAudioSource.Play();
        }
    }
}