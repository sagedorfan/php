using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class StoryPauseMenu : MonoBehaviour
{
    [Header("Pause menu objects")]
    public GameObject pauseRoot;
    public CanvasGroup skullGroup;
    public Slider volumeSlider;
    public TMP_Text volumeText;

    [Header("Buttons")]
    public Button resumeButton;
    public Button mainMenuButton;
    public Button volumeDownButton;
    public Button volumeUpButton;

    [Header("Gameplay")]
    public string mainMenuSceneName = "MainMenu";
    public bool lockCursorOnResume = true;
    public MonoBehaviour[] gameplayScriptsToDisable;

    [Header("Volume and animation")]
    public float volumeStep = 0.1f;
    public float skullFadeSpeed = 0.35f;
    public float resumeRevealSeconds = 0.5f;
    public float resumeHoldSeconds = 0.25f;

    bool paused = false;
    bool resuming = false;
    float volume;

    void Start()
    {
        volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = volume;

        pauseRoot.SetActive(false);
        skullGroup.alpha = 0f;

        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        UpdateVolumeUI();

        resumeButton.onClick.AddListener(Resume);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        volumeDownButton.onClick.AddListener(VolumeDown);
        volumeUpButton.onClick.AddListener(VolumeUp);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void Update()
    {
        bool escapePressed;

#if ENABLE_INPUT_SYSTEM
        escapePressed = Keyboard.current.escapeKey.wasPressedThisFrame;
#else
        escapePressed = Input.GetKeyDown(KeyCode.Escape);
#endif

        if (escapePressed && !resuming)
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (paused && !resuming)
        {
            skullGroup.alpha = Mathf.MoveTowards(skullGroup.alpha, volume,
                Time.unscaledDeltaTime / skullFadeSpeed);
        }
    }

    void Pause()
    {
        paused = true;
        Time.timeScale = 0f;

        foreach (MonoBehaviour script in gameplayScriptsToDisable)
        {
            script.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        skullGroup.alpha = volume;
        UpdateVolumeUI();
        pauseRoot.SetActive(true);
        resumeButton.Select();
    }

    void Resume()
    {
        if (!paused || resuming) return;

        StartCoroutine(ResumeRoutine());
    }

    IEnumerator ResumeRoutine()
    {
        resuming = true;

        resumeButton.interactable = false;
        mainMenuButton.interactable = false;
        volumeDownButton.interactable = false;
        volumeUpButton.interactable = false;
        volumeSlider.interactable = false;

        float startAlpha = skullGroup.alpha;
        float timer = 0f;

        while (timer < resumeRevealSeconds)
        {
            timer += Time.unscaledDeltaTime;
            skullGroup.alpha = Mathf.Lerp(startAlpha, 1f, timer / resumeRevealSeconds);
            yield return null;
        }

        skullGroup.alpha = 1f;
        yield return new WaitForSecondsRealtime(resumeHoldSeconds);

        PlayerPrefs.Save();
        pauseRoot.SetActive(false);

        resumeButton.interactable = true;
        mainMenuButton.interactable = true;
        volumeDownButton.interactable = true;
        volumeUpButton.interactable = true;
        volumeSlider.interactable = true;

        paused = false;
        resuming = false;
        Time.timeScale = 1f;

        foreach (MonoBehaviour script in gameplayScriptsToDisable)
        {
            script.enabled = true;
        }

        if (lockCursorOnResume)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void VolumeDown()
    {
        SetVolume(volume - volumeStep);
    }

    void VolumeUp()
    {
        SetVolume(volume + volumeStep);
    }

    void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
        UpdateVolumeUI();
    }

    void UpdateVolumeUI()
    {
        volumeSlider.SetValueWithoutNotify(volume);
        volumeText.text = Mathf.RoundToInt(volume * 100f) + "%";
    }

    void GoToMainMenu()
    {
        PlayerPrefs.Save();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}