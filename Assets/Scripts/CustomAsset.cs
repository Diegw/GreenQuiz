using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class CustomAsset : MonoBehaviour
{
    [SerializeField, HideLabel, BoxGroup("AssetID")] private CustomAssetID _assetID;
    [SerializeField] private TMP_Text _display = null;
    [SerializeField] private Image _image = null;
    private SettingsAssets _settingsAssets = null;

    private void Start()
    {
        _settingsAssets = SettingsManager.Assets;
        SetCustomAssetData();
    }

    private void SetCustomAssetData()
    {
        if(_settingsAssets == null)
        {
            Debug.LogError("Settings Assets is null");
            return;
        }

        CustomAssetData customAssetData = _settingsAssets.GetCustomAssetData(_assetID);
        if (_image != null)
        {
            _image.sprite = customAssetData.CustomSprite;
            _image.color = customAssetData.CustomColor;
        }
        if (_display != null)
        {
            _display.color = customAssetData.CustomColor;
        }
    }
}