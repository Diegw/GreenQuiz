using System;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

[Serializable] public class CustomAsset
{
    [SerializeField, HideLabel, FoldoutGroup("Custom Asset")] private CustomAssetContainer _customAsset;

    public void SetCustomAsset(ref SettingsAssets settingsAssets)
    {
        _customAsset.SetCustomAsset(ref settingsAssets);
    }
}

[Serializable] public class CustomAssetContainer
{
    [SerializeField, HideLabel, BoxGroup("Asset ID")] private CustomAssetID _assetID;
    [SerializeField] private TMP_Text[] _displays = null;
    [SerializeField] private Image[] _images = null;

    public void SetCustomAsset(ref SettingsAssets settingsAssets)
    {
        CustomAssetData customAssetData = settingsAssets.GetCustomAssetData(_assetID);
        if (_images != null && _images.Length > 0)
        {
            foreach (Image image in _images)
            {
                if (image != null)
                {
                    image.sprite = customAssetData.CustomSprite;
                    image.color = customAssetData.CustomSpriteColor;
                }
            }
        }
        if (_displays != null && _displays.Length > 0)
        {
            foreach (TMP_Text display in _displays)
            {
                if (display != null)
                {
                    display.color = customAssetData.CustomTextColor;
                }
            }
        }
    }
}