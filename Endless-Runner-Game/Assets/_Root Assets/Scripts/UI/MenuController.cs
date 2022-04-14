using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Transform infoPanel;
    [SerializeField] private Text highScore;

    private void Awake()
    {
        highScore.text = HighScoreSaveManager.GetHighScore().ToString();
    }

    public void OpenInfoPanel() => infoPanel.gameObject.SetActive(true);
    public void CloseInfoPanel() => infoPanel.gameObject.SetActive(false);

    public void StartGame() => SceneManager.LoadScene(1);
    public void QuitGame() => Application.Quit();
    
    
}
