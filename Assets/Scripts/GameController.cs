using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject yellowBird;
    public GameObject pinkBird;

    private Vector3 spawnPosition;

    void Start()
    {
        StartCoroutine("SpawnObj");
    }


    IEnumerator SpawnObj()
    {
        spawnPosition.x = Random.Range(0, 2) == 1 ? -10: 10;
        spawnPosition.y = Random.Range(-4.5f, 4.5f);

        if (Random.Range(0, 2) == 1)
            Instantiate(yellowBird, spawnPosition, Quaternion.identity);
        else
            Instantiate(pinkBird, spawnPosition, Quaternion.identity);

        yield return new WaitForSeconds(Random.Range(.0f, 5.0f));
        StartCoroutine("SpawnObj");
    }
}
