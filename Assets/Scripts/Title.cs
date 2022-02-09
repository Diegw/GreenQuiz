using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviourCustom
{
    [SerializeField] private Image _background = null;
    [SerializeField] private Image _logo = null;
    [SerializeField] private ButtonCustom _continueButton = null;

    private void Awake()
    {
        if(AreThereNullReferences(_background, _logo, _continueButton))
        {
            return;
        }
        Settings();
    }

    private void Settings()
    {
        
    }
}