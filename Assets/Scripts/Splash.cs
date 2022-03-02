using System;
using UnityEngine;

public class Splash : MonoBehaviour
{
    public static Action<EDirection> OnSceneFinishedEvent;

    private void Awake()
    {
        Debug.LogError("splash");
    }

    private void OnEnable()
    {
        Debug.LogError("splash enable");
        ExecutionManager.OnSetUpReadyEvent += Continue;
    }

    private void OnDisable()
    {
        ExecutionManager.OnSetUpReadyEvent -= Continue;
    }

    private void Continue()
    {
        //@TODO wait some time to see loading bar completed
        OnSceneFinishedEvent?.Invoke(EDirection.NEXT);
    }
}