using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using GameAuthoringAPI;

public class MainMenu : MonoBehaviour
{
    private TMP_InputField username;
    private TMP_InputField password;
    private Button incorrectDataButton;
    private Button loginButton;
    private GameObject incorrectData;
    private bool logged;

    private GameObject login;
    private GameObject mainMenu;
    private GameObject playMenu;
    private GameObject scoreMenu;
    private GameObject achievementMenu;
    private GameObject optionMenu;
    private GameObject loadingBar;

    [SerializeField]
    private Slider slider;

    private MisionData config;

    private void showLogin() { LeanTween.moveLocalY(login, -30f, 1f); }
    private void hideLogin() { LeanTween.moveLocalY(login, 550f, 1f); }

    private void showMain() { LeanTween.moveLocalY(mainMenu, -50f, 1f); }
    private void hideMain() { LeanTween.moveLocalY(mainMenu, -550f, 1f); }

    private void showIncorrectData() { LeanTween.moveLocalX(incorrectData, 0f, 1f); }
    private void hideIncorrectData() { LeanTween.moveLocalX(incorrectData, 700f, 1f); }

    private void showplay() { LeanTween.moveLocalX(playMenu, 0f, 1f); }
    private void hidePlay() { LeanTween.moveLocalX(playMenu, -600f, 1f); }

    //private void showScore() { LeanTween.moveLocalY(scoreMenu, -50f, 1f); }
    //private void hideScore() { LeanTween.moveLocalY(scoreMenu, -600f, 1f); }

    //private void showAchiev() { LeanTween.moveLocalY(achievementMenu, -50f, 1f); }
    //private void hideAchiev() { LeanTween.moveLocalY(achievementMenu, -600f, 1f); }

    private void showOption() { LeanTween.moveLocalX(optionMenu, 0f, 1f); }
    private void hideOption() { LeanTween.moveLocalX(optionMenu, 650f, 1f); }

    private void showLoading() { LeanTween.moveLocalY(loadingBar, -50f, 1f); }

    public void showMainAfterLog() { StartCoroutine(showMainAfterLogRoutine()); }
    public IEnumerator showMainAfterLogRoutine()
    {
        hideLogin();
        yield return null; // new WaitForSeconds(1f);
        showMain();
    }

    public void showLogAfterLogout() { StartCoroutine(showLogAfterLogoutRoutine()); }
    public IEnumerator showLogAfterLogoutRoutine()
    {
        showLogin();
        yield return null; // new WaitForSeconds(1f);
        hideMain();
    }

    public void showPlayGame() { StartCoroutine(showPlayGameRoutine()); }
    public IEnumerator showPlayGameRoutine()
    {
        hideMain();
        yield return null; // new WaitForSeconds(1f);
        showplay();
    }

    public void hidePlayGame() { StartCoroutine(hidePlayGameRoutine()); }
    public IEnumerator hidePlayGameRoutine()
    {
        hidePlay();
        yield return null; // new WaitForSeconds(1f);
        showMain();
    }

    public void showOptions() { StartCoroutine(showOptionsRoutine()); }
    public IEnumerator showOptionsRoutine()
    {
        hideMain();
        yield return null; // new WaitForSeconds(1f);
        showOption();
    }

    public void hideOptions() { StartCoroutine(hideOptionsRoutine()); }
    public IEnumerator hideOptionsRoutine()
    {
        hideOption();
        yield return null; // new WaitForSeconds(1f);
        showMain();
    }

    private void Awake()
    {
        login = transform.GetChild(1).gameObject;
        mainMenu = transform.GetChild(2).gameObject;
        playMenu = transform.GetChild(3).gameObject;
        scoreMenu = transform.GetChild(4).gameObject;
        achievementMenu = transform.GetChild(5).gameObject;
        optionMenu = transform.GetChild(6).gameObject;
        loadingBar = transform.GetChild(7).gameObject;

        incorrectData = login.transform.GetChild(6).gameObject;

        incorrectDataButton = incorrectData.transform.GetChild(1).GetComponent<Button>();
        loginButton = login.transform.GetChild(1).GetComponent<Button>();
        username = login.transform.GetChild(3).GetComponent<TMP_InputField>();
        password = login.transform.GetChild(5).GetComponent<TMP_InputField>();

        logged = false;
    }

