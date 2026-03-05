using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Sprite[] obstacleSprites;

    public float spawnInterval = 1.2f;
    public float obstacleSpeed = 3.5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        Vector3 topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, 0f));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

        float x = Random.Range(topLeft.x, topRight.x);
        float y = topLeft.y + 1.0f;

        GameObject obj = Instantiate(obstaclePrefab, new Vector3(x, y, 0f), Quaternion.identity);

        var sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null && obstacleSprites != null && obstacleSprites.Length > 0)
        {
            sr.sprite = obstacleSprites[Random.Range(0, obstacleSprites.Length)];
        }

        var mover = obj.GetComponent<ObstacleMover>();
        if (mover == null) mover = obj.AddComponent<ObstacleMover>();
        mover.speed = obstacleSpeed;
    }
}

public class ObstacleMover : MonoBehaviour
{
    public float speed = 3.5f;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        Vector3 bottom = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        if (transform.position.y < bottom.y - 2f)
        {
            Destroy(gameObject);
        }
    }
}
