using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MenuUI : MonoBehaviour
{
    public static Action<EButtonType> OnButtonPressedEvent;
    public static Action<Dictionary<EMenuCategory, MenuStateUI>> OnSetCategoriesUIEvent;

    [SerializeField] private TMP_Text _itemTitle = null;
    [SerializeField] private ButtonCustom _backButton = null;
    [SerializeField] private ButtonCustom _continueButton = null;
    private SettingsMenu _menuSettings = null;
    private EMenuState _lastMenuState = EMenuState.NONE;
    private InfiniteScroll _infiniteScroll = null;

    private void Awake()
    {
        _menuSettings = SettingsManager.Menu;
        _infiniteScroll = GetComponentInChildren<InfiniteScroll>();
        if(_menuSettings == null)
        {
            Debug.LogError("Menu Settings is null");
        }
    }

    private void OnEnable()
    {
        Menu.OnStateChangedEvent += SetUI;
        AddButtonListener(_backButton, BackButton);
        AddButtonListener(_continueButton, ContinueButton);
    }

    private void OnDisable()
    {
        Menu.OnStateChangedEvent -= SetUI;
        RemoveButtonListener(_backButton, BackButton);
        RemoveButtonListener(_continueButton, ContinueButton);
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

    private void SetUI(EMenuState state, bool isFirstState)
    {
        if(!isFirstState || _menuSettings == null)
        {
            return;
        }
        if(_lastMenuState == state)
        {
            return;
        }
        _lastMenuState = state;

        switch (state)
        {
            case EMenuState.CATEGORIES:
            {
                _itemTitle.text = _menuSettings.GetCategoryName(GameManager.Category);
                _infiniteScroll.InstantiateItems(_menuSettings.Categories.Count);
                _infiniteScroll.SetItemsSprites(_menuSettings.GetCategoriesSprites());
                _infiniteScroll.Initialize();
                // OnSetCategoriesUIEvent?.Invoke(_menuSettings.Categories);
                break;
            }
        }
    }

    private void BackButton()
    {
        ButtonPressed(EButtonType.BACK);
    }

    private void ContinueButton()
    {
        ButtonPressed(EButtonType.CONTINUE);
    }

    private void ButtonPressed(EButtonType buttonType)
    {
        OnButtonPressedEvent?.Invoke(buttonType);
    }
}