using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 0.5f;
    public float moveLimitLeft = -10f;
    public float moveLimitRight = 10f;

    [Header("Sprites / Animator")]
    public Sprite defaultSprite; // drag default sprite here
    public Sprite seedsSprite;   // sprite for spitting seeds if nothing to interact
    public Animator animator;    // optional
    private SpriteRenderer sr;

    [Header("Interaction")]
    public float interactRadius = 1.2f;
    public LayerMask interactableLayer;

    [Header("Audio")]
    public AudioClip walkSfx;
    public AudioClip interactSfx;
    public AudioClip spitSfx;
    private AudioSource audioSource;

    private Vector2 moveVector;
    Rigidbody2D rb;


    private PlayerInputActions input;  

    void Awake()
    {
        input = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        //if (sr == null) sr = gameObject.AddComponent<SpriteRenderer>();
        //if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable() => input.Enable();
    private void OnDisable() => input.Disable();

    void Update()
    {
        float move = input.Player.Move.ReadValue<float>();

        if (move > 0.01f)
            sr.flipX = false; // смотрит вправо
        else if (move < -0.01f)
            sr.flipX = true;  // смотрит влево
        // запускаем анимации
        if (animator)
        {
            animator.SetFloat("Speed", Mathf.Abs(move));
            animator.SetBool("isWalking", Mathf.Abs(moveVector.x) > 0.01f);
        }

    }
    void FixedUpdate()
    {
        float move = input.Player.Move.ReadValue<float>();
        moveVector.x = move;

        rb.linearVelocity = new Vector2(moveVector.x * speed, rb.linearVelocity.y);
    }

    void PlaySfx(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}