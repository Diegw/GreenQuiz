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
            _ID = course.ID;
            _courseType = course.CourseType;
            _name = course.Name;
            _questions = new List<Question>(course.Questions);
        }
    }
    public int ID => _ID;
    public EMenuCourse CourseType => _courseType;
    public string Name => _name;
    public string URL => _url;
    public List<Question> Questions => _questions;

    [SerializeField, DisplayAsString] private int _ID = -1;
    [SerializeField, OnValueChanged(nameof(SetID))] private EMenuCourse _courseType = EMenuCourse.NONE;
    [SerializeField] private string _name = "Course";
    [SerializeField] private string _url = "https://www.greentecher.com/";
    [SerializeField] private List<Question> _questions = new List<Question>();

    private void SetID()
    {
        _ID = (int)_courseType;
    }

    public Question GetQuestion(int questionIndex)
    {
        if(questionIndex < _questions.Count)
        {
            return _questions[questionIndex];
        }
        return new Question();
    }
}