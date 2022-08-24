using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SettingsAssets : SerializedScriptableObject
{
    [SerializeField, DictionaryDrawerSettings(KeyLabel = "ColorID", ValueLabel = "Color")] 
    private Dictionary<int, Color> _customAssetColors = new Dictionary<int, Color>();
    
    [SerializeField, DictionaryDrawerSettings(KeyLabel = "SpriteID", ValueLabel = "Sprite")] 
    private Dictionary<int, Sprite> _customAssetSprites = new Dictionary<int, Sprite>();

    public CustomAssetData GetCustomAssetData(CustomAssetID assetID)
    {
        return new CustomAssetData(GetCustomSprite(assetID.SpriteID), GetCustomColor(assetID.ColorID));
    }
    
    private Sprite GetCustomSprite(int spriteID)
    {
        if (_customAssetSprites == null || _customAssetSprites.Count <= 0 || !_customAssetSprites.ContainsKey(spriteID))
        {
            return null;
        }
        return _customAssetSprites[spriteID];
    }

    private Color GetCustomColor(int colorID)
    {
        if (_customAssetColors == null || _customAssetColors.Count <= 0 || !_customAssetColors.ContainsKey(colorID))
        {
            return Color.white;
        }
        return _customAssetColors[colorID];
    }
}

[Serializable] public struct CustomAssetID
{
    [field: SerializeField] public int SpriteID { get; }
    [field: SerializeField] public int ColorID { get; }
}

[Serializable] public struct CustomAssetData
{
    public CustomAssetData(Sprite customSprite, Color customColor)
    {
        CustomSprite = customSprite;
        CustomColor = customColor;
    }

    [field: SerializeField] public Sprite CustomSprite { get; }
    [field: SerializeField] public Color CustomColor { get; }
}