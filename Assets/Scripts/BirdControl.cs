
using UnityEngine;

public class BirdControl : MonoBehaviour
{

    private float speed;
    private GameController gameController;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        speed = Random.Range(1f, 6f);
        transform.localScale *= Random.Range(0.5f, 1.5f);

        if (transform.position.x > 0f)
        {
            //flip
            transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
            speed *= -1f;
        }
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x) > 10.5f)
        {
            Destroy(gameObject);
            gameController.score -= 1;
            gameController.ShowScore();
        }
            
    }
}
