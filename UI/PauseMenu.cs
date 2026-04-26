using Michsky.MUIP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private InputSystem_Actions InputSystemActions;
    public ButtonManager resumeButton;
    public ButtonManager settingsButton;
    public ButtonManager mainMenuButton;
    public SliderManager sensitivitySlider;
    public Canvas playerUI;
    public Canvas pauseMenu;
    public GameObject settingsPanel;
    public GameObject buttonsPanel;
    private bool isPaused;
    void Awake()
    {
        InputSystemActions = new InputSystem_Actions();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu.gameObject.SetActive(false);
        isPaused = false;
        sensitivitySlider.mainSlider.value = PlayerPrefs.GetFloat("Sensitivity") * 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputSystemActions.Player.Pause.triggered && !isPaused)
        {
            SetEnable();
        }
        else if (InputSystemActions.Player.Pause.triggered && isPaused)
        {
            SetDisable();
        }
    }

    public void SetEnable()
    {
        playerUI.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
        buttonsPanel.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(true);
        isPaused = true;
        GameState.SetUIOpen(true);
    }

    public void SetDisable()
    {

        pauseMenu.gameObject.SetActive(false);
        isPaused = false;
        GameState.SetUIOpen(false);
        playerUI.gameObject.SetActive(true);
    }

    public void Back()
    {
        buttonsPanel.gameObject.SetActive(true);
        settingsPanel.gameObject.SetActive(false);
    }

    public void Settings()
    {
        buttonsPanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnSensitivityChange()
    {
        CameraController.Instance.SetSensitivity(sensitivitySlider.mainSlider.value);
    }

    void OnEnable()
    {
        InputSystemActions.Enable();
    }
}
