using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable] public class CourseData
{
    public CourseData(){}
    public CourseData(CourseData courseData = null)
    {
        if(courseData != null)
        {
            _course = new Course(courseData.Course);
        }
    }
    public Course Course => _course;

    private string _courseName = "Course";
    [FoldoutGroup("@_courseName"), SerializeField, HideLabel, OnValueChanged(nameof(SetCourseName),true)] private Course _course = null;

    private void SetCourseName()
    {
        _courseName = _course.CourseType.ToString();
    }
}