using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _display = null;
    [SerializeField] private Image _backImage;
    [SerializeField] private Image _highlightImage;
    [SerializeField] private Image _fillImage;
    [SerializeField] private Image _shadingImage;
    private SettingsAssets _assetsSettings = null;

    private void Awake()
    {
        _assetsSettings = SettingsManager.Assets;
        if(AreNullReferences())
        {
            return;
        }
    
        _display.color = _assetsSettings.InstructionsData.TextColor;
        SetSpriteToImage(_backImage, _assetsSettings.ButtonData.BackData.Sprite, _assetsSettings.ButtonData.BackData.Color);
        SetSpriteToImage(_highlightImage, _assetsSettings.ButtonData.HighlightData.Sprite, _assetsSettings.ButtonData.HighlightData.Color);
        SetSpriteToImage(_fillImage, _assetsSettings.ButtonData.FillData.Sprite, _assetsSettings.ButtonData.FillData.Color);
        SetSpriteToImage(_shadingImage, _assetsSettings.ButtonData.ShadingData.Sprite, _assetsSettings.ButtonData.ShadingData.Color);
    }

    private bool AreNullReferences()
    {
        if(_assetsSettings == null)
        {
            Debug.LogError("ButtonUI - assetsSettings is null");
            return true;
        }
        if(_display == null)
        {
            Debug.LogError("ButtonUI - display is null");
            return true;
        }
        if(_backImage == null)
        {
            Debug.LogError("ButtonUI - backImage is null");
            return true;
        }
        if(_highlightImage == null)
        {
            Debug.LogError("ButtonUI - highlightImage is null");
            return true;
        }
        if(_fillImage == null)
        {
            Debug.LogError("ButtonUI - fillImage is null");
            return true;
        }
        if(_shadingImage == null)
        {
            Debug.LogError("ButtonUI - shadingImage is null");
            return true;
        }
        return false;
    }

    private void SetSpriteToImage(Image image, Sprite sprite, Color color)
    {
        if(image == null)
        {
            return;
        }
        image.sprite = sprite;
        image.color = color;
    }
}