using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameController : MonoBehaviour
{

    public GameObject yellowBird;
    public GameObject pinkBird;
    public GameObject deadEffect;

    private Vector3 spawnPosition;

    void Start()
    {
        StartCoroutine("SpawnBird");
    }

    private void Update()
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
                }
            }
        }
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