    private void Start()
    {
        showLogin();
        AudioManager.instance.StopSound("BackgroundMusic");
        AudioManager.instance.PlaySound("MenuMusic");
    }

    public void PlayGame(){ StartCoroutine(PlayGameRoutine()); }
    private IEnumerator PlayGameRoutine()
    {
        hidePlay();
        yield return null;
        showLoading();
        yield return new WaitForSeconds(2f);
        LoadLevel("Seinn");
    }

    public Login.LoginData _user;

    public void CheckLogin()
    {
        _user = new Login.LoginData(username.text, password.text);
        //StartCoroutine(SeinnAuthoringAPIAdapter.Instance.Login(username.text, password.text,
        //                ProcessGameStudentAfterAuthentication));

        if (Login.TestLogin(_user))
        {
            //login succesfull -show main menu
            showMainAfterLog();
        }
        else
        {
            //login unsuccessfull -show incorrect
            loginButton.enabled = false;
            showIncorrectData();
        }

    }

    public void ProcessGameStudentAfterAuthentication(GameStudent gameStudent)
    {
        SeinnAuthoringAPIAdapter.student = (SeinnGameStudent)gameStudent;
        if (gameStudent != null)
        {
            StartCoroutine(SeinnAuthoringAPIAdapter.Instance.GetStudentGameConfigs(
                gameStudent.Token, gameStudent.Username, SeinnAuthoringAPIAdapter.GAME_ID,
                ProcessStudentGameConfigs));
            logged = true;
        }
        else
        {
            //Debug.Log("Usuario y password incorrectos");
            logged = false;
            loginButton.enabled = false;
            showIncorrectData();
        }
    }

    public void ProcessStudentGameConfigs(List<StudentGameSessionConfig> studentGameSessionConfigs)
    {
        config = new MisionData();
        config.numConfig = new List<Utils.NumberType>();
        config.maxDigits = new List<int>();
        config.numDivMult = new List<int>();
        config.isEquation = new List<bool>();
        config.numConfig.Add(Utils.NumberType.Natural);
        config.numConfig.Add(Utils.NumberType.Natural);
        foreach (SeinnStudentGameSessionConfig studentGameSessionConfig in studentGameSessionConfigs)
        {
            //Debug.Log("JBM Configuración:" + studentGameSessionConfig.Label);

            for (int i = 0; i < studentGameSessionConfig.competences.Count; i++)
            {
                //Debug.Log("JBM Configuración: Competence - " + studentGameSessionConfig.competences[i].Name);
                if (studentGameSessionConfig.competences[i].IsActive)
                {
                    //Debug.Log("JBM Configuración: Competence - " + studentGameSessionConfig.competences[i].Name + " is active. ");
                    for (int j = 0; j < studentGameSessionConfig.competences[i].parameters.Count; j++)
                    {
                        if (i == 0)
                        {
                            if(j == 0)
                            {
                                for (int k = 0; k < Utils.numMissions; k++)
                                    config.maxDigits.Add(Int32.Parse(studentGameSessionConfig.competences[i].parameters[j].Value.ToString()));
                            }
                            else if(j==1)
                            {
                                InsertNumTypeInConfig(studentGameSessionConfig, i, j, 2);
                            }
                        }
                        else if (i == 1)
                        {
                            if (j == 0)
                            {
                                for (int k = 0; k < Utils.numMissions; k++)
                                    config.numDivMult.Add(Int32.Parse(studentGameSessionConfig.competences[i].parameters[j].Value.ToString()));
                            }
                            else if (j == 1)
                            {
                                InsertNumTypeInConfig(studentGameSessionConfig, i, j, 4);
                            }
                        }
                        else if (i == 2)
                        {
                            if (j == 0)
                            {
                                for (int k = 0; k < Utils.numMissions; k++)
                                    config.isEquation.Add(studentGameSessionConfig.competences[i].parameters[j].Value.ToString()=="Ecuaciones"?true:false);
                            }
                            else if (j == 1)
                            {
                                InsertNumTypeInConfig(studentGameSessionConfig, i, j, 2);
                            }
                        }
                    }
                }
            }

        }

        if (logged)
        {
            //login succesfull - show main menu
            showMainAfterLog();
            Debug.Log(config.ToString());
        }
    }

