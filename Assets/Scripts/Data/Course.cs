using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Course
{
    public Course(){}
    public Course(Course course = null)
    {
        if(course != null)
        {
            _courseType = course.CourseType;
            _questionsData = new List<QuestionData>(course.QuestionsData);
        }
    }
    public EMenuCourse CourseType => _courseType;
    public List<QuestionData> QuestionsData => _questionsData;
    
    [SerializeField] private EMenuCourse _courseType = EMenuCourse.NONE;
    [SerializeField] private List<QuestionData> _questionsData = new List<QuestionData>();

    public List<Question> GetQuestions()
    {
        List<Question> questions = new List<Question>();
        foreach (QuestionData questionData in _questionsData)
        {
            if (questionData == null || questionData.Question == null)
            {
                continue;
            }
            questions.Add(questionData.Question);
        }
        return questions;
    }

    public Question GetQuestion(int questionIndex)
    {
        if(questionIndex < _questionsData.Count)
        {
            return _questionsData[questionIndex].Question;
        }
        return new Question();
    }
}