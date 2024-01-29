public class ResultsData
{
    public ResultsData(string name, int answers, int questions, EMenuCourse course1, EMenuCourse course2)
    {
        _titleName = name;
        _correctAnswers = answers;
        _totalQuestions = questions;
        _course1 = course1;
        _course2 = course2;
    }
    public string TitleName => _titleName;
    public int CorrectAnswers => _correctAnswers;
    public int TotalQuestions => _totalQuestions;
    public EMenuCourse Course1 => _course1;
    public EMenuCourse Course2 => _course2;

    private string _titleName = string.Empty;
    private int _correctAnswers = 0;
    private int _totalQuestions = 0;
    private EMenuCourse _course1 = EMenuCourse.NONE;
    private EMenuCourse _course2 = EMenuCourse.NONE;
}