using System.Collections;
using Michsky.MUIP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    public ButtonManager playButton;
    public ButtonManager settingsButton;
    public ButtonManager quitButton;
    public ButtonManager backButton;
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;
    public float startUpTime;

    void Start()
    {
        settingsPanel.gameObject.SetActive(false);
    }

    public void Play()
    {
        StopAllCoroutines();
        Debug.Log("Pressed Play!");
        //SceneManager.LoadScene(1); 
        StartCoroutine(PlayDelay());
    }

    public void Settings()
    {
        mainMenuPanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(true);
    }

    public void Back()
    {
        mainMenuPanel.gameObject.SetActive(true);
        settingsPanel.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator PlayDelay()
    {
        FindFirstObjectByType<EnemyAI>().enabled = true;
        yield return new WaitForSeconds(startUpTime);
        SceneManager.LoadScene(1);
    }
}
