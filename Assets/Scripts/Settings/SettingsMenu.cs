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
            newMenuState = _nextStates[currentState];
            if (currentState == EMenuState.MODES && GameManager.Mode == EMenuMode.RANDOM)
            {
                newMenuState = EMenuState.GAMEPLAY;
            }
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

    public EMenuCategory GetFirstCategory()
    {
        EMenuCategory firstCategory = EMenuCategory.NONE;
        int index = 0;
        foreach (EMenuCategory category in _categories.Keys)
        {
            firstCategory = category;
            if (index == 1)
            {
                break;
            }
            index++;
        }
        return firstCategory;
    }
    
    public EMenuCategory[] GetCategories()
    {
        EMenuCategory[] categories = new EMenuCategory[_categories.Count];
        int index = 0;
        foreach (var category in _categories.Keys)
        {
            categories[index] = category;
            index++;
        }
        return categories;
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

    public EMenuMode GetFirstMode()
    {
        EMenuMode firstMode = EMenuMode.NONE;
        int index = 0;
        foreach (EMenuMode mode in _modes.Keys)
        {
            firstMode = mode;
            if (index == 1)
            {
                break;
            }
            index++;
        }
        return firstMode;
    }
    
    public EMenuMode[] GetModes()
    {
        EMenuMode[] modes = new EMenuMode[_modes.Count];
        int index = 0;
        foreach (var mode in _modes.Keys)
        {
            modes[index] = mode;
            index++;
        }
        return modes;
    }
#endregion

#region COURSES
    public string GetCourseName(EMenuCourse course)
    {
        string courseName = "";
        if (_courses != null && _courses.ContainsKey(course))
        {
            courseName = _courses[course].Name;
        }
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

    public EMenuCourse GetFirstCourse(EMenuCategory category)
    {
        EMenuCourse firstCourse = EMenuCourse.NONE;
        EMenuCourse[] courses = GetCategoryCourses(category);
        int index = 0;
        foreach (EMenuCourse course in courses)
        {
            firstCourse = course;
            if (index == 1)
            {
                break;
            }
            index++;
        }
        return firstCourse;
    }
    
    public EMenuCourse[] GetCategoryCourses(EMenuCategory category)
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