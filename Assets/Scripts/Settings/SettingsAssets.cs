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
        return new CustomAssetData(GetCustomSprite(assetID.SpriteID),
                                GetCustomColor(assetID.ColorSpriteID),
                                GetCustomColor(assetID.ColorTextID));
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
    public CustomAssetID(int spriteID, int colorSpriteID, int colorTextID)
    {
        _spriteID = spriteID;
        _colorSpriteID = colorSpriteID;
        _colorTextID = colorTextID;
    }

    public int SpriteID => _spriteID;
    public int ColorSpriteID => _colorSpriteID; 
    public int ColorTextID => _colorTextID;

    [SerializeField] private int _spriteID;
    [SerializeField] private int _colorSpriteID;
    [SerializeField] private int _colorTextID;
}

[Serializable] public struct CustomAssetData
{
    public CustomAssetData(Sprite customSprite, Color customSpriteColor, Color customTextColor)
    {
        CustomSprite = customSprite;
        CustomSpriteColor = customSpriteColor;
        CustomTextColor = customTextColor;
    }

    [field: SerializeField] public Sprite CustomSprite { get; }
    [field: SerializeField] public Color CustomSpriteColor { get; }
    [field: SerializeField] public Color CustomTextColor { get; }
}