
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public Text bestScore;
    public Text lastScore;

    void Start()
    {
        if (!PlayerPrefs.HasKey("bestScore"))
        {
            PlayerPrefs.SetInt("bestScore", 0);
        }
        if (!PlayerPrefs.HasKey("lastScore"))
        {
            PlayerPrefs.SetInt("lastScore", 0);
        }

        bestScore.text = PlayerPrefs.GetFloat("bestScore").ToString();
        lastScore.text = PlayerPrefs.GetFloat("lastScore").ToString();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
