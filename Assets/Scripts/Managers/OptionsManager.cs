using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour, IManager
{
    [SerializeField] private Button _optionsButton = null;
    [SerializeField] private GameObject _optionsBlocker = null;
    [SerializeField] private GameObject _optionsMenu = null;

    public void Contruct()
    {
        SetOptions(false);
        DeactivateOptions();
    }

    public void Activate()
    {
        ScenesManager.OnSceneLoadedEvent += OnSceneLoaded;
    }

    public void Deactivate()
    {
        DeactivateOptions();
        ScenesManager.OnSceneLoadedEvent -= OnSceneLoaded;
    }

    private void OnSceneLoaded(EScene sceneType)
    {
        if (sceneType != EScene.MENU && sceneType != EScene.GAMEPLAY)
        {
            return;
        }
        ActivateOptions();
    }

    private void ActivateOptions()
    {
        if (_optionsButton)
        {
            SetGameObjectActive(_optionsButton.gameObject, true);
        }
        SubscriptionButton();
    }
    
    private void DeactivateOptions()
    {
        if (_optionsButton)
        {
            SetGameObjectActive(_optionsButton.gameObject, false);
        }
        DesubscriptionButton();
    }

    private void SubscriptionButton()
    {
        if (_optionsButton == null)
        {
            return;
        }
        _optionsButton.onClick.AddListener(ToggleOptions);
    }
    
    private void DesubscriptionButton()
    {
        if (_optionsButton == null)
        {
            return;
        }
        _optionsButton.onClick.RemoveListener(ToggleOptions);
    }

    private void ToggleOptions()
    {
        if (_optionsMenu == null)
        {
            return;
        }
        SetOptions(!_optionsMenu.activeSelf);
    }
    
    private void SetOptions(bool newState)
    {
        if (_optionsMenu == null)
        {
            return;
        }
        SetGameObjectActive(_optionsMenu, newState);
        SetGameObjectActive(_optionsBlocker, newState);
    }

    private void SetGameObjectActive(GameObject objectToSet, bool newState)
    {
        if (objectToSet == null || objectToSet.activeSelf == newState)
        {
            return;
        }
        objectToSet.SetActive(newState);
    }
}