using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public float speed = 6f;
    public float fallGravity = 2.5f;
    public float boostForce = 6f;

    private Rigidbody2D rb;
    private Vector2 input;
    private Animator anim;
    private bool hasCrashed = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Al inicio no cae
        rb.gravityScale = 0f;
    }

    void Update()
    {
        if (!hasCrashed)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

            bool moving = input.sqrMagnitude > 0.001f;
            if (anim != null)
                anim.SetBool("isMoving", moving);
        }
        else
        {
            // Ya no tiene movimiento libre después del choque
            input = Vector2.zero;

            if (anim != null)
                anim.SetBool("isMoving", false);

            // Space funciona como impulso / "salto"
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * boostForce, ForceMode2D.Impulse);
            }
        }
    }

    void FixedUpdate()
    {
        if (!hasCrashed)
        {
            rb.linearVelocity = input * speed;
            ClampToCamera();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            hasCrashed = true;
            rb.gravityScale = fallGravity;
        }
    }

    void ClampToCamera()
    {
        var sr = GetComponent<SpriteRenderer>();
        Vector3 extents = sr.bounds.extents;

        Vector3 pos = transform.position;

        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

        pos.x = Mathf.Clamp(pos.x, min.x + extents.x, max.x - extents.x);
        pos.y = Mathf.Clamp(pos.y, min.y + extents.y, max.y - extents.y);

        transform.position = pos;
    }
}