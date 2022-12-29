using UnityEngine;

public class CustomAssets : MonoBehaviour
{
    [SerializeField] private CustomAsset[] _customAssets;
    private SettingsAssets _settingsAssets;
    
    private void Awake()
    {
        if (_customAssets == null || _customAssets.Length <= 0)
        {
            return;
        }

        _settingsAssets = SettingsManager.Assets;
        if(_settingsAssets == null)
        {
            Debug.LogError("CustomAssets - Awake - SettingsAssets is null");
            return;
        }
        foreach (CustomAsset customAsset in _customAssets)
        {
            if (customAsset != null)
            {
                customAsset.SetCustomAsset(ref _settingsAssets);
            }
        }
    }
}