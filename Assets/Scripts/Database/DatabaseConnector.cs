using System;
using System.Collections.Generic;
using UnityEngine;
using Npgsql;
using System.Linq;

public static class DatabaseConnector
{
    public static string IdNowUser;
    public static string UsernName;
    public static string Password;
    public static string IdCources;
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

    public static bool Verification(string userName, string password)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();

        string sql =
            "SELECT * " +
            $"FROM users WHERE username = \'{userName}\' AND password = \'{password}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        if (!_reader.Read()) return false;

        IdNowUser = _reader[0].ToString();

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        return true;
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

    public static List<string> AllCoursesUserAuthor()
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<string> result = new();

        string sql =
            "SELECT id " +
            $"FROM \"Ñources\" WHERE \"author_id\" = \'{IdNowUser}\'";
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

    public static string TitleCourse(string id)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        string result = "";

        string sql =
            "SELECT \"title\" " +
            $"FROM Ñources WHERE \"id\" = \'{id}\'";
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

    public static List<QuestionModel> AllCoursesQuestionsForRepetition()
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<QuestionModel> result = new();

        string sql =
            "SELECT id, \"question\", \"trueanswer\", \"oneFalseAnswer\", \"twofalseanswer\", \"explanation\", time " +
            $"FROM questions WHERE \"idcources\" = \'{IdCources}\' AND startpoint <= {ProgressPoint}";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i += 7)
            {
                result.Add(new QuestionModel(_reader[i].ToString(), _reader[i + 1].ToString(),
                    _reader[i + 2].ToString(), _reader[i + 3].ToString(), _reader[i + 4].ToString(),
                    _reader[i + 5].ToString(), Int32.Parse(_reader[i + 6].ToString())));
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

    public static List<QuestionModel> AllCoursQuestions(string id)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<QuestionModel> result = new();

        string sql =
            "SELECT id, \"question\", \"trueanswer\", \"oneFalseAnswer\", \"twofalseanswer\", \"explanation\", time " +
            $"FROM questions WHERE \"idcources\" = \'{id}\'";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i += 7)
            {
                result.Add(new QuestionModel(_reader[i].ToString(), _reader[i + 1].ToString(),
                    _reader[i + 2].ToString(), _reader[i + 3].ToString(), _reader[i + 4].ToString(),
                    _reader[i + 5].ToString(), Int32.Parse(_reader[i + 6].ToString())));
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

    public static List<string> AllNeedCoursesRepetition()
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        List<string> courcesRepetiotion = new();

        string sql =
            "SELECT idcources " +
            $"FROM repetitioncourses WHERE date_trunc('day', passdate) = current_date;";
        _dbcmd.CommandText = sql;

        _reader = _dbcmd.ExecuteReader();

        while (_reader.Read())
        {
            for (int i = 0; i < _reader.FieldCount; i += 6)
            {
                courcesRepetiotion.Add(_reader[i].ToString());
            }
        }

        _reader.Close();
        _reader = null;
        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;

        List<string> allcources = AllCoursesUserTakes();
        List<string> result = new();

        foreach (var id in allcources)
        {
            if (!HaveRepetition(id) && !courcesRepetiotion.Contains(id))
            {
                result.Add(id);
            }
        }

        return result.Distinct().ToList();
    }

    private static bool HaveRepetition(string id)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        bool result = true;

        string sql =
            "SELECT * " +
            $"FROM repetitioncourses WHERE \"idcources\" = \'{id}\';";
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


    public static QuestionModel GetQuestion(string id)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        QuestionModel result = new();

        string sql =
            "SELECT question, trueanswer, onefalseanswer, twofalseanswer, explanation " +
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

    public static void AddMistake(MistakeModel mistake)
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();

        string sql =
            "INSERT INTO FalseAnswers " +
            "(idQuestion, idUser, idRepetitionCourse, idCource) " +
            $"VALUES(\'{mistake.IdQuestion}\', \'{mistake.IdUser}\', \'{mistake.IdRepetitionCource}\', \'{mistake.IdCource}\');";
        _dbcmd.CommandText = sql;
        _dbcmd.ExecuteReader();

        _dbcmd.Dispose();
        _dbcmd = null;
        _dbcon.Close();
        _dbcon = null;
    }

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

    public static bool CheckAuthorCources()
    {
        _dbcon = new NpgsqlConnection(_connectionString);
        _dbcon.Open();
        _dbcmd = _dbcon.CreateCommand();
        bool result = true;

        string sql =
            "SELECT id " +
            $"FROM \"Ñources\" WHERE \"author_id\" = \'{IdNowUser}\'";
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
}
