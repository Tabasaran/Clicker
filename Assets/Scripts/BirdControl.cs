
using UnityEngine;

public class BirdControl : MonoBehaviour
{

    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(1f, 5f);
        transform.localScale *= Random.Range(0.4f, 1.2f);

        if (transform.position.x > 0f)
        {
            //flip
            transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
            speed *= -1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x) > 10.5f) 
            Destroy(gameObject);
    }
}
