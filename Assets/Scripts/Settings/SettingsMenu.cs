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

    public EMenuState NewState(EMenuState currentState, EDirection direction)
    {
        EMenuState newMenuState = EMenuState.NONE;
        if(states != null && states.ContainsKey(currentState))
        {
            newMenuState = SelectNewState(currentState, direction, states[currentState]);
        }
        return newMenuState;
    }

    private EMenuState SelectNewState(EMenuState currentState, EDirection direction, MenuState menuState)
    {
        EMenuState newState = EMenuState.NONE;
        if(direction == EDirection.PREVIOUS)
        {
            newState = menuState.PreviousState;
        }
        else if (direction == EDirection.NEXT)
        {
            newState = menuState.NextState;
            if(currentState == EMenuState.MODES && GameManager.Mode == EMenuMode.RANDOM)
            {
                newState = EMenuState.GAMEPLAY;
            }
        }
        return newState;
    }
}