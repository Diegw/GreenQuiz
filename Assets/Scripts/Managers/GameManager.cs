using UnityEngine;

public class GameManager : MonoBehaviour, IManager
{
    public static EMenuCategory Category => _category;
    public static EMenuMode Mode => _mode;
    public static EMenuCourse Course => _course;

    private static EMenuCategory _category = EMenuCategory.NONE;
    private static EMenuMode _mode = EMenuMode.NONE;
    private static EMenuCourse _course = EMenuCourse.NONE;

    [SerializeField] private EMenuCategory _currentCategory = EMenuCategory.NONE;
    [SerializeField] private EMenuMode _currentMode = EMenuMode.NONE;
    [SerializeField] private EMenuCourse _currentCourse = EMenuCourse.NONE;
    private static GameManager _instance = null;

    public void Contruct()
    {
        _instance = this;
    }

    public void Activate()
    {
    }

    public void Deactivate()
    {
    }

    public static void SetCategory(EMenuCategory category)
    {
        _category = category;
        if(_instance != null)
        {
            _instance._currentCategory = category;
        }
    }

    public static void SetGameMode(EMenuMode mode)
    {
        _mode = mode;
        if(_instance != null)
        {
            _instance._currentMode = mode;
        }
    }

    public static void SetCourse(EMenuCourse course)
    {
        _course = course;
        if(_instance != null)
        {
            _instance._currentCourse = course;
        }
    }
}