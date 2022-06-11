using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SettingsMenu : SerializedScriptableObject
{
    [SerializeField] private Dictionary<EMenuState, EMenuState> _nextStates = new Dictionary<EMenuState, EMenuState>();
    [SerializeField] private Dictionary<EMenuState, string> _instructionsStates = new Dictionary<EMenuState, string>();
    [SerializeField] private Dictionary<EMenuCategory, MenuStateUI> _categories = new Dictionary<EMenuCategory, MenuStateUI>();
    [SerializeField] private Dictionary<EMenuMode, MenuStateUI> _modes = new Dictionary<EMenuMode, MenuStateUI>();
    [SerializeField] private Dictionary<EMenuCourse, MenuStateUI> _courses = new Dictionary<EMenuCourse, MenuStateUI>();
    [SerializeField] private Dictionary<EMenuCategory, EMenuCourse[]> _coursesPerCategory = new Dictionary<EMenuCategory, EMenuCourse[]>();
    
    public EMenuState NewState(EMenuState currentState)
    {
        EMenuState newMenuState = EMenuState.NONE;
        if(_nextStates != null && _nextStates.ContainsKey(currentState))
        {
            newMenuState = _nextStates[currentState];;
        }
        return newMenuState;
    }

    public string GetStateInstructions(EMenuState state)
    {
        string instructions = "";
        if(_instructionsStates != null && _instructionsStates.ContainsKey(state))
        {
            instructions = _instructionsStates[state];
        }
        return instructions;
    }

#region CATEGORIES
    public int GetCategoriesCount()
    {
        return _categories.Count;
    }

    public Sprite[] GetCategoriesSprites()
    {
        Sprite[] sprites = new Sprite[_categories.Count];
        int index = 0;
        foreach (var category in _categories.Values)
        {
            sprites[index] = category.Image;
            index++;
        }
        return sprites;
    }

    public string GetCategoryName(EMenuCategory categoryType)
    {
        string categoryName = "";
        if(_categories == null || !_categories.ContainsKey(categoryType))
        {
            return categoryName;
        }
        categoryName = _categories[categoryType].Name;
        return categoryName;
    }
#endregion

#region MODES
    public int GetModesCount()
    {
        return _modes.Count;
    }

    public Sprite[] GetModesSprites()
    {
        Sprite[] sprites = new Sprite[_modes.Count];
        int index = 0;
        foreach (var mode in _modes.Values)
        {
            sprites[index] = mode.Image;
            index++;
        }
        return sprites;
    }

    public string GetModeName(EMenuMode modeType)
    {
        string modeName = "";
        if(_modes == null || !_modes.ContainsKey(modeType))
        {
            return modeName;
        }
        modeName = _modes[modeType].Name;
        return modeName;
    }
#endregion

#region COURSES
    public string GetCourseName(EMenuCourse course)
    {
        string courseName = "";
        return courseName;
    }

    public int GetCoursesCount(EMenuCategory category)
    {
        EMenuCourse[] courses = GetCategoryCourses(category);
        return courses == null ? 0 : courses.Length;
    }

    public Sprite[] GetCoursesSprites(EMenuCategory category)
    {
        Sprite[] sprites = null;
        EMenuCourse[] courses = GetCategoryCourses(category);
        if(courses != null && _courses != null)
        {
            sprites = new Sprite[courses.Length];
            for (int i = 0; i < courses.Length; i++)
            {
                sprites[i] = _courses[courses[i]].Image;
            }
        }
        return sprites;
    }

    private EMenuCourse[] GetCategoryCourses(EMenuCategory category)
    {
        EMenuCourse[] courses = null;
        if(_coursesPerCategory != null && _coursesPerCategory.ContainsKey(category))
        {
            courses = _coursesPerCategory[category];
        }
        return courses;
    }
#endregion

}

public class MenuStateUI
{
    public string Name => _name;
    public Sprite Image => _image;

    [SerializeField] private string _name;
    [SerializeField] private Sprite _image;
}