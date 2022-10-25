using System;
using UnityEngine;

[Serializable] public class SpriteCustom
{
    public Sprite Sprite => _sprite;
    public Color Color => _color;

    [SerializeField] private Sprite _sprite = null;
    [SerializeField] private Color _color = Color.white;
}