using System;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    public static Action<EDirection> OnSceneFinishedEvent;
    public static Action<EButtonType> OnButtonPressedEvent;

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
        SelectNextState();
    }

    private void OnEnable()
    {
        AddButtonListener(_continueButton, SelectNextState);
    }

    private void OnDisable()
    {
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

    private void SelectNextState()
    {
        if(_menuSettings == null)
        {
            return;
        }
        SelectData();
        EMenuState state = _menuSettings.NewState(_currentState);
        if(state == _currentState)
        {
            return;
        }
        _currentState = state;
        SetUI();
        if(!_hasSceneFinished && state == EMenuState.GAMEPLAY)
        {
            _hasSceneFinished = true;
            OnSceneFinishedEvent(EDirection.NEXT);
        }
    }

    private void SelectData()
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
        switch (actualState)
        {
            case EMenuState.CATEGORIES:
            {
                _itemDisplay.text = _menuSettings.GetCategoryName(GameManager.Category);
                _infiniteScroll.InstantiateItems(_menuSettings.GetCategoriesCount());
                _infiniteScroll.SetItemsSprites(_menuSettings.GetCategoriesSprites());
                _infiniteScroll.Initialize();
                break;
            }
            case EMenuState.MODES:
            {
                _itemDisplay.text = _menuSettings.GetModeName(GameManager.Mode);
                _infiniteScroll.InstantiateItems(_menuSettings.GetModesCount());
                _infiniteScroll.SetItemsSprites(_menuSettings.GetModesSprites());
                _infiniteScroll.Initialize();
                break;
            }
            case EMenuState.COURSES:
            {
                _itemDisplay.text = _menuSettings.GetCourseName(GameManager.Course);
                _infiniteScroll.InstantiateItems(_menuSettings.GetCoursesCount(GameManager.Category));
                _infiniteScroll.SetItemsSprites(_menuSettings.GetCoursesSprites(GameManager.Category));
                _infiniteScroll.Initialize();
                break;
            }
        }
    }

    private string GetStateItemName(EMenuState state)
    {
        string stateItemName = "";
        if(_menuSettings != null)
        {
            switch (state)
            {
                case EMenuState.CATEGORIES:
                {
                    stateItemName = _menuSettings.GetCategoryName(GameManager.Category);
                    break;
                }
                case EMenuState.MODES:
                {
                    stateItemName = _menuSettings.GetModeName(GameManager.Mode);
                    break;
                }
                case EMenuState.COURSES:
                {
                    stateItemName = _menuSettings.GetCourseName(GameManager.Course);
                    break;
                }
            }
        }
        return stateItemName;
    }

    private void ContinueButton()
    {
        OnButtonPressedEvent?.Invoke(EButtonType.CONTINUE);
    }
}