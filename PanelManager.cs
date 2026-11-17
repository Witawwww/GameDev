using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class PanelManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject gamePanel;
    public GameObject pausePanel;

    [Header("Main menu")]
    public Button continueButton;
    public Color continueEnabledColor = Color.white;
    public Color continueDisabledColor = new Color(1f, 1f, 1f, 0.35f);

    [Header("Video")]
    public VideoPlayer videoPlayer;

    GameObject returnTo;
    bool isPaused;
    Coroutine startRoutine;

    void Start()
    {
        ShowMainMenu();
    }

    void Update()
    {
        if (gamePanel != null && gamePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            SetPaused(!isPaused);
    }

    // Plain main menu (used at launch)
    public void OpenMainMenu() => ShowMainMenu();

    void ShowMainMenu()
    {
        isPaused = false;
        Time.timeScale = 1f;

        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        if (gamePanel != null) gamePanel.SetActive(false);
        pausePanel.SetActive(false);

        UpdateContinueButton();
    }

    void UpdateContinueButton()
    {
        if (continueButton == null) return;

        bool hasSave = PlayerPrefs.HasKey("SavedTime");
        continueButton.interactable = hasSave;

        var label = continueButton.GetComponentInChildren<TMP_Text>(true);
        if (label != null)
            label.color = hasSave ? continueEnabledColor : continueDisabledColor;
    }

    // Hook to the pause menu's "Main Menu" button
    public void ExitToMainMenu()
    {
        if (startRoutine != null) StopCoroutine(startRoutine);

        if (videoPlayer != null)
        {
            PlayerPrefs.SetFloat("SavedTime", (float)videoPlayer.time);
            PlayerPrefs.Save();
            videoPlayer.Stop();
        }
        ShowMainMenu();
    }

    // Hook to the main menu's "New Game" button
    public void NewGame()
    {
        PlayerPrefs.DeleteKey("SavedTime");
        StartGameAt(0);
    }

    // Hook to the main menu's "Continue" button
    public void ContinueGame()
    {
        StartGameAt(PlayerPrefs.GetFloat("SavedTime", 0f));
    }

    void StartGameAt(double time)
    {
        isPaused = false;
        Time.timeScale = 1f;

        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        if (gamePanel != null) gamePanel.SetActive(true);
        pausePanel.SetActive(false);

        if (videoPlayer == null) return;

        if (startRoutine != null) StopCoroutine(startRoutine);
        startRoutine = StartCoroutine(StartVideo(time));
    }

    IEnumerator StartVideo(double time)
    {
        videoPlayer.Stop();
        yield return null;               // let Stop() finish

        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();
        if (time > 0.1)
            videoPlayer.time = time;

        startRoutine = null;
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
        pausePanel.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;

        if (videoPlayer != null)
        {
            if (paused) videoPlayer.Pause();
            else videoPlayer.Play();
        }
    }

    public void OpenSettingsFrom(GameObject from)
    {
        returnTo = from;
        from.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        if (returnTo != null) returnTo.SetActive(true);
        else mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}