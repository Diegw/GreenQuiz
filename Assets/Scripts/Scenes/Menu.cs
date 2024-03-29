using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class Menu : SerializedMonoBehaviour
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
    [SerializeField] private Image _itemImage = null;
    [SerializeField] private ButtonCustom _leftButton = null;
    [SerializeField] private ButtonCustom _rightButton = null;
    [SerializeField] private ButtonCustom _continueButton = null;
    [SerializeField] private EMenuCategory[] _categories = null;
    [SerializeField] private EMenuMode[] _modes = null;
    [SerializeField] private Dictionary<EMenuCategory, Animator> _animators = new Dictionary<EMenuCategory, Animator>();
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
        SetSelectionImages();
        AudioManager.PlaySfx();
    }

    private void ChangePreviousSelection()
    {
        ChangeSelection(false);
        SetSelectionNames();
        SetSelectionImages();
        AudioManager.PlaySfx();
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
        if (_currentState == EMenuState.MODES && _currentMode == EMenuMode.RANDOM)
        {
            _currentCourse = _menuSettings.GetRandomCourse(_currentCategory);
        }
        ConfirmSelection();
        EMenuState state = _menuSettings.NewState(_currentState);
        if(state == _currentState)
        {
            return;
        }
        _currentState = state;
        OnStateChangedEvent?.Invoke(state);
        if (state == EMenuState.COURSES)
        {
            _currentCourse = _menuSettings.GetFirstCourse(_currentCategory);
        }
        if(!_hasSceneFinished && state == EMenuState.GAMEPLAY)
        {
            _hasSceneFinished = true;
            OnSceneFinishedEvent(EDirection.NEXT);
        }
        SetUI();
        AudioManager.PlaySfx();
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
        SetSelectionImages();
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
    
    private void SetSelectionImages()
    {
        Sprite itemImage = null;
        switch (_currentState)
        {
            case EMenuState.CATEGORIES:
            {
                SetAnimationCategory();
                // itemImage = _menuSettings.GetCategorySprite(_currentCategory);
                return;
            }
            case EMenuState.MODES:
            {
                SetAnimationCategory(true);
                itemImage = _menuSettings.GetModeSprite(_currentMode);
                break;
            }
            case EMenuState.COURSES:
            {
                SetAnimationCategory(true);
                itemImage = _menuSettings.GetCourseSprite(_currentCourse);
                break;
            } 
        }
        if (_itemImage && itemImage)
        {
            _itemImage.sprite = itemImage;
            _itemImage.SetNativeSize();
            _itemImage.gameObject.SetActive(true);
        }
    }

    private void SetAnimationCategory(bool hideAll = false)
    {
        foreach (var animator in _animators)
        {
            if (animator.Key == EMenuCategory.NONE || animator.Value == null)
            {
                continue;
            }
            animator.Value.gameObject.SetActive(animator.Key == _currentCategory && hideAll == false);
        }
    }
}