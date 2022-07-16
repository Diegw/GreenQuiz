public class ResultsData
{
    public ResultsData(string name, int answers, int questions)
    {
        _titleName = name;
        _correctAnswers = answers;
        _totalQuestions = questions;
    }
    public string TitleName => _titleName;
    public int CorrectAnswers => _correctAnswers;
    public int TotalQuestions => _totalQuestions;

    private string _titleName = string.Empty;
    private int _correctAnswers = 0;
    private int _totalQuestions = 0;
}