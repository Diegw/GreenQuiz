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

    public Sprite GetCategorySprite(EMenuCategory categoryType)
    {
        if(_categories == null || !_categories.ContainsKey(categoryType))
        {
            return null;
        }
        Sprite categorySprite = _categories[categoryType].Image;
        return categorySprite;
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

    public Sprite GetModeSprite(EMenuMode modeType)
    {
        if(_modes == null || !_modes.ContainsKey(modeType))
        {
            return null;
        }
        Sprite modeSprite = _modes[modeType].Image;
        return modeSprite;
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
    public Sprite GetCourseSprite(EMenuCourse course)
    {
        Sprite courseSprite = null;
        if (_courses != null && _courses.ContainsKey(course))
        {
            courseSprite = _courses[course].Image;
        }
        return courseSprite;
    }
    
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

    public EMenuCourse GetRandomCourse(EMenuCategory category)
    {
        EMenuCourse[] courses = GetCategoryCourses(category);
        EMenuCourse randomCourse = courses[Random.Range(0, courses.Length)];
        return randomCourse;
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
    
    public EMenuCourse GetNewCourse(bool next, EMenuCourse course,EMenuCategory category)
    {
        if (_coursesPerCategory == null || !_coursesPerCategory.ContainsKey(category))
        {
            return EMenuCourse.NONE;
        }
        EMenuCourse[] courses = _coursesPerCategory[category];
        int newIndex = 0;
        EMenuCourse newCourse = EMenuCourse.NONE;
        for (int i = 0; i < courses.Length; i++)
        {
            newIndex = next ? 
                i + 1 >= courses.Length ? 0 : i+1 : 
                i -1 < 0 ? courses.Length-1 : i-1;

            if (newIndex < courses.Length && newIndex >= 0 && courses[i] == course)
            {
                newCourse = courses[newIndex];
                break;
            }
        }
        return newCourse;
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