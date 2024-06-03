using System;
using System.Collections.Generic;
using UnityEngine;
using Npgsql;
using System.Linq;

public static class DatabaseConnector
{
    public static string IdNowUser = "";
    public static string UsernName;
    public static string Password;
    public static string IdCources;
    public static string PlayerHero;
    public static int MaxQuantityQuestion;
    public static int ProgressPoint;

    private static string _connectionString =
            "Port = 5432;" +
            "Server=localhost;" +
            "Database=postgres;" +
            "User ID=postgres;" +
            "Password=meow;";

    private static NpgsqlConnection _dbcon;

    private static NpgsqlCommand _dbcmd;

    private static NpgsqlDataReader _reader;

    //Добавлено 
    public static bool Verification(string userName, string password)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        bool result = false;
        string sql =
            "SELECT \"id\" " +
            $"FROM users WHERE username = \'{userName}\' AND password = \'{password}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        result = _reader.Read();

        if (result)
            IdNowUser = _reader[0].ToString();

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    public static bool TryVerification()
    {
        return IdNowUser == "";
    }

    public static void AddCourcesId(string id)
    {
        IdCources = id;

        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();

        string sql =
            "SELECT progresspoint " +
            $"FROM progress WHERE \"iduser\" = \'{IdNowUser}\' AND \"idcources\" = \'{id}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        if (_reader.Read())
        {
            ProgressPoint = Int32.Parse(_reader[0].ToString());
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;
    }

    //Добавлено
    public static List<string> AllCoursesUserTakes()
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<string> result = new();

        string sql =
            "SELECT idcources " +
            $"FROM progress WHERE \"iduser\" = \'{IdNowUser}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i++)
            {
                result.Add(_reader[i].ToString());
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    //добавлено
    public static List<string> AllCoursesUserAuthor()
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<string> result = new();

        string sql =
            "SELECT id " +
            $"FROM \"Сources\" WHERE \"author_id\" = \'{IdNowUser}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i++)
            {
                result.Add(_reader[i].ToString());
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    //добавлено
    public static string TitleCourse(string id)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        string result = "";

        string sql =
            "SELECT \"title\" " +
            $"FROM Сources WHERE \"id\" = \'{id}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        if (_reader.Read())
        {
            result = _reader[0].ToString();
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    // добавлено
    public static List<QuestionModel> AllCoursesQuestionsForRepetition()
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<QuestionModel> result = new();

        string sql =
            "SELECT id, \"question\", \"trueanswer\", \"oneFalseAnswer\", \"twofalseanswer\", \"explanation\", time, startpoint " +
            $"FROM questions WHERE \"idcources\" = \'{IdCources}\' AND startpoint <= {ProgressPoint}";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i += 8)
            {
                result.Add(new QuestionModel(_reader[i].ToString(), IdCources, _reader[i + 1].ToString(),
                    _reader[i + 2].ToString(), _reader[i + 3].ToString(), _reader[i + 4].ToString(),
                    _reader[i + 5].ToString(), Int32.Parse(_reader[i + 6].ToString()), Int32.Parse(_reader[i + 7].ToString())));
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    public static List<QuestionModel> AllCoursesQuestionsForRepetition(string id)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<QuestionModel> result = new();

        string sql =
            "SELECT id, \"question\", \"trueanswer\", \"oneFalseAnswer\", \"twofalseanswer\", \"explanation\", time, startpoint " +
            $"FROM questions WHERE \"idcources\" = \'{id}\' AND startpoint <= {ProgressPoint}";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();
        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i += 8)
            {
                result.Add(new QuestionModel(_reader[i].ToString(), IdCources, _reader[i + 1].ToString(),
                    _reader[i + 2].ToString(), _reader[i + 3].ToString(), _reader[i + 4].ToString(),
                    _reader[i + 5].ToString(), Int32.Parse(_reader[i + 6].ToString()), Int32.Parse(_reader[i + 7].ToString())));
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    // добавлено
    public static List<QuestionModel> AllCoursQuestions(string id)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<QuestionModel> result = new();

        string sql =
            "SELECT id, \"question\", \"trueanswer\", \"oneFalseAnswer\", \"twofalseanswer\", \"explanation\", time, startpoint " +
            $"FROM questions WHERE \"idcources\" = \'{id}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i += 8)
            {
                result.Add(new QuestionModel(_reader[i].ToString(), id, _reader[i + 1].ToString(),
                    _reader[i + 2].ToString(), _reader[i + 3].ToString(), _reader[i + 4].ToString(),
                    _reader[i + 5].ToString(), Int32.Parse(_reader[i + 6].ToString()), Int32.Parse(_reader[i + 7].ToString())));
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    // добавлено
    public static List<TermModel> AllCoursTerms(string id)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<TermModel> result = new();

        string sql =
            "SELECT id, \"terminology\", \"definition\", time, startpoint " +
            $"FROM definitions WHERE \"idcources\" = \'{id}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i += 5)
            {
                result.Add(new TermModel(_reader[i].ToString(), id, _reader[i + 1].ToString(),
                    _reader[i + 2].ToString(), Int32.Parse(_reader[i + 3].ToString()), Int32.Parse(_reader[i + 4].ToString())));
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    // добавлено
    public static List<TermModel> AllCoursTermsForRepetition()
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<TermModel> result = new();

        string sql =
            "SELECT id, \"terminology\", \"definition\", time, startpoint " +
            $"FROM definitions WHERE \"idcources\" = \'{IdCources}\' AND startpoint <= {ProgressPoint}";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i += 5)
            {
                result.Add(new TermModel(_reader[i].ToString(), IdCources, _reader[i + 1].ToString(),
                    _reader[i + 2].ToString(), Int32.Parse(_reader[i + 3].ToString()), Int32.Parse(_reader[i + 4].ToString())));
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    // добавлено
    public static List<RepetitionModel> AllCoursesRepetition()
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<RepetitionModel> result = new();

        string sql =
            "SELECT id, idcources, iduser, passdate, passprogresspoint, percentagecorrectanswers " +
            $"FROM repetitioncourses WHERE \"idcources\" = \'{IdCources}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i += 6)
            {
                var temp = new RepetitionModel();
                temp.Id = _reader[i].ToString();
                temp.IdCources = _reader[i + 1].ToString();
                temp.IdUser = _reader[i + 2].ToString();
                temp.Date = _reader[i + 3].ToString(); // .Substring(0, 10)
                temp.PassProgressPoint = Int32.Parse(_reader[i + 4].ToString());
                temp.PercentageCorrectAnswers = Int32.Parse(_reader[i + 5].ToString());
                result.Add(temp);
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    // добавлено
    public static List<RepetitionModel> AllCourseRepetition(string idCource)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<RepetitionModel> result = new();

        string sql =
            "SELECT id, idcources, iduser, passdate, passprogresspoint, percentagecorrectanswers " +
            $"FROM repetitioncourses WHERE \"idcources\" = \'{idCource}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i += 6)
            {
                var temp = new RepetitionModel();
                temp.Id = _reader[i].ToString();
                temp.IdCources = _reader[i + 1].ToString();
                temp.IdUser = _reader[i + 2].ToString();
                temp.Date = _reader[i + 3].ToString(); // .Substring(0, 10)
                temp.PassProgressPoint = Int32.Parse(_reader[i + 4].ToString());
                temp.PercentageCorrectAnswers = Int32.Parse(_reader[i + 5].ToString());
                result.Add(temp);
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    //не добавлено
    public static bool WasCourseRepetitionToday(string idCources)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<string> courcesRepetiotion = new();
        bool result = false;

        string sql =
            "SELECT idcources " +
            $"FROM repetitioncourses WHERE date_trunc('day', passdate) = current_date AND \"idcources\" = \'{idCources}\';";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        result = _reader.Read();

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }


    // добавлено
    public static QuestionModel GetQuestion(string id)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        QuestionModel result = new();

        string sql =
            "SELECT \"question\", \"trueanswer\", \"oneFalseAnswer\", \"twofalseanswer\", \"explanation\" " +
            $"FROM questions WHERE \"id\" = \'{id}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i += 5)
            {
                result.QuestionText = _reader[i].ToString();
                result.TrueAnswer = _reader[i + 1].ToString();
                result.OneFalseAnswer = _reader[i + 2].ToString();
                result.TwoFalseAnswer = _reader[i + 3].ToString();
                result.Explanation = _reader[i + 4].ToString();
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    public static TermModel GetTerm(string id)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        TermModel result = new();

        string sql =
            "SELECT \"idcources\", \"terminology\", \"definition\", time, startpoint " +
            $"FROM definitions WHERE \"id\" = \'{id}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i += 5)
            {
                result.IdCources = _reader[i].ToString();
                result.Terminology = _reader[i + 1].ToString();
                result.Description = _reader[i + 2].ToString();
                result.Time = Int32.Parse(_reader[i + 3].ToString());
                result.StartPoint = Int32.Parse(_reader[i + 4].ToString());
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    // добавлено
    public static RepetitionModel GetLastRetition()
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        RepetitionModel result = new();

        string sql =
            "SELECT id, idcources, passdate, passprogresspoint, percentagecorrectanswers " +
            $"FROM repetitioncourses WHERE \"iduser\" = \'{IdNowUser}\' ORDER BY passdate DESC LIMIT 1;";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i += 5)
            {
                result.Id = _reader[i].ToString();
                result.IdCources = _reader[i + 1].ToString();
                result.Date = _reader[i + 2].ToString(); //.Substring(0, 10)
                result.PassProgressPoint = Int32.Parse(_reader[i + 3].ToString());
                result.PercentageCorrectAnswers = Int32.Parse(_reader[i + 4].ToString());
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    //TODO: переписать
    //List<string> AllAnswersQuestion(string idrepetitioncourse)
    public static List<AnswerModel> AllAnswersQuestion(string idrepetition)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<AnswerModel> result = new();

        string sql =
            "SELECT id, \"idquestion\", \"idrepetitioncourse\", rightanswer " +
            $"FROM answersonquestions WHERE \"idrepetitioncourse\" = \'{idrepetition}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i += 4)
            {
                result.Add(new AnswerModel(_reader[i].ToString(), _reader[i + 1].ToString(),
                    _reader[i + 2].ToString(), Boolean.Parse(_reader[i + 3].ToString()), AnswersType.Question));
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }


    //List<string> AllAnswersTerm (string idrepetitioncourse) 

    public static List<AnswerModel> AllAnswersTerm(string idrepetition)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<AnswerModel> result = new();

        string sql =
            "SELECT id, \"iddefinition\", \"idrepetitioncourse\", rightanswer " +
            $"FROM answersondefinitions WHERE \"idrepetitioncourse\" = \'{idrepetition}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i += 4)
            {
                result.Add(new AnswerModel(_reader[i].ToString(), _reader[i + 1].ToString(),
                    _reader[i + 2].ToString(), Boolean.Parse(_reader[i + 3].ToString()), AnswersType.Term));
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    public static List<string> AllRepetitionMistakes(string idrepetitioncourse)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<string> result = new();

        string sql =
            "SELECT idquestion " +
            $"FROM falseanswers WHERE \"idrepetitioncourse\" = \'{idrepetitioncourse}\' AND \"idcource\" = \'{IdCources}\' AND \"iduser\" = \'{IdNowUser}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i++)
            {
                result.Add(_reader[i].ToString());
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    // добавлено
    public static string AddRepetition(int percentageResult)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        string result = "";

        string sql =
            "INSERT INTO RepetitionCourses " +
            "(idcources, iduser, passProgressPoint, PercentageCorrectAnswers)" +
            $" VALUES(\'{IdCources}\', \'{IdNowUser}\', {ProgressPoint}, {percentageResult})" +
            " RETURNING id; ";

        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        if (_reader.Read())
        {
            result = _reader[0].ToString();
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    // void AddQuestionAnswer(QuestionAnswer answer);
    public static void AddQuestionAnswer(AnswerModel answer)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();

        string sql =
            "INSERT INTO answersonquestions " +
            "(idQuestion, idRepetitionCourse, rightanswer) " +
            $"VALUES(\'{answer.IdQuestion}\', \'{answer.IdRepetitionCource}\', {(answer.Answer ? "True" : "False")});";
        _dbcmd.CommandText = sql;
        _dbcmd.ExecuteReader();

        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;
    }

    // void AddTermAnswer(TrmAnswer answer);
    public static void AddTermAnswer(AnswerModel answer)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();

        string sql =
            "INSERT INTO answersondefinitions " +
            "(iddefinition, idRepetitionCourse, rightanswer) " +
            $"VALUES(\'{answer.IdQuestion}\', \'{answer.IdRepetitionCource}\', {(answer.Answer ? "True" : "False")});";
        _dbcmd.CommandText = sql;
        _dbcmd.ExecuteReader();

        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;
    }

    // добавлено
    public static bool AddQuestion(string idcource, string startPoint, string question, string trueAnswer, string oneFalseAnswer, string twoFalseAnswer, string explanation, string time)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        bool result = true;

        try
        {
            string sql =
            "INSERT INTO \"questions\" " +
            "(idcources, startpoint, \"question\", \"trueanswer\", \"oneFalseAnswer\", \"twofalseanswer\", \"explanation\", time) " +
            $"VALUES(\'{idcource}\', {startPoint},\'{question}\', \'{trueAnswer}\', \'{oneFalseAnswer}\', \'{twoFalseAnswer}\',\'{explanation}\', {time});";
            _dbcmd.CommandText = sql;
            _dbcmd.ExecuteReader();
        }
        catch
        {
            result = false;
        }

        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;
        return result;
    }

    //добавлено
    public static void DeleteQuestion(string id)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();

        _dbcmd.CommandText = "DELETE FROM \"questions\" " +
        $"WHERE id = \'{id}\';";
        _dbcmd.ExecuteReader();

        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;
    }

    // добавление
    public static bool ChangeQuestion(string id, string idcource, string startPoint, string question, string trueAnswer, string oneFalseAnswer, string twoFalseAnswer, string explanation, string time)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        bool result = true;

        //try
        //{
        string sql =
        "UPDATE \"questions\" " +
        $"SET \"idcources\" = \'{idcource}\', startpoint = {startPoint}, \"question\" = \'{question}\', \"trueanswer\" = \'{trueAnswer}\', \"oneFalseAnswer\" = \'{oneFalseAnswer}\', \"twofalseanswer\" = \'{twoFalseAnswer}\', \"explanation\" = \'{explanation}\', time = {time} " +
        $"WHERE \"id\" = \'{id}\';";
        _dbcmd.CommandText = sql;
        _dbcmd.ExecuteReader();
        //}
        //catch
        //{
        //    result = false;
        //}

        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;
        return result;
    }

    // добавлено
    public static bool AddTerm(string idcource, string startPoint, string term, string description, string time)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        bool result = true;

        try
        {
            string sql =
            "INSERT INTO \"definitions\" " +
            "(idcources, startpoint, \"terminology\", \"definition\", time) " +
            $"VALUES(\'{idcource}\', {startPoint},\'{term}\', \'{description}\', {time});";
            _dbcmd.CommandText = sql;
            _dbcmd.ExecuteReader();
        }
        catch
        {
            result = false;
        }

        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;
        return result;
    }

    // добавлено
    public static void DeleteTerm(string id)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();

        _dbcmd.CommandText = "DELETE FROM \"definitions\" " +
        $"WHERE id = \'{id}\';";
        _dbcmd.ExecuteReader();

        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;
    }

    //добавлено
    public static bool ChangeTerm(string id, string idcource, string startPoint, string term, string description, string time)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        bool result = true;

        //try
        //{
        string sql =
        "UPDATE \"definitions\" " +
        $"SET \"idcources\" = \'{idcource}\', startpoint = {startPoint}, \"terminology\" = \'{term}\', \"definition\" = \'{description}\', time = {time} " +
        $"WHERE \"id\" = \'{id}\';";
        _dbcmd.CommandText = sql;
        _dbcmd.ExecuteReader();
        //}
        //catch
        //{
        //    result = false;
        //}

        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;
        return result;
    }

    // добавлено
    public static bool CheckAuthorCources()
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        bool result = true;

        string sql =
            "SELECT id " +
            $"FROM \"Сources\" WHERE \"author_id\" = \'{IdNowUser}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        result = _reader.Read();

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }


    public static int CountAllAnswerQuestion(string idQuestion)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        int result = 0;

        string sql =
            "SELECT id " +
            $"FROM answersonquestions WHERE \"idquestion\" = \'{idQuestion}\';";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i++)
            {
                result++;
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    public static int CountAllAnswerTerm(string idTerm)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        int result = 0;

        string sql =
            "SELECT id " +
            $"FROM answersondefinitions WHERE \"iddefinition\" = \'{idTerm}\';";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i++)
            {
                result++;
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    public static int CountAllRightAnswerTerm(string idTerm)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        int result = 0;

        string sql =
            "SELECT id " +
            $"FROM answersondefinitions WHERE \"iddefinition\" = \'{idTerm}\' AND rightanswer = True;";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i++)
            {
                result++;
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    public static int CountAllRightAnswerQuestion(string idQuestion)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        int result = 0;

        string sql =
            "SELECT id " +
            $"FROM answersonquestions WHERE \"idquestion\" = \'{idQuestion}\' AND rightanswer = True;";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i++)
            {
                result++;
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    public static int CountRepetiotion(string idCources)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        int result = 0;

        string sql =
            "SELECT id " +
            $"FROM repetitioncourses WHERE \"idcources\" = \'{idCources}\';";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i++)
            {
                result++;
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }

    public static int AveragePassingValue(string idCources)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        int result = 0;

        string sql =
            "SELECT percentagecorrectanswers " +
            $"FROM repetitioncourses WHERE \"idcources\" = \'{idCources}\';";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            int i = 0;
            for (; i < _reader.FieldCount; i++)
            {
                result += Int32.Parse(_reader[i].ToString());
            }
            result /= i;
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return result;
    }
}
