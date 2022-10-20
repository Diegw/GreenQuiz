using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviourCustom
{
    [SerializeField] private Button _optionsButton = null;
    [SerializeField] private GameObject _optionsBlocker = null;
    [SerializeField] private GameObject _optionsMenu = null;

    protected override void Awake()
    {
        base.Awake();
        ToggleOptions();
        SubscriptionButton(true);
    }

    private void SubscriptionButton(bool isSubscribing)
    {
        if (_optionsButton == null)
        {
            return;
        }

        if (isSubscribing)
        {
            _optionsButton.onClick.AddListener(ToggleOptions);
        }
        else
        {
            _optionsButton.onClick.RemoveListener(ToggleOptions);
        }
    }

    private void ToggleOptions()
    {
        if (_optionsMenu == null)
        {
            return;
        }
        SetGameObjectActive(_optionsMenu, !_optionsMenu.activeSelf);
        SetGameObjectActive(_optionsBlocker, !_optionsBlocker.activeSelf);
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