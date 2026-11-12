using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FreshMainMenu : MonoBehaviour
{
    [Header("Scene to load when Play is clicked")]
    public string gameplaySceneName;

    [Header("Buttons")]
    public Button playButton;
    public Button controlsButton;
    public Button exitButton;
    public Button backButton;

    [Header("Menu stuff")]
    public GameObject controlsPanel;
    public CanvasGroup skull;
    public Image background;

    [Header("Animation")]
    public float skullFadeDuration = 0.5f;
    public float playFadeDuration = 0.8f;

    bool hovering = false;
    bool startingGame = false;
    float bgTimer = 0f;
    Color bgColour;

    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        controlsPanel.SetActive(false);
        skull.alpha = 0f;
        bgColour = background.color;

        playButton.onClick.AddListener(PlayGame);
        controlsButton.onClick.AddListener(OpenControls);
        backButton.onClick.AddListener(CloseControls);
        exitButton.onClick.AddListener(QuitGame);

        AddHover(playButton);
        AddHover(controlsButton);
        AddHover(exitButton);
    }

    void AddHover(Button btn)
    {
        EventTrigger trigger = btn.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener((data) => { hovering = true; });
        trigger.triggers.Add(enter);

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { hovering = false; });
        trigger.triggers.Add(exit);
    }

    void Update()
    {
        if (!startingGame)
        {
            float target = 0f;
            if (hovering && !controlsPanel.activeSelf)
            {
                target = 1f;
            }

            skull.alpha = Mathf.MoveTowards(skull.alpha, target,
                Time.deltaTime / skullFadeDuration);
        }

        bgTimer += Time.deltaTime;
        float wave = 0.5f + 0.5f * Mathf.Cos(bgTimer * Mathf.PI * 2f / 10f);
        float brightness = Mathf.Lerp(0.92f, 1f, wave);

        background.color = new Color(bgColour.r * brightness,
            bgColour.g * brightness, bgColour.b * brightness, bgColour.a);
    }

    void OpenControls()
    {
        controlsPanel.SetActive(true);
        hovering = false;

        playButton.interactable = false;
        controlsButton.interactable = false;
        exitButton.interactable = false;
    }

    void CloseControls()
    {
        controlsPanel.SetActive(false);

        playButton.interactable = true;
        controlsButton.interactable = true;
        exitButton.interactable = true;
    }

    void PlayGame()
    {
        if (startingGame) return;

        startingGame = true;
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        playButton.interactable = false;
        controlsButton.interactable = false;
        exitButton.interactable = false;

        float startAlpha = skull.alpha;
        float timer = 0f;

        while (timer < playFadeDuration)
        {
            timer += Time.deltaTime;
            skull.alpha = Mathf.Lerp(startAlpha, 0f, timer / playFadeDuration);
            yield return null;
        }

        skull.alpha = 0f;
        SceneManager.LoadScene(gameplaySceneName);
    }

    void QuitGame()
    {
        Debug.Log("Quit pressed (only closes in a build)");
        Application.Quit();
    }
}