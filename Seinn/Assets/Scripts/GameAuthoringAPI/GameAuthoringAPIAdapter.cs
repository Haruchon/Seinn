using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace GameAuthoringAPI
{
    public abstract class GameStudent
    {
        private string userName;
        public string Username { get; set; }

        private string firstName;
        public string FirstName { get; set; }

        private string lastName;
        public string LastName { get; set; }

        private string email;
        public string Email { get; set; }

        private string institution;
        public string Institution { get; set; }

        private string token;
        public string Token { get; set; }
    }

    public abstract class StudentGameSessionConfig {
        private long id;
        public long Id { get; set; }

        private string label;
        public string Label { get; set; }
    }

    public abstract class GameAuthoringAPIAdapter
    {
        protected const string GAME_AUTHORING_SERVER = "79.137.77.50";
        protected const string GAME_AUTHORING_SERVER_PORT = "8000";
        protected const string GAME_AUTHORING_URL_API_LOGIN = "api/api-token-auth";
        protected const string GAME_AUTHORING_URL_API_GAMES = "api/games";
        protected const string GAME_AUTHORING_URL_API_STUDENTS = "api/students";
        protected const string GAME_AUTHORING_URL_API_INVENTORY = "api/inventory";
        protected const string GAME_AUTHORING_URL_API_ACTIVE_GAMES = "api/active_games";
        protected const string GAME_AUTHORING_URL_API_GAME_CONFIG = "api/game_configs";
        protected const string GAME_AUTHORING_URL_API_STUDENT_GAME_CONFIG = "api/student_game_config";

        public IEnumerator Login(string username, string password, Action<GameStudent> processGameStudent)
        { 
            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("password", password);
            UnityWebRequest www = UnityWebRequest.Post(String.Format("http://{0}:{1}/{2}/",
                GAME_AUTHORING_SERVER, GAME_AUTHORING_SERVER_PORT, GAME_AUTHORING_URL_API_LOGIN), form);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                ProcessGameStudentData(null, null, processGameStudent);
            }
            else
            {
                //Debug.Log("JBM: GameAuthoringAPIAdapter.Login: POST successful!");
                //Debug.Log("JBM www.downloadHandler.text:" + www.downloadHandler.text);
                Login loginData = GameAuthoringAPI.Login.FromJson(www.downloadHandler.text);
                //Debug.Log("JBM: GameAuthoringAPIAdapter.Login: Token: " + loginData.Token);

                using (UnityWebRequest req = UnityWebRequest.Get(String.Format("http://{0}:{1}/{2}/{3}/",
                    GAME_AUTHORING_SERVER, GAME_AUTHORING_SERVER_PORT, GAME_AUTHORING_URL_API_STUDENTS,
                    username)))
                {
                    req.SetRequestHeader("Authorization", "Token " + loginData.Token);

                    yield return req.SendWebRequest();
                    while (!req.isDone)
                        yield return null;
                    byte[] result = req.downloadHandler.data;
                    string studentJSON = System.Text.Encoding.Default.GetString(result);
                    //Debug.Log("JBM studentJSON:" + studentJSON);
                    Student student = Student.FromJson(studentJSON);
                    ProcessGameStudentData(loginData.Token, student, processGameStudent);
                }
            }
        }

        public IEnumerator GetStudentGameConfigs(string token, string username, string game_id, 
            Action<List<StudentGameSessionConfig>> processStudentGameConfigs)
        {
            //Debug.Log("JBM Executing GetStudentGameConfigs.");
            string url = String.Format("http://{0}:{1}/{2}/{3}/{4}", GAME_AUTHORING_SERVER, GAME_AUTHORING_SERVER_PORT, GAME_AUTHORING_URL_API_GAME_CONFIG, username, game_id);
            using (UnityWebRequest req = UnityWebRequest.Get(url))
            {
                req.SetRequestHeader("Authorization", "Token " + token);

                yield return req.SendWebRequest();
                while (!req.isDone)
                    yield return null;
                byte[] result = req.downloadHandler.data;
                string gameSessionsJSON = System.Text.Encoding.Default.GetString(result);
                Debug.Log("JBM gameSessionsJSON:" + gameSessionsJSON);
                List<StudentGameConfig> studentGameConfigs = StudentGameConfigsByUsernameGame.FromJson(gameSessionsJSON);                
                ProcessStudentGameConfigsData(studentGameConfigs, processStudentGameConfigs);
            }
        }

        public IEnumerator GetStudentGameConfig(string token, string student_game_config_id, Action<StudentGameSessionConfig> processStudentGameConfig)
        {
            using (UnityWebRequest req = UnityWebRequest.Get(String.Format("http://{0}:{1}/{2}/{3}/", 
                GAME_AUTHORING_SERVER, GAME_AUTHORING_SERVER_PORT, GAME_AUTHORING_URL_API_STUDENT_GAME_CONFIG, 
                student_game_config_id)))
            {
                req.SetRequestHeader("Authorization", "Token " + token);

                yield return req.SendWebRequest();
                while (!req.isDone)
                    yield return null;
                byte[] result = req.downloadHandler.data;
                string studentGameConfigJSON = System.Text.Encoding.Default.GetString(result);
                Debug.Log("JBM GameAuthoringAPIAdapter.GetStudentGameConfig: \n"+ studentGameConfigJSON);
                StudentGameConfig studentGameConfig = StudentGameConfig.FromJson(studentGameConfigJSON);
                ProcessStudentGameConfigData(studentGameConfig, processStudentGameConfig);
            }            
        }

        //#pragma warning disable CS0436 // Type conflicts with imported type
        public abstract void ProcessStudentGameConfigsData(List<StudentGameConfig> studentGameConfigs, Action<List<StudentGameSessionConfig>> action);
        public abstract void ProcessStudentGameConfigData(StudentGameConfig studentGameConfig, Action<StudentGameSessionConfig> action);
        public abstract void ProcessGameStudentData(string token, Student student, Action<GameStudent> action);
        //#pragma warning restore CS0436 // Type conflicts with imported type

    }
}
