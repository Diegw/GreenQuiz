using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable] public class ButtonCustom
{
    public Button Button => _button;
    public TMP_Text Display => _display;

    [SerializeField] private Button _button = null;
    [SerializeField] private TMP_Text _display = null;
}