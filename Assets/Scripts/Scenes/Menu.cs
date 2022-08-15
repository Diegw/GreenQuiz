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
    [SerializeField] private ButtonCustom _continueButton = null;
    [SerializeField] private InfiniteScroll _infiniteScroll = null;
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
        _currentCategory = _menuSettings.GetFirstCategory();
        _currentMode = _menuSettings.GetFirstMode();
        _currentCourse = _menuSettings.GetFirstCourse(_currentCategory);
    }
    
    private void OnEnable()
    {
        InfiniteScroll.OnEndDragEvent += ChangeSelection;
        AddButtonListener(_continueButton, SelectNextState);
    }

    private void OnDisable()
    {
        InfiniteScroll.OnEndDragEvent -= ChangeSelection;
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

    private void ChangeSelection(EMenuCategory category, EMenuMode mode, EMenuCourse course)
    {
        if (category != EMenuCategory.NONE)
        {
            _currentCategory = category;
        }
        if (mode != EMenuMode.NONE)
        {
            _currentMode = mode;
        }
        if (course != EMenuCourse.NONE)
        {
            _currentCourse = course;
        }
        SetItemNames();
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
        SetItemNames();
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
        _infiniteScroll.InstantiateItems(itemCount);
        _infiniteScroll.SetItemsSprites(sprites);
        _infiniteScroll.Initialize();
    }

    private void SetItemNames()
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