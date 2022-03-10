using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class ScenesManager : MonoBehaviour, IManager
{
    public static Action<EScene> OnSceneUnloadedEvent;
    public static Action<EScene> OnSceneLoadedEvent;

    [TabGroup("DEBUG"), SerializeField] private string _currentSceneName = "";
    [TabGroup("DEBUG"), SerializeField] private EScene _currentSceneType = EScene.NONE;
    private ScenesManager _instance = null;
    private SettingsScenes _settingsScenes = null;

    public void Contruct()
    {
        _settingsScenes = SettingsManager.Scenes;
        if(_settingsScenes == null)
        {
            return;
        }
        SetCurrentSceneData(_settingsScenes.GetSceneData(_settingsScenes.FirstScene)?.Name);
    }

    public void Activate()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Splash.OnSceneFinishedEvent += LoadScene;
        Title.OnSceneFinishedEvent += LoadScene;
        Menu.OnSceneFinishedEvent += LoadScene;
        // Results.OnSceneFinishedEvent += LoadScene;
        // Options.OnExitMatchEvent += LoadScene;
        // Options.OnExitGameEvent += ExitApplication;
        // Gameplay.OnRestartEvent += LoadScene;
        // Gameplay.OnMenuEvent += LoadScene;
        // MainMenu.OnExitEvent += ExitApplication;
    }

    public void Deactivate()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Splash.OnSceneFinishedEvent -= LoadScene;
        Title.OnSceneFinishedEvent -= LoadScene;
        Menu.OnSceneFinishedEvent -= LoadScene;
        // Results.OnSceneFinishedEvent -= LoadScene;
        // Options.OnExitMatchEvent -= LoadScene;
        // Options.OnExitGameEvent -= ExitApplication;
        // Gameplay.OnRestartEvent -= LoadScene;
        // Gameplay.OnMenuEvent -= LoadScene;
        // MainMenu.OnExitEvent -= ExitApplication;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        OnSceneUnloadedEvent?.Invoke(_currentSceneType);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        SetCurrentSceneData(scene.name);
        OnSceneLoadedEvent?.Invoke(_currentSceneType);
    }

    private void SetCurrentSceneData(string sceneName)
    {
        _currentSceneName = sceneName;
        if (_settingsScenes != null)
        {
            _currentSceneType = _settingsScenes.GetSceneType(sceneName);
        }
    }

    private void LoadScene(EDirection direction)
    {
        if(_settingsScenes == null)
        {
            Debug.LogError("Couldnt get sceneData because SettingsScenes is null");
            return;
        }
        SceneData scene = _settingsScenes.GetSceneData(_currentSceneType, direction);
        if(scene == null || scene.Name == "")
        {
            Debug.LogError("Couldnt get valid sceneData");
            return;
        }
        SceneManager.LoadScene(scene.Name);
    }

    private void ExitApplication()
    {
        if(Application.isEditor)
        {
            return;
        }
        //@TODO destroy everything and wait until is destroyed then leave
        Application.Quit();
    }
}
