using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour, IManager
{
    [SerializeField] private Image _background = null;
    [SerializeField] private Image _drawing = null;
    private SettingsAssets _settingsAssets = null;

    public void Contruct()
    {
        _settingsAssets = SettingsManager.Assets;
        if(_settingsAssets == null)
        {
            Debug.LogError("Assets Settings is null");
        }
        if(_background == null)
        {
            Debug.LogError("BackgroundManager - Background Image is null");
        }
        if(_drawing == null)
        {
            Debug.LogError("BackgroundManager - Drawing Image is null");
        }
    }

    public void Activate()
    {
        SetGameBackground();
    }

    public void Deactivate()
    {
    }

    private void SetGameBackground()
    {
        if(_settingsAssets == null)
        {
            return;
        }

        Sprite backgroundSprite = _settingsAssets.GameBackgroundData.BackgroundData.Sprite;
        Color backgroundColor = _settingsAssets.GameBackgroundData.BackgroundData.Color;
        SetSpriteToImage(_background, backgroundSprite, backgroundColor);

        Sprite drawingSprite = _settingsAssets.GameBackgroundData.DrawingData.Sprite;
        Color drawingColor = _settingsAssets.GameBackgroundData.DrawingData.Color;
        SetSpriteToImage(_drawing, drawingSprite, drawingColor);
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