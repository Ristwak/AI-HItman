using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    // public GameObject gamePanel;
    public GameObject comingSoonPanel;
    public GameObject exitPanel;
    private bool isComingSoonActive = false;

    void Start()
    {
        // gamePanel.SetActive(true);
        comingSoonPanel.SetActive(false);
        exitPanel.SetActive(false);
        isComingSoonActive = false;
    }

    void Update()
    {
        if (comingSoonPanel.activeSelf)
            isComingSoonActive = true;
        if (Input.GetKeyDown(KeyCode.Escape)) // Android back button
        {
            if (!exitPanel.activeSelf)
            {
                exitPanel.SetActive(true);
                comingSoonPanel.SetActive(false);
            }
        }

        if (exitPanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
    }

    public void OnExitYes()
    {
        SoundManager.Instance.PlaySound("Click");
        Application.Quit();
        Debug.Log("Game closed.");
    }

    public void OnExitNo()
    {
        SoundManager.Instance.PlaySound("Click");
        exitPanel.SetActive(false);
        if(isComingSoonActive)
            SceneManager.LoadScene("StartScene");
        else
            comingSoonPanel.SetActive(false);
        Time.timeScale = 1f; // Resume game if exit panel is closed
    }
}
