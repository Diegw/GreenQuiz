using UnityEngine;

public class SettingsTitle : ScriptableObject
{
    public SpriteCustom BackgroundSprite
    { 
        get => _backgroundSprite; 
    }
    public SpriteCustom LogoSprite => _logoSprite;

    [SerializeField] private SpriteCustom _backgroundSprite = null;
    [SerializeField] private SpriteCustom _logoSprite = null;
}