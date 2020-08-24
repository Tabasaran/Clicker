using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public GameObject yellowBird;
    public GameObject pinkBird;
    public GameObject deadEffect;

    public Slider gameDuratioSlider;
    private float gameDurationTime;
    

    public Text scoreText;

    private int _score;
    public int score { 
        get { return _score; } 
        set { _score = value; } 
    }

    private Vector3 spawnPosition;


    void Start()
    {
        gameDurationTime = 30f;
        gameDuratioSlider.maxValue = gameDurationTime;
        _score = 10;

        StartCoroutine("SpawnBird");
    }

    private void Update()
    {
        gameDurationTime -= Time.deltaTime;
        gameDuratioSlider.value = gameDurationTime;

        if (_score == 0 || gameDurationTime < 0.01f)
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
                    ShowScore();
                }
            }
        }
    }

    public void ShowScore()
    {
        scoreText.text = _score.ToString();
    }

    IEnumerator SpawnBird()
    {
        //random position
        spawnPosition.x = Random.Range(0, 2) == 1 ? -10: 10;
        spawnPosition.y = Random.Range(-4.5f, 4.5f);

        if (Random.Range(0, 2) == 1)
            Instantiate(yellowBird, spawnPosition, Quaternion.identity);
        else
            Instantiate(pinkBird, spawnPosition, Quaternion.identity);

        yield return new WaitForSeconds(Random.Range(.0f, 2.0f));
        StartCoroutine("SpawnBird");
    }
}
