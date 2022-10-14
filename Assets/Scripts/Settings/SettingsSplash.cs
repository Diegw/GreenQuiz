using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class SettingsSplash : ScriptableObject
{
        public float PublisherSeconds => _publisherSecondsToShow;
        public float DeveloperSeconds => _developerSecondsToShow;
        
        [Range(0.5f, 10f), SerializeField] private float _publisherSecondsToShow;
        [Range(0.5f, 10f), SerializeField] private float _developerSecondsToShow;
}