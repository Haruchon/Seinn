using System;
using UnityEngine;
using GameAuthoringAPI;
using System.Collections;
using System.Collections.Generic;

public class SeinnGameStudent : GameStudent
{
    public List<SeinnStudentGameSessionConfig> gameSessionConfigs = new List<SeinnStudentGameSessionConfig>();
}

public class SeinnStudentGameSessionConfig : StudentGameSessionConfig
{
    public List<Competence> competences = new List<Competence>();

    public class Competence
    {
        private string name;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        private bool isActive;
        public bool IsActive
        {
            get { return this.isActive; }
            set { this.isActive = value; }
        }
        public List<Parameter> parameters = new List<Parameter>();
        public List<Competence> competences = new List<Competence>();
    }

    public class Parameter
    {
        private string name;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        private string type;
        public string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
        private object _value;
        public object Value
        {
            get { return this._value; }
            set { this._value = value; }
        }
    }
}

//#pragma warning disable CS0436 // Type conflicts with imported type
public class SeinnAuthoringAPIAdapter : GameAuthoringAPIAdapter
//#pragma warning restore CS0436 // Type conflicts with imported type
{
    public const string GAME_ID = "7";
    public static SeinnGameStudent student;

    private static SeinnAuthoringAPIAdapter instance;
    private SeinnAuthoringAPIAdapter() { }
    public static SeinnAuthoringAPIAdapter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SeinnAuthoringAPIAdapter();
            }
            return instance;
        }
    }

    public override void ProcessGameStudentData(string token, Student student, Action<GameStudent> action)
    {
        SeinnGameStudent gameStudent = null;

        if (token != null && student != null)
        {
            gameStudent = new SeinnGameStudent();
            gameStudent.Username = student.User.Username;
            gameStudent.Email = student.User.Email;
            gameStudent.FirstName = student.User.FirstName;
            gameStudent.LastName = student.User.LastName;
            gameStudent.Institution = student.Institution;
            gameStudent.Token = token;
        }

        action(gameStudent);
    }

    public override void ProcessStudentGameConfigData(StudentGameConfig studentGameConfig, Action<StudentGameSessionConfig> action)
    {
        throw new NotImplementedException();
    }

    public override void ProcessStudentGameConfigsData(List<StudentGameConfig> studentGameConfigs, Action<List<StudentGameSessionConfig>> action)
    {
        //Debug.Log("JBM: Executing Adapter.ProcessStudentGameConfigsData");
        List<StudentGameSessionConfig> studentGameSessionConfigs = new List<StudentGameSessionConfig>();

        for (int i = 0; i < studentGameConfigs.Count; i++)
        {
            StudentGameConfig studentGameConfigObj = studentGameConfigs[i];
            SeinnStudentGameSessionConfig seinnStudentGameSessionConfig = null;

            if (studentGameConfigObj != null && studentGameConfigObj.DgblConfig != null &&
                studentGameConfigObj.DgblConfig.IlosConfig != null &&
                studentGameConfigObj.DgblConfig.IlosConfig.Count > 0)
            {
                seinnStudentGameSessionConfig = new SeinnStudentGameSessionConfig();
                seinnStudentGameSessionConfig.Id = studentGameConfigObj.Id;
                seinnStudentGameSessionConfig.Label = studentGameConfigObj.Label;

                int competences_number = studentGameConfigObj.DgblConfig.IlosConfig.Count;
                for (int j = 0; j < competences_number; j++)
                {
                    SeinnStudentGameSessionConfig.Competence competence = new SeinnStudentGameSessionConfig.Competence();
                    competence.Name = studentGameConfigObj.DgblConfig.IlosConfig[j].Label;
                    competence.IsActive = studentGameConfigObj.DgblConfig.IlosConfig[j].IsActive;

                    if (studentGameConfigObj.DgblConfig.IlosConfig[j].IloParameters != null &&
                        studentGameConfigObj.DgblConfig.IlosConfig[j].IloParameters.Count > 0)
                    {
                        int parameters_number = studentGameConfigObj.DgblConfig.IlosConfig[j].IloParameters.Count;
                        for (int k = 0; k < parameters_number; k++)
                        {
                            SeinnStudentGameSessionConfig.Parameter parameter = new SeinnStudentGameSessionConfig.Parameter();
                            parameter.Name = studentGameConfigObj.DgblConfig.IlosConfig[j].IloParameters[k].Label;
                            if (studentGameConfigObj.DgblConfig.IlosConfig[j].IloParameters[k].Type == TypeEnum.Integer)
                            {
                                parameter.Type = "Int";
                                parameter.Value = Int32.Parse(studentGameConfigObj.DgblConfig.IlosConfig[j].IloParameters[k].Value);
                            }
                            if (studentGameConfigObj.DgblConfig.IlosConfig[j].IloParameters[k].Type == TypeEnum.Text)
                            {
                                parameter.Type = "String";
                                parameter.Value = studentGameConfigObj.DgblConfig.IlosConfig[j].IloParameters[k].Value.ToString();
                            }
                            competence.parameters.Add(parameter);
                        }
                    }

                    seinnStudentGameSessionConfig.competences.Add(competence);
                }
            }
            studentGameSessionConfigs.Add(seinnStudentGameSessionConfig);
            student.gameSessionConfigs.Add(seinnStudentGameSessionConfig);
        }
        action(studentGameSessionConfigs);
    }
}
