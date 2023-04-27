using System;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    public static Action<EMenuState> OnStateChangedEvent;
    public static Action<EDirection> OnSceneFinishedEvent;

    [SerializeField] private bool _hasSceneFinished = false;
    [SerializeField] private EMenuState _currentState = EMenuState.NONE;
    [SerializeField] private EMenuCategory _currentCategory = EMenuCategory.NONE;
    [SerializeField] private EMenuMode _currentMode = EMenuMode.NONE;
    [SerializeField] private EMenuCourse _currentCourse = EMenuCourse.NONE;
    [Header("UI")]
    [SerializeField] private TMP_Text _instructionsDisplay = null;
    [SerializeField] private TMP_Text _itemDisplay = null;
    [SerializeField] private ButtonCustom _leftButton = null;
    [SerializeField] private ButtonCustom _rightButton = null;
    [SerializeField] private ButtonCustom _continueButton = null;
    [SerializeField] private EMenuCategory[] _categories = null;
    [SerializeField] private EMenuMode[] _modes = null;
    private SettingsMenu _menuSettings = null;

    private void Awake()
    {
        _menuSettings = SettingsManager.Menu;
        if(_menuSettings == null)
        {
            Debug.LogError("Settings Menu is null");
            return;
        }
        InitializeSelection();
        SelectNextState();
    }

    private void InitializeSelection()
    {
        if (_menuSettings == null)
        {
            return;
        }
        _currentCategory = (EMenuCategory)1;
        _currentMode = (EMenuMode)1;
        _currentCourse = _menuSettings.GetFirstCourse(_currentCategory);
    }
    
    private void OnEnable()
    {
        AddButtonListener(_leftButton, ChangePreviousSelection);
        AddButtonListener(_rightButton, ChangeNextSelection);
        AddButtonListener(_continueButton, SelectNextState);
    }

    private void OnDisable()
    {
        RemoveButtonListener(_leftButton, ChangePreviousSelection);
        RemoveButtonListener(_rightButton, ChangeNextSelection);
        RemoveButtonListener(_continueButton, SelectNextState);
    }

    private void AddButtonListener(ButtonCustom buttonCustom, UnityAction action)
    {
        if(buttonCustom == null || buttonCustom.Button == null)
        {
            return;
        }
        buttonCustom.Button.onClick.AddListener(action);
    }

    private void RemoveButtonListener(ButtonCustom buttonCustom, UnityAction action)
    {
        if(buttonCustom == null || buttonCustom.Button == null)
        {
            return;
        }
        buttonCustom.Button.onClick.RemoveListener(action);
    }

    private void ChangeNextSelection()
    {
        ChangeSelection(true);
        SetSelectionNames();
    }

    private void ChangePreviousSelection()
    {
        ChangeSelection(false);
        SetSelectionNames();
    }

    private void ChangeSelection(bool next)
    {
        switch (_currentState)
        {
            case EMenuState.CATEGORIES:
            {
                _currentCategory = GetCategory(next);
                break;
            }
            case EMenuState.MODES:
            {
                _currentMode = GetMode(next);
                break;
            }
            case EMenuState.COURSES:
            {
                _currentCourse = _menuSettings.GetNewCourse(next, _currentCourse, _currentCategory);
                break;
            }
        }
    }
    
    private EMenuCategory GetCategory(bool next)
    {
        int newIndex = 0;
        EMenuCategory newCategory = EMenuCategory.NONE;
        for (int i = 0; i < _categories.Length; i++)
        {
            newIndex = next ? 
                i + 1 >= _categories.Length ? 0 : i+1 : 
                i -1 < 0 ? _categories.Length-1 : i-1;

            if (newIndex < _categories.Length && newIndex >= 0 && _categories[i] == _currentCategory)
            {
                newCategory = _categories[newIndex];
                break;
            }
        }
        return newCategory;
    }
    
    private EMenuMode GetMode(bool next)
    {
        int newIndex = 0;
        EMenuMode newMode = EMenuMode.NONE;
        for (int i = 0; i < _modes.Length; i++)
        {
            if (next)
            {
                newIndex = i + 1 >= _modes.Length ? 0 : i+1;
            }
            else
            {
                newIndex = i -1 < 0 ? _modes.Length-1 : i-1;
            }

            if (newIndex < _modes.Length && newIndex >= 0 && _modes[i] == _currentMode)
            {
                newMode = _modes[newIndex];
                break;
            }
        }
        return newMode;
    }

    private void SelectNextState()
    {
        if(_menuSettings == null)
        {
            return;
        }
        ConfirmSelection();
        EMenuState state = _menuSettings.NewState(_currentState);
        if(state == _currentState)
        {
            return;
        }
        _currentState = state;
        OnStateChangedEvent?.Invoke(state);
        if(state == EMenuState.COURSES) _currentCourse = _menuSettings.GetFirstCourse(_currentCategory);
        SetUI();
        if(!_hasSceneFinished && state == EMenuState.GAMEPLAY)
        {
            _hasSceneFinished = true;
            OnSceneFinishedEvent(EDirection.NEXT);
        }
    }

    private void ConfirmSelection()
    {
        GameManager.SetCategory(_currentCategory);
        GameManager.SetGameMode(_currentMode);
        GameManager.SetCourse(_currentCourse);
    }

    private void SetUI()
    {
        if(_menuSettings == null)
        {
            return;
        }
        EMenuState actualState = _currentState;
        if(_instructionsDisplay != null)
        {
            _instructionsDisplay.text = _menuSettings.GetStateInstructions(actualState);
        }

        if (actualState == EMenuState.NONE || actualState == EMenuState.GAMEPLAY)
        {
            return;
        }
        SetSelectionNames();
        int itemCount = 0;
        Sprite[] sprites = null;
        switch (actualState)
        {
            case EMenuState.CATEGORIES:
            {
                itemCount = _menuSettings.GetCategoriesCount();
                sprites = _menuSettings.GetCategoriesSprites();
                break;
            }
            case EMenuState.MODES:
            {
                itemCount = _menuSettings.GetModesCount();
                sprites = _menuSettings.GetModesSprites();
                break;
            }
            case EMenuState.COURSES:
            {
                itemCount = _menuSettings.GetCoursesCount(GameManager.Category);
                sprites = _menuSettings.GetCoursesSprites(GameManager.Category);
                break;
            } 
        }
    }

    private void SetSelectionNames()
    {
        string itemName = "";
        switch (_currentState)
        {
            case EMenuState.CATEGORIES:
            {
                itemName = _menuSettings.GetCategoryName(_currentCategory);
                break;
            }
            case EMenuState.MODES:
            {
                itemName = _menuSettings.GetModeName(_currentMode);
                break;
            }
            case EMenuState.COURSES:
            {
                itemName = _menuSettings.GetCourseName(_currentCourse);
                break;
            } 
        }
        _itemDisplay.text = itemName;
    }
}