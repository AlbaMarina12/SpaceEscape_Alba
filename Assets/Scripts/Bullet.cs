using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public GameObject explosionPrefab; // NUEVO: para la explosión

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * speed; // se mantiene igual
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            // 💥 Crear explosión (si tienes prefab asignado)
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);
            }

            // 🧮 Sumar puntos
            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(10);
            }

            // 🔥 Lo que ya tenías
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}