using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCustom : MonoBehaviour
{
    public Sprite Sprite => _sprite;
    public Color Color => _color;

    [SerializeField] private Sprite _sprite = null;
    [SerializeField] private Color _color = Color.white;
}