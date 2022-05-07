using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class SettingsSplash : ScriptableObject
{
    [BoxGroup("PUBLISHER"), HideLabel, SerializeField] private SplashData _publisherData;
    [BoxGroup("DEVELOPER"), HideLabel, SerializeField] private SplashData _developerData;

    [Serializable] private struct SplashData
    {
        public float SecondsToShow => _secondsToShow;
        public Sprite Logo => _logo;

        [Range(0.5f, 10f), SerializeField] private float _secondsToShow;
        [SerializeField] private Sprite _logo;
    }

#region DEVELOPER
    public float GetDeveloperSeconds()
    {
        return _developerData.SecondsToShow;
    }

    public Sprite GetDeveloperLogo()
    {
        return _developerData.Logo;
    }
#endregion

#region PUBLISHER
    public float GetPublisherSeconds()
    {
        return _publisherData.SecondsToShow;
    }

    public Sprite GetPublisherLogo()
    {
        return _publisherData.Logo;
    }
}
#endregion