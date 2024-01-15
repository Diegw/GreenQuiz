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
        if(_questionsPerCategory == null || _isUpdating)
        {
            return;
        }
        _isUpdating = true;
        int id = 0;
        foreach (CategoryData categoryData in _questionsPerCategory)
        {
            if(categoryData == null || categoryData.Category == null || categoryData.Category.QuestionsPerCourses == null)
            {
                continue;
            }
            foreach (CourseData courseData in categoryData.Category.QuestionsPerCourses)
            {
                if(courseData.Course == null || courseData.Course.QuestionsData == null)
                {
                    continue;
                }
                foreach (QuestionData questionData in courseData.Course.QuestionsData)
                {
                    if(questionData == null || questionData.Question == null)
                    {
                        continue;
                    }
                    questionData.Question.SetID(id);
                    questionData.SetQuestionName();
                    id++;
                }
            }
        }
        _isUpdating = false;
    }
    [Min(5f), SerializeField] private int _questionsPerMatch = 15;
    [Min(5f), SerializeField] private float _timePerQuestion = 15f;
    [SerializeField] private List<CategoryData> _questionsPerCategory = new List<CategoryData>();
    [SerializeField, HideLabel] private CourseUrlList _courseUrlList = new CourseUrlList();

    public Stack<Question> GetQuestions(EMenuCategory categoryType, EMenuMode menuModeType, EMenuCourse courseType)
    {
        Stack<Question> randomQuestions = new Stack<Question>();
        if(_questionsPerMatch <= 0)
        {
            return randomQuestions;
        }
        Category category = GetCategory(categoryType);
        List<Course> courses = GetCourses(category, menuModeType, courseType);

        if (courses != null && courses.Count > 0)
        {
            List<Question> questions = new List<Question>();
            for (int i = 0; i < _questionsPerMatch; i++)
            {
                int randomCourseIndex = UnityEngine.Random.Range(0, courses.Count);
                questions = courses[randomCourseIndex].GetQuestions();
                int randomQuestionIndex = UnityEngine.Random.Range(0, questions.Count);
                randomQuestions.Push(new Question(questions[randomQuestionIndex]));
            }
        }
        return randomQuestions;
    }

    public Category GetCategory(EMenuCategory categoryType)
    {
        Category newCategory = new Category();
        foreach (CategoryData data in _questionsPerCategory)
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
            if(courseType != EMenuCourse.NONE && category.QuestionsPerCourses != null && category.QuestionsPerCourses.Count > 0)
            {
                foreach (CourseData courseData in category.QuestionsPerCourses)
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
            if(category.QuestionsPerCourses != null && category.QuestionsPerCourses.Count > 0)
            {
                foreach (CourseData courseData in category.QuestionsPerCourses)
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
        if(category.QuestionsPerCourses != null && category.QuestionsPerCourses.Count > 0)
        {
            foreach (CourseData courseData in category.QuestionsPerCourses)
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
        if(category != null && category.QuestionsPerCourses != null)
        {
            foreach (CourseData courseData in category.QuestionsPerCourses)
            {
                if(courseData.Course == null || courseData.Course.QuestionsData == null)
                {
                    continue;
                }
                foreach (QuestionData questionData in courseData.Course.QuestionsData)
                {
                    if(questionData == null || questionData.Question == null)
                    {
                        continue;
                    }
                    if(questionData.Question.ID == questionID)
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