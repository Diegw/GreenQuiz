using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class SettingsAssets : ScriptableObject
{
    public BackgroundAssetData GameBackgroundData => _gameBackgroundData;
    
    [SerializeField] private BackgroundAssetData _gameBackgroundData;
}

[Serializable] public struct BackgroundAssetData
{
    public AssetData BackgroundData => _backgroundData;
    public AssetData DrawingData => _drawingData;

    [SerializeField] private AssetData _backgroundData;
    [SerializeField] private AssetData _drawingData;
}

[Serializable] public struct AssetData
{
    public Color Color => _color;
    public Sprite Sprite => _sprite;

    [SerializeField] private Color _color;
    [SerializeField] private Sprite _sprite;
}