using System;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public static Action OnStateChangedEvent;

    [SerializeField] private EMenuState _currentState = EMenuState.NONE;

    private void Awake()
    {
        SelectState(EMenuState.CATEGORIES);
    }

    private void OnEnable()
    {
        MenuUI.OnButtonPressedEvent += CheckToChangeState;
    }

    private void OnDisable()
    {
        MenuUI.OnButtonPressedEvent -= CheckToChangeState;
    }

    private void CheckToChangeState(EButtonType buttonType)
    {
        switch (buttonType)
        {
            case EButtonType.BACK:
            {
                break;
            }
            case EButtonType.CONTINUE:
            {
                break;
            }
        }
    }

    private void SelectState(EMenuState state)
    {
        _currentState = state;
        OnStateChangedEvent?.Invoke();
    }
}