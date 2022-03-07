using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SettingsMenu : SerializedScriptableObject
{
    [SerializeField] private Dictionary<EMenuState, MenuState> states = new Dictionary<EMenuState, MenuState>();

    private class MenuState
    {
        [SerializeField] private EMenuState _previousState;
        [SerializeField] private EMenuState _nextState;

        public EMenuState PreviousState => _previousState;
        public EMenuState NextState => _nextState;
    }
}