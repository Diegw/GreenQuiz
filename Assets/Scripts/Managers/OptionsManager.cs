using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour, IManager
{
    [SerializeField] private GameObject _optionsUI = null;
    [SerializeField] private GameObject _optionsBlocker = null;
    [SerializeField] private Button _optionsButton = null;

    public void Contruct()
    {
        SetOptions(false);
    }

    public void Activate()
    {
        SubscriptionButton();
        Options.OnResumeButtonEvent += SetOptions;
        ScenesManager.OnSceneUnloadedEvent += OnSceneUnloaded;
        ScenesManager.OnSceneLoadedEvent += OnSceneLoaded;
    }

    public void Deactivate()
    {
        UnsubscriptionButton();
        Options.OnResumeButtonEvent -= SetOptions;
        ScenesManager.OnSceneUnloadedEvent -= OnSceneUnloaded;
        ScenesManager.OnSceneLoadedEvent -= OnSceneLoaded;
    }

    private void OnSceneUnloaded(EScene sceneType)
    {
        SetOptions(false);
    }
    
    private void OnSceneLoaded(EScene sceneType)
    {
        bool enableOptionsInScene = sceneType == EScene.MENU || sceneType == EScene.GAMEPLAY;
        SetGameObjectActive(_optionsButton.gameObject, enableOptionsInScene);
    }

    private void SubscriptionButton()
    {
        if (_optionsUI == null)
        {
            return;
        }
        _optionsButton.onClick.AddListener(ToggleOptions);
    }
    
    private void UnsubscriptionButton()
    {
        if (_optionsUI == null)
        {
            return;
        }
        _optionsButton.onClick.RemoveListener(ToggleOptions);
    }

    private void ToggleOptions()
    {
        if (_optionsUI == null)
        {
            return;
        }
        SetOptions(!_optionsUI.gameObject.activeSelf);
    }
    
    private void SetOptions(bool newState)
    {
        if (_optionsUI == null || _optionsBlocker == null)
        {
            return;
        }
        SetGameObjectActive(_optionsUI.gameObject, newState);
        SetGameObjectActive(_optionsBlocker.gameObject, newState);
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