using System;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable] public class SceneData
{
    public EScene PreviousScene => _previousScene;
    public EScene NextScene => _nextScene;
    public string Name => _name;

    [SerializeField] private EScene _previousScene = EScene.NONE;
    [SerializeField] private EScene _nextScene = EScene.NONE;
    [SerializeField] private string _name = "";
#if UNITY_EDITOR
    [SerializeField, OnValueChanged(nameof(SetData))] private SceneAsset _unityScene = null;

    private void SetData()
    {
        if(_unityScene != null)
        {
            _name = _unityScene.name;
        }
    }
#endif
}