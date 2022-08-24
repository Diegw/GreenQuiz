using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}