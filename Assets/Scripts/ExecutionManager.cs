using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MEC;

public class ExecutionManager : SerializedMonoBehaviour
{
    public static Action OnSetUpReadyEvent;

    [SerializeField] private List<IManager> _managers = new List<IManager>();
    private static ExecutionManager _instance = null;

    private void Start()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(_instance);
        Timing.RunCoroutine(WaitForFirstFrame());
    }

    private IEnumerator<float> WaitForFirstFrame()
    {
        yield return Timing.WaitForOneFrame;
        if(_managers == null || _managers.Count <= 0)
        {
            yield break;
        }
        foreach (IManager manager in _managers)
        {
            Debug.Log($"MANAGER: {manager}");
            manager.Contruct();
            manager.Activate();
        }
        OnSetUpReadyEvent?.Invoke();
    }
}