    private void InsertNumTypeInConfig(SeinnStudentGameSessionConfig studentGameSessionConfig, int i, int j, int numDatos)
    {
        Utils.NumberType nType0 = Utils.NumberType.Natural;
        if (studentGameSessionConfig.competences[i].parameters[j].Value.ToString() == "Números naturales")
            nType0 = Utils.NumberType.Natural;
        else if (studentGameSessionConfig.competences[i].parameters[j].Value.ToString() == "Fracciones")
            nType0 = Utils.NumberType.Fraccion;
        else if (studentGameSessionConfig.competences[i].parameters[j].Value.ToString() == "Números decimales")
            nType0 = Utils.NumberType.Decimal;

        for(int n = 0; n < numDatos; n++)
        {
            config.numConfig.Add(nType0);
        }
    }

    public void TryAgain()
    {
        hideIncorrectData();
        loginButton.enabled = true;
    }

    public void Logout()
    {
        showLogAfterLogout();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGameWithConfig()
    {
        LoadConfig();
        PlayGame();
    }

    private void LoadConfig()
    {
        SaveConfig();
        PlayGame();
    }

    public void LoadGameWithConfig1()
    {
        LoadConfig1();
        PlayGame();
    }

    public void LoadGameWithConfig2()
    {
        LoadConfig2();
        PlayGame();
    }

    private void LoadConfig1()
    {
        config = new MisionData();

        config.numConfig = new List<Utils.NumberType>();
        config.numConfig.Add(Utils.NumberType.Natural);
        config.numConfig.Add(Utils.NumberType.Natural);
        config.numConfig.Add(Utils.NumberType.Natural);
        config.numConfig.Add(Utils.NumberType.Natural);

        config.maxDigits = new List<int>();
        config.maxDigits.Add(1);
        config.maxDigits.Add(1);
        config.maxDigits.Add(1);
        config.maxDigits.Add(1);

        config.numDivMult = new List<int>();
        config.numDivMult.Add(2);
        config.numDivMult.Add(0);
        config.numDivMult.Add(0);
        config.numDivMult.Add(0);

        SaveConfig();
    }

    private void LoadConfig2()
    {
        config = new MisionData();

        config.numConfig = new List<Utils.NumberType>();
        config.numConfig.Add(Utils.NumberType.Natural);
        config.numConfig.Add(Utils.NumberType.Fraccion);
        config.numConfig.Add(Utils.NumberType.Fraccion);
        config.numConfig.Add(Utils.NumberType.Fraccion);

        config.maxDigits = new List<int>();
        config.maxDigits.Add(1);
        config.maxDigits.Add(1);
        config.maxDigits.Add(1);
        config.maxDigits.Add(1);

        config.numDivMult = new List<int>();
        config.numDivMult.Add(3);
        config.numDivMult.Add(0);
        config.numDivMult.Add(0);
        config.numDivMult.Add(0);
        SaveConfig();
    }

    private void SaveConfig()
    {
        SaveSystem.SaveConfigData(config);
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadLevelAsync(sceneName));
    }

    private IEnumerator LoadLevelAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }

}
