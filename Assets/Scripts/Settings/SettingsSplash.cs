using UnityEngine;

public class SettingsSplash : ScriptableObject
{
    public float DeveloperSeconds => _developerSeconds;
    public Sprite DeveloperLogo => _developerLogo;
    public float PublisherSeconds => _publisherSeconds;
    public Sprite PublisherLogo => _publisherLogo;

    [Space]
    [Range(0.5f, 10f), SerializeField] private float _developerSeconds;
    [SerializeField] private Sprite _developerLogo = null;
    [Space]
    [Range(0.5f, 10f), SerializeField] private float _publisherSeconds;
    [SerializeField] private Sprite _publisherLogo = null;
}