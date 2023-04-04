using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject colorMenuPanel;
    [SerializeField] private GameObject player;

    private Animator animator;
    private Renderer playerRenderer;

    void Start()
    {
        playerRenderer = player.GetComponent<Renderer>();
        animator = player.GetComponent<Animator>();
        colorMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuPanel.activeSelf) Resume();
            else Pause();
        }
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
        animator.enabled = false;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
        colorMenuPanel.SetActive(false);
        animator.enabled = true;
    }

    public void ColoraizeButton()
    {
        colorMenuPanel.SetActive(true);
    }

    public void ChangeColor(int colorId)
    {
        Color color;
        switch (colorId)
        {
            case 0:
                color = new Color(0.6666667f, 1, 0.6f, 0.95f);
                break;
            case 1:
                color = new Color(0.851f, 0.341f, 0.388f, 0.95f);
                break;
            case 2:
                color = new Color(0.6f, 0.8313726f, 0.03529412f, 0.95f);
                break;
            default:
                color = new Color(0.922f, 1f, 0.647f, 0.95f);
                break;
        }

        playerRenderer.material.SetColor("_NewColor", color);
        colorMenuPanel.SetActive(false);
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
