using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Transform infoPanel;

    public void OpenInfoPanel() => infoPanel.gameObject.SetActive(true);
    public void CloseInfoPanel() => infoPanel.gameObject.SetActive(false);

    public void StartGame() => SceneManager.LoadScene(1);
    public void QuitGame() => Application.Quit();
}
