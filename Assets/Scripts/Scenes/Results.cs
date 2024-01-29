using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Results : SerializedMonoBehaviour
{
    public static Action<EDirection> OnSceneFinishedEvent;
    public static Action<ResultsData> OnResultsDataReadyEvent;

    [TabGroup("DEBUG"), SerializeField] private int _totalCorrectAnswers = 0;
    [TabGroup("DEBUG"), SerializeField] private Dictionary<EMenuCourse, Vector3Int> _coursesResults = new Dictionary<EMenuCourse, Vector3Int>();
    private SettingsQuestion _settingsQuestion = null;
    private SettingsMenu _settingsMenu = null;

    private void Awake()
    {
        _settingsQuestion = SettingsManager.Question;
        _settingsMenu = SettingsManager.Menu;
    }

    private void OnEnable()
    {
        Gameplay.OnChoiceSelectedEvent += UpdateResults;
        Gameplay.OnMatchEndedEvent += PrepareResults;
        ResultsUI.OnContinueButtonPressedEvent += Continue;
    }

    private void OnDisable()
    {
        Gameplay.OnChoiceSelectedEvent -= UpdateResults;
        Gameplay.OnMatchEndedEvent -= PrepareResults;
        ResultsUI.OnContinueButtonPressedEvent -= Continue;
    }

    private void UpdateResults(Question question, Choice choice)
    {
        int correctAnswer = choice.IsCorrect ? 1 : 0;
        _totalCorrectAnswers += correctAnswer;
        if(_settingsQuestion == null)
        {
            return;
        }
        EMenuCourse courseType = _settingsQuestion.GetCourseType(GameManager.Category, question.ID);
        if(_coursesResults != null)
        {
            if(!_coursesResults.ContainsKey(courseType))
            {
                _coursesResults.Add(courseType, Vector3Int.zero);
            }
            Vector3Int courseResult = _coursesResults[courseType];
            Vector3Int newCourseResult = new Vector3Int(courseResult.x + correctAnswer, courseResult.y + 1, 0);
            _coursesResults[courseType] = new Vector3Int(newCourseResult.x, newCourseResult.y, newCourseResult.x /newCourseResult.y);
        }
    }

    private void PrepareResults()
    {
        string title = _settingsMenu.GetCategoryName(GameManager.Category);
        if (GameManager.Mode == EMenuMode.MANUAL)
        {
            title = _settingsMenu.GetCourseName(GameManager.Course);
        }

        EMenuCourse secondaryCourse1 = SettingsManager.Menu.GetRandomCourse(GameManager.Category);
        while (secondaryCourse1 == GameManager.Course)
        {
            secondaryCourse1 = SettingsManager.Menu.GetRandomCourse(GameManager.Category);
        }
        EMenuCourse secondaryCourse2 = SettingsManager.Menu.GetRandomCourse(GameManager.Category);
        while (secondaryCourse2 == GameManager.Course || secondaryCourse2 == secondaryCourse1)
        {
            secondaryCourse2 = SettingsManager.Menu.GetRandomCourse(GameManager.Category);
        }
        ResultsData resultsData = new ResultsData(title, _totalCorrectAnswers, _settingsQuestion.QuestionsPerMatch,
                                                    secondaryCourse1, secondaryCourse2);
        OnResultsDataReadyEvent?.Invoke(resultsData);
    }

    private void Continue()
    {        
        OnSceneFinishedEvent?.Invoke(EDirection.NEXT);
    }
}