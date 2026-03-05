using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public float speed = 6f;

    private Rigidbody2D rb;
    private Vector2 input;
    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        bool moving = input.sqrMagnitude > 0.001f;
        if (anim != null)
            anim.SetBool("isMoving", moving);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = input * speed;
        ClampToCamera();
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