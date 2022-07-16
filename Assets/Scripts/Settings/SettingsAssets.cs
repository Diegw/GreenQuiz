using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class SettingsAssets : ScriptableObject
{
    public BackgroundAssetData GameBackgroundData => _gameBackgroundData;
    public InstructionsAssetData InstructionsData => _instructionsData;
    public ButtonAssetData ButtonData => _buttonData;
    
    [SerializeField, HideLabel, FoldoutGroup("Game Background")] private BackgroundAssetData _gameBackgroundData;
    [SerializeField, HideLabel, FoldoutGroup("Instructions")] private InstructionsAssetData _instructionsData;
    [SerializeField, HideLabel, FoldoutGroup("Button")] private ButtonAssetData _buttonData;
}

[Serializable] public struct BackgroundAssetData
{
    public AssetData BackgroundData => _backgroundData;
    public AssetData DrawingData => _drawingData;

    [SerializeField] private AssetData _backgroundData;
    [SerializeField] private AssetData _drawingData;
}

[Serializable] public struct InstructionsAssetData
{
    public Color TextColor => _textColor;
    public AssetData BackBackgroundData => _backBackgroundData;
    public AssetData MiddleBackgroundData => _middleBackgroundData;
    public AssetData FrontBackgroundData => _frontBackgroundData;

    [SerializeField] private Color _textColor;
    [SerializeField] private AssetData _backBackgroundData;
    [SerializeField] private AssetData _middleBackgroundData;
    [SerializeField] private AssetData _frontBackgroundData;
}

[Serializable] public struct ButtonAssetData
{
    public Color TextColor => _textColor;
    public AssetData BackData => _backBackgroundData;
    public AssetData HighlightData => _highlightBackgroundData;
    public AssetData FillData => _fillBackgroundData;
    public AssetData ShadingData => _shadingBackgroundData;

    [SerializeField] private Color _textColor;
    [SerializeField] private AssetData _backBackgroundData;
    [SerializeField] private AssetData _highlightBackgroundData;
    [SerializeField] private AssetData _fillBackgroundData;
    [SerializeField] private AssetData _shadingBackgroundData;
}

[Serializable] public struct AssetData
{
    public Color Color => _color;
    public Sprite Sprite => _sprite;

    [SerializeField] private Color _color;
    [SerializeField] private Sprite _sprite;
}