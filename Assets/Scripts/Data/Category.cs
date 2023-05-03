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
            _categoryType = category.CategoryType;
            _questionsPerCourses = new List<CourseData>(category.QuestionsPerCourses);
        }
    }
    public EMenuCategory CategoryType => _categoryType;
    public List<CourseData> QuestionsPerCourses => _questionsPerCourses;
    
    [SerializeField] private EMenuCategory _categoryType = EMenuCategory.NONE;
    [SerializeField] private List<CourseData> _questionsPerCourses = new List<CourseData>();

    public List<Course> Courses()
    {
        List<Course> courses = new List<Course>();
        foreach (CourseData courseData in _questionsPerCourses)
        {
            courses.Add(courseData.Course);
        }
        return courses;
    }
}
