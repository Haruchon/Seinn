using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private const float TWEENING_TIME = 0.5f;
    public static bool gamePaused = false;

    public bool gameOver = false;

    public GameObject pauseMenuUI;

    private Animator pauseMenuAnimator;
    private GameObject menuItems;
    private GameObject freeViewItems;
    private GameObject helperItems;
    private GameObject settingsItems;

    private void Start()
    {
        pauseMenuAnimator = pauseMenuUI.GetComponent<Animator>();
        menuItems = pauseMenuUI.transform.GetChild(0).gameObject;
        freeViewItems = pauseMenuUI.transform.GetChild(1).gameObject;
        helperItems = pauseMenuUI.transform.GetChild(5).gameObject;
        settingsItems = pauseMenuUI.transform.GetChild(7).gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private IEnumerator ShowHelperRoutine()
    {
        LeanTween.moveLocalY(helperItems, 0f, TWEENING_TIME);
        yield return new WaitForSeconds(TWEENING_TIME);
        Time.timeScale = 0f;
    }

    private IEnumerator HideHelperRoutine()
    {
        LeanTween.moveLocalY(helperItems, 550f, TWEENING_TIME);
        yield return new WaitForSeconds(TWEENING_TIME);
        Time.timeScale = 1f;
    }

    private IEnumerator ShowFreeView()
    {
        LeanTween.moveLocalX(freeViewItems,0f, TWEENING_TIME);
        yield return new WaitForSeconds(TWEENING_TIME);
        Time.timeScale = 0f;
    }

    private IEnumerator HideFreeView()
    {
        LeanTween.moveLocalX(freeViewItems, 220f, TWEENING_TIME);
        yield return new WaitForSeconds(TWEENING_TIME);
    }

    private IEnumerator ShowMenu()
    {
        Time.timeScale = 1f;
        LeanTween.moveLocalX(menuItems, 0f, TWEENING_TIME);
        yield return new WaitForSeconds(TWEENING_TIME);
    }

    private IEnumerator HideMenu()
    {
        LeanTween.moveLocalX(menuItems, 650f, TWEENING_TIME);
        yield return new WaitForSeconds(TWEENING_TIME);
    }

    private IEnumerator ShowSettingsRoutine()
    {
        Time.timeScale = 1f;
        LeanTween.moveLocalY(settingsItems, 0f, TWEENING_TIME);
        yield return new WaitForSeconds(TWEENING_TIME);
    }

    private IEnumerator HideSettingsRoutine()
    {
        LeanTween.moveLocalY(settingsItems, 550f, TWEENING_TIME);
        yield return new WaitForSeconds(TWEENING_TIME);
    }

    public void ShowHelper()
    {
        StartCoroutine(HideMenu());
        StartCoroutine(ShowHelperRoutine());
    }

    public void HideHelper()
    {
        StartCoroutine(HideHelperRoutine());
        StartCoroutine(ShowMenu());
    }

    public void ShowSettings()
    {
        StartCoroutine(HideMenu());
        StartCoroutine(ShowSettingsRoutine());
    }

    public void HideSettings()
    {
        StartCoroutine(HideSettingsRoutine());
        StartCoroutine(ShowMenu());
    }

    public void Hide()
    {
        pauseMenuAnimator.SetBool("active", false);
        StartCoroutine(HideMenu());
        StartCoroutine(ShowFreeView());
    }

    public void Show()
    {
        StartCoroutine(HideFreeView());
        pauseMenuAnimator.SetBool("active", true);
        StartCoroutine(ShowMenu());
    }

    public void Resume()
    {
        StartCoroutine(ResumeRoutine());
    }

    private IEnumerator ResumeRoutine()
    {
        yield return null;
        Time.timeScale = 1f;
        pauseMenuAnimator.SetBool("active", false);
        StartCoroutine(HideMenu());
        StartCoroutine(HideFreeView());
        StartCoroutine(HideHelperRoutine());
        StartCoroutine(HideSettingsRoutine());
        GameObject.FindObjectOfType<QuestManager>().HideQuestLog();
        gamePaused = false;
    }

    public void Pause()
    {
        StartCoroutine(PauseRoutine());
    }

    private IEnumerator PauseRoutine()
    {
        pauseMenuAnimator.SetBool("active", true);
        StartCoroutine(ShowMenu());
        yield return null;
        gamePaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetLevel()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene("Seinn");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
