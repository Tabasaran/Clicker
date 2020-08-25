
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject yellowBird;
    public GameObject pinkBird;
    public GameObject deadEffect;
    public GameObject bonus;
    public GameObject pausePanel;

    public Slider gameDuratioSlider;
    public Slider bonusTimeSlider;

    public Text scoreText;

    private float maxX = 10f;
    private float maxY = 3.5f;
    private float minX = -10;
    private float minY = -4.5f;

    private Vector3 spawnBirdPosition;

    private float _bonusTime;
    private int _score;

    public int score { 
        get { return _score; } 
        set { _score = value; } 
    }


    void Start()
    {
        gameDuratioSlider.maxValue = 30f;
        gameDuratioSlider.value = 30f;

        _bonusTime = 3f;
        _score = 10;

        StartCoroutine("SpawnBird");
    }

    

    private void Update()
    {

        gameDuratioSlider.value -= Time.deltaTime;

        if (_score == 0 || gameDuratioSlider.value < 0.01f)
        {
            GameOver();
        }

        if (Input.GetMouseButtonDown(0) && pausePanel.activeSelf == false)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Bird"))
                {
                    Instantiate(deadEffect, hit.transform.position, Quaternion.identity);
                    Destroy(hit.collider.gameObject);

                    _score++;

                    bonusTimeSlider.value++;
                    if (bonusTimeSlider.value == bonusTimeSlider.maxValue)
                        ShowBonus();

                    ShowScore();
                }
                else if (hit.collider.gameObject.Equals(bonus))
                {
                    bonus.SetActive(false);
                    gameDuratioSlider.value += _bonusTime;
                }
                else
                    DroppingBonusTimer();
            }
            else
                DroppingBonusTimer();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void Restart()
    {
        PlayerPrefs.SetFloat("lastScore", _score);

        if (PlayerPrefs.GetFloat("bestScore") < _score)
            PlayerPrefs.SetFloat("bestScore", _score);

        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void GameOver()
    {
        PlayerPrefs.SetFloat("lastScore", _score);

        if (PlayerPrefs.GetFloat("bestScore") < _score)
            PlayerPrefs.SetFloat("bestScore", _score);

        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    private void ShowBonus()
    {
        bonus.transform.position = new Vector3(Random.Range(minX + 2f, maxX - 2f), Random.Range(minY + 0.5f, maxY), 0);
        bonus.SetActive(true);
        DroppingBonusTimer();
    }

    public void ShowScore()
    {
        scoreText.text = _score.ToString();
    }

    public void DroppingBonusTimer()
    {
        bonusTimeSlider.value = 0;
    }

    IEnumerator SpawnBird()
    {
        //random position
        spawnBirdPosition.x = Random.Range(0, 2) == 1 ? minX: maxX;
        spawnBirdPosition.y = Random.Range(minY, maxY);

        if (Random.Range(0, 2) == 1)
            Instantiate(yellowBird, spawnBirdPosition, Quaternion.identity);
        else
            Instantiate(pinkBird, spawnBirdPosition, Quaternion.identity);

        yield return new WaitForSeconds(Random.Range(.0f, 1.0f));
        StartCoroutine("SpawnBird");
    }
}
