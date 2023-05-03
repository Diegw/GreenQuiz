using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable] public class Course
{
    public Course(){}
    public Course(Course course = null)
    {
        if(course != null)
        {
            _courseType = course.CourseType;
            _questions = new List<Question>(course.Questions);
        }
    }
    public EMenuCourse CourseType => _courseType;
    public string URL => _url;
    public List<Question> Questions => _questions;
    
    [SerializeField] private EMenuCourse _courseType = EMenuCourse.NONE;
    [SerializeField] private string _url = "https://www.greentecher.com/";
    [SerializeField] private List<Question> _questions = new List<Question>();

    public Question GetQuestion(int questionIndex)
    {
        if(questionIndex < _questions.Count)
        {
            return _questions[questionIndex];
        }
        return new Question();
    }
}