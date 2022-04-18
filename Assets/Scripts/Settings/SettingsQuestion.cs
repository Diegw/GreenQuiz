using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SettingsQuestion : SerializedScriptableObject
{
    public float TimePerQuestion => _timePerQuestion;
    public int QuestionsPerMatch => _questionsPerMatch;

    private bool _isUpdating = false;
    [SerializeField, Button("Update Questions ID")] private void UpdateIDs()
    {
        if(_categories == null || _isUpdating)
        {
            return;
        }
        _isUpdating = true;
        int id = 0;
        foreach (CategoryData categoryData in _categories)
        {
            if(categoryData == null || categoryData.Category == null || categoryData.Category.CoursesData == null)
            {
                continue;
            }
            foreach (CourseData courseData in categoryData.Category.CoursesData)
            {
                if(courseData.Course == null || courseData.Course.Questions == null)
                {
                    continue;
                }
                foreach (Question question in courseData.Course.Questions)
                {
                    if(question == null)
                    {
                        continue;
                    }
                    question.SetID(id);
                    id++;
                }
            }
        }
        _isUpdating = false;
    }
    [Min(5f), SerializeField] private int _questionsPerMatch = 15;
    [Min(5f), SerializeField] private float _timePerQuestion = 15f;
    [SerializeField] private List<CategoryData> _categories = new List<CategoryData>();

    public Stack<Question> GetQuestions(EMenuCategory categoryType, EMenuMode menuModeType, EMenuCourse courseType)
    {
        if(_questionsPerMatch <= 0)
        {
            return null;
        }
        Stack<Question> randomQuestions = new Stack<Question>();
        Category category = GetCategory(categoryType);
        List<Course> courses = GetCourses(category, menuModeType, courseType);
        
        List<Question> questions = new List<Question>();
        for (int i = 0; i < _questionsPerMatch; i++)
        {
            int randomCourseIndex = UnityEngine.Random.Range(0, courses.Count);
            questions = courses[randomCourseIndex].Questions;
            int randomQuestionIndex = UnityEngine.Random.Range(0, questions.Count);
            randomQuestions.Push(new Question(questions[randomQuestionIndex]));
        }
        return randomQuestions;
    }

    public Category GetCategory(EMenuCategory categoryType)
    {
        Category newCategory = new Category();
        foreach (CategoryData data in _categories)
        {
            if(data.Category.CategoryType == categoryType)
            {
                newCategory = new Category(data.Category);
                break;
            }
        }
        return newCategory;
    }

    private List<Course> GetCourses(Category category, EMenuMode gameMode, EMenuCourse courseType)
    {
        List<Course> courses = new List<Course>();
        if(gameMode == EMenuMode.MANUAL)
        {
            if(courseType != EMenuCourse.NONE && category.CoursesData != null && category.CoursesData.Count > 0)
            {
                foreach (CourseData courseData in category.CoursesData)
                {
                    if(courseData.Course.CourseType == courseType)
                    {
                        courses.Add(GetCourse(category, courseType));
                        break;
                    }
                }
            }
        }
        else
        {
            if(category.CoursesData != null && category.CoursesData.Count > 0)
            {
                foreach (CourseData courseData in category.CoursesData)
                {
                    courses.Add(new Course(courseData.Course));
                }
            }
        }
        return courses;
    }

    public Course GetCourse(Category category, EMenuCourse courseType)
    {
        Course newCourse = new Course();
        if(category.CoursesData != null && category.CoursesData.Count > 0)
        {
            foreach (CourseData courseData in category.CoursesData)
            {
                if(courseData.Course.CourseType == courseType)
                {
                    newCourse = new Course(courseData.Course);
                    break;
                }
            }
        }
        return newCourse;
    }

    public EMenuCourse GetCourseType(EMenuCategory categoryType, int questionID)
    {
        EMenuCourse courseType = EMenuCourse.NONE;
        Category category = GetCategory(categoryType);
        if(category != null && category.CoursesData != null)
        {
            foreach (CourseData courseData in category.CoursesData)
            {
                if(courseData.Course == null || courseData.Course.Questions == null)
                {
                    continue;
                }
                foreach (Question question in courseData.Course.Questions)
                {
                    if(question.ID == questionID)
                    {
                        courseType = courseData.Course.CourseType;
                        break;
                    }
                }
                if(courseType != EMenuCourse.NONE)
                {
                    break;
                }
            }
        }
        return courseType;
    }
}