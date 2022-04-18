using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SettingsMenu : SerializedScriptableObject
{
    [SerializeField] private Dictionary<EMenuState, MenuState> states = new Dictionary<EMenuState, MenuState>();
    [SerializeField] private Dictionary<EMenuCategory, MenuStateUI> _categories = new Dictionary<EMenuCategory, MenuStateUI>();
    [SerializeField] private Dictionary<EMenuMode, MenuStateUI> _modes = new Dictionary<EMenuMode, MenuStateUI>();
    [SerializeField] private Dictionary<EMenuCourse, MenuStateUI> _courses = new Dictionary<EMenuCourse, MenuStateUI>();

    private class MenuState
    {
        [SerializeField] private EMenuState _previousState;
        [SerializeField] private EMenuState _nextState;

        public EMenuState PreviousState => _previousState;
        public EMenuState NextState => _nextState;
    }

    public MenuStateUI GetCategoryUI(EMenuCategory categoryType)
    {
        MenuStateUI category = null;
        if(_categories == null || !_categories.ContainsKey(categoryType))
        {
            return category;
        }
        category = _categories[categoryType];
        return category;
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

public class MenuStateUI
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _image;
}