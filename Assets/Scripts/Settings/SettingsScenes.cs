using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SettingsScenes : SerializedScriptableObject
{
    public EScene FirstScene => _firstScene;
    
    [SerializeField] private EScene _firstScene = EScene.SPLASH;
    [SerializeField] private Dictionary<EScene, SceneData> _scenes = new Dictionary<EScene, SceneData>();

    public EScene GetSceneType(string sceneName)
    {
        EScene sceneType = EScene.NONE;
        if(sceneName != "" && _scenes != null)
        {
            foreach (KeyValuePair<EScene, SceneData> scene in _scenes)
            {
                if(scene.Value.Name == sceneName)
                {
                    sceneType = scene.Key;
                    break;
                }
            }
        }
        return sceneType;
    }

    public SceneData GetSceneData(EScene sceneType, EDirection direction = EDirection.NONE)
    {
        SceneData scene = null;
        if(DoesContainsSceneType(sceneType))
        {
            switch (direction)
            {
                case EDirection.NONE:
                {
                    scene = _scenes[sceneType];
                    break;
                }
                case EDirection.NEXT:
                {
                    EScene nextSceneType = _scenes[sceneType].NextScene;
                    if(DoesContainsSceneType(sceneType))
                    {
                        scene = _scenes[nextSceneType];
                    }
                    break;
                }
                case EDirection.PREVIOUS:
                {
                    EScene previousSceneType = _scenes[sceneType].PreviousScene;
                    if(DoesContainsSceneType(sceneType))
                    {
                        scene = _scenes[previousSceneType];
                    }
                    break;
                }
            }
        }
        return scene;
    }

    private bool DoesContainsSceneType(EScene sceneType)
    {
        if(_scenes != null && _scenes.ContainsKey(sceneType))
        {
            return true;
        }
        return false;
    }
}