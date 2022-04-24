using System;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public static Action<EDirection> OnSceneFinishedEvent;
    public static Action<EMenuState, bool> OnStateChangedEvent;

    [SerializeField] private bool _hasSceneFinished = false;
    [SerializeField] private EMenuState _currentState = EMenuState.NONE;
    [SerializeField] private EMenuCategory _currentCategory = EMenuCategory.NONE;
    [SerializeField] private EMenuMode _currentMode = EMenuMode.NONE;
    [SerializeField] private EMenuCourse _currentCourse = EMenuCourse.NONE;
    private SettingsMenu _settingsMenu = null;


    private void OnEnable()
    {
        MenuUI.OnButtonPressedEvent += CheckToChangeState;
    }

    private void OnDisable()
    {
        MenuUI.OnButtonPressedEvent -= CheckToChangeState;
    }

    private void Start()
    {
        _settingsMenu = SettingsManager.Menu;
        if(_settingsMenu == null)
        {
            Debug.LogError("Settings Menu is null");
            return;
        }
        SelectState(EDirection.NEXT, true);
    }
    
    private void CheckToChangeState(EButtonType buttonType)
    {
        if(_settingsMenu == null)
        {
            return;
        }
        EDirection direction = EDirection.NONE;
        switch (buttonType)
        {
            case EButtonType.BACK:
            {
                direction = EDirection.PREVIOUS;
                break;
            }
            case EButtonType.CONTINUE:
            {
                direction = EDirection.NEXT;
                break;
            }
        }
        SelectState(direction);
    }

    private void SelectState(EDirection direction, bool isFirstState = false)
    {
        if(_settingsMenu == null)
        {
            return;
        }
        SelectData(direction);
        EMenuState state = _settingsMenu.NewState(_currentState, direction);
        if(state == _currentState)
        {
            return;
        }
        _currentState = state;
        OnStateChangedEvent?.Invoke(_currentState, isFirstState);
        if(!_hasSceneFinished && state == EMenuState.GAMEPLAY)
        {
            _hasSceneFinished = true;
            OnSceneFinishedEvent(EDirection.NEXT);
        }
    }

    private void SelectData(EDirection direction)
    {
        GameManager.SetCategory(_currentCategory);
        GameManager.SetGameMode(_currentMode);
        GameManager.SetCourse(_currentCourse);
    }
}