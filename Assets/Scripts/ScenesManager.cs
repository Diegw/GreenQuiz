using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class ScenesManager : MonoBehaviour
{
    public static Action<EScene> OnSceneUnloadedEvent;
    public static Action<EScene> OnSceneLoadedEvent;

    [TabGroup("DEBUG"), SerializeField] private string _currentSceneName = "";
    [TabGroup("DEBUG"), SerializeField] private EScene _currentScene = EScene.NONE;
    private ScenesManager _instance = null;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Title.OnSceneFinishedEvent += LoadScene;
        // Menu.OnSceneFinishedEvent += LoadScene;
        // Results.OnSceneFinishedEvent += LoadScene;
        // Options.OnExitMatchEvent += LoadScene;
        // Options.OnExitGameEvent += ExitApplication;
        // Gameplay.OnRestartEvent += LoadScene;
        // Gameplay.OnMenuEvent += LoadScene;
        // MainMenu.OnExitEvent += ExitApplication;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Title.OnSceneFinishedEvent -= LoadScene;
        // Menu.OnSceneFinishedEvent -= LoadScene;
        // Results.OnSceneFinishedEvent -= LoadScene;
        // Options.OnExitMatchEvent -= LoadScene;
        // Options.OnExitGameEvent -= ExitApplication;
        // Gameplay.OnRestartEvent -= LoadScene;
        // Gameplay.OnMenuEvent -= LoadScene;
        // MainMenu.OnExitEvent -= ExitApplication;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        OnSceneUnloadedEvent?.Invoke(_currentScene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        _currentSceneName = scene.name;
        OnSceneLoadedEvent?.Invoke(_currentScene);
    }

    private void LoadScene(EDirection direction)
    {
        string sceneName = "";
        SceneManager.LoadScene(sceneName);
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
