using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable] public class Category
{
    public Category(){}
    public Category(Category category)
    {
        if(category != null)
        {
            _ID = category.ID;
            _name = category.Name;
            _categoryType = category.CategoryType;
            _courses = new List<CourseData>(category.CoursesData);
        }
    }
    public int ID => _ID;
    public EMenuCategory CategoryType => _categoryType;
    public string Name => _name;
    public List<CourseData> CoursesData => _courses;

    [SerializeField, DisplayAsString] private int _ID = -1;
    [SerializeField, OnValueChanged(nameof(SetID))] private EMenuCategory _categoryType = EMenuCategory.NONE;
    [SerializeField] private string _name = "Category";
    [SerializeField] private List<CourseData> _courses = new List<CourseData>();

    private void SetID()
    {
        _ID = (int)_categoryType;
    }

    public List<Course> Courses()
    {
        List<Course> courses = new List<Course>();
        foreach (CourseData courseData in _courses)
        {
            courses.Add(courseData.Course);
        }
        return courses;
    }
}
