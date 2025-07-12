using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Button startButton; // Butonu Inspector'dan bağlayacağız
    public Button exitButton;

    void Start()
    {
        startButton.onClick.AddListener(LoadGameScene);
        exitButton.onClick.AddListener(OnExitYes);
    }

    void LoadGameScene()
    {
        SoundManager.Instance.PlaySound("Click");
        SceneManager.LoadScene("MainScene"); // GameScene'e geçiş yap
    }

    public void OnExitYes()
    {
        SoundManager.Instance.PlaySound("Click");
        Application.Quit();
        Debug.Log("Game closed.");
    }
}
