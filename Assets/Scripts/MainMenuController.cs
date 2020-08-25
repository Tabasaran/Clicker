
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public Text bestScore;
    public Text lastScore;

    void Start()
    {
        MusicManager.instance.audioSource.PlayOneShot(MusicManager.instance.menu);
        bestScore.text = PlayerPrefs.GetInt("bestScore", 0).ToString();
        lastScore.text = PlayerPrefs.GetInt("lastScore", 0).ToString();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
