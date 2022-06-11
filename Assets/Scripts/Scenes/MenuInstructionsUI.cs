using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuInstructionsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _display = null;
    [SerializeField] private Image _backImage = null;
    [SerializeField] private Image _middleImage = null;
    [SerializeField] private Image _frontImage = null;
    private SettingsAssets _assetsSettings = null;

    private void Awake()
    {
        _assetsSettings = SettingsManager.Assets;
        if(AreNullReferences())
        {
            return;
        }

        _display.color = _assetsSettings.InstructionsData.TextColor;
        SetSpriteToImage(_backImage, _assetsSettings.InstructionsData.BackBackgroundData.Sprite, _assetsSettings.InstructionsData.BackBackgroundData.Color);
        SetSpriteToImage(_middleImage, _assetsSettings.InstructionsData.MiddleBackgroundData.Sprite, _assetsSettings.InstructionsData.MiddleBackgroundData.Color);
        SetSpriteToImage(_frontImage, _assetsSettings.InstructionsData.FrontBackgroundData.Sprite, _assetsSettings.InstructionsData.FrontBackgroundData.Color);
    }

    private bool AreNullReferences()
    {
        if(_assetsSettings == null)
        {
            Debug.LogError("MenuInstructionsUI - assetsSettings is null");
            return true;
        }
        if(_display == null)
        {
            Debug.LogError("MenuInstructionsUI - display is null");
            return true;
        }
        if(_backImage == null)
        {
            Debug.LogError("MenuInstructionsUI - backImage is null");
            return true;
        }
        if(_middleImage == null)
        {
            Debug.LogError("MenuInstructionsUI - middleImage is null");
            return true;
        }
        if(_frontImage == null)
        {
            Debug.LogError("MenuInstructionsUI - frontImage is null");
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