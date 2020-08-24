using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject yellowBird;
    public GameObject pinkBird;
    public GameObject deadEffect;

    private float maxX = 10f;
    private float maxY = 4.5f;
    private float minX = -10;
    private float minY = -4.5f;

    public Slider gameDuratioSlider;
    public Slider bonusTime;

    public Text scoreText;

    private int _score;
    public int score { 
        get { return _score; } 
        set { _score = value; } 
    }

    private Vector3 spawnPosition;


    void Start()
    {
        gameDuratioSlider.maxValue = 30f;
        gameDuratioSlider.value = 30f;

        _score = 10;

        StartCoroutine("SpawnBird");
    }

    private void Update()
    {

        gameDuratioSlider.value -= Time.deltaTime;

        if (_score == 0 || gameDuratioSlider.value < 0.01f)
        {
            SceneManager.LoadScene("Game");
        }

        KillingBirds();
 
    }

    private void KillingBirds()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Bird"))
                {
                    Instantiate(deadEffect, hit.transform.position, Quaternion.identity);
                    Destroy(hit.collider.gameObject);

                    _score++;
                    bonusTime.value++;
                    if (bonusTime.value == 5)
                    {
                        gameDuratioSlider.value += 3f;
                        DroppingBonusTimer();
                    }
                    ShowScore();
                }
                else 
                    DroppingBonusTimer();
            }
            else
                DroppingBonusTimer();
        }
    }

    public void ShowScore()
    {
        scoreText.text = _score.ToString();
    }

    public void DroppingBonusTimer()
    {
        bonusTime.value = 0;
    }

    IEnumerator SpawnBird()
    {
        //random position
        spawnPosition.x = Random.Range(0, 2) == 1 ? minX: maxX;
        spawnPosition.y = Random.Range(minY, maxY);

        if (Random.Range(0, 2) == 1)
            Instantiate(yellowBird, spawnPosition, Quaternion.identity);
        else
            Instantiate(pinkBird, spawnPosition, Quaternion.identity);

        yield return new WaitForSeconds(Random.Range(.0f, 2.0f));
        StartCoroutine("SpawnBird");
    }
}
