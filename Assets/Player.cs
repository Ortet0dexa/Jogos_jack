using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float laneDistance = 3f;
    public float laneChangeSpeed = 10f;
    public float jumpForce = 6f;

    [Header("Gravity")]
    public float gravityMultiplier = 2f;

    [Header("Audio")]
    public AudioSource runAudioSource;
    public AudioSource jumpAudioSource;
    public AudioSource slideAudioSource;

    private Rigidbody rb;
    private Animator animator;

    private int currentLane = 1; // 0 esquerda | 1 meio | 2 direita
    private bool isGrounded;
    private bool isDead;

    // ================= START =================
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.useGravity = true;

        // Gravidade global
        Physics.gravity = new Vector3(0, -9.81f * gravityMultiplier, 0);

        animator.SetBool("isRunning", true);
        animator.SetBool("isGrounded", false);
    }

    // ================= UPDATE =================
    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.A))
            ChangeLane(-1);

        if (Input.GetKeyDown(KeyCode.D))
            ChangeLane(1);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            Jump();

        if (Input.GetKeyDown(KeyCode.S) && isGrounded)
            Slide();

        HandleRunAudio();
    }

    // ================= FIXED =================
    void FixedUpdate()
    {
        if (isDead) return;
        MoveLanes();
    }

    // ================= MOVIMENTO =================
    void MoveLanes()
    {
        Vector3 target = transform.position;
        target.x = (currentLane - 1) * laneDistance;

        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(target.x, transform.position.y, transform.position.z),
            Time.fixedDeltaTime * laneChangeSpeed
        );
    }

    void ChangeLane(int dir)
    {
        currentLane = Mathf.Clamp(currentLane + dir, 0, 2);
    }

    // ================= PULO =================
    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        isGrounded = false;

        animator.SetTrigger("jump");
        animator.SetBool("isGrounded", false);

        // Som de pulo
        if (jumpAudioSource != null)
            jumpAudioSource.Play();
    }

    // ================= SLIDE =================
    void Slide()
    {
        animator.SetTrigger("slide");

        // Som de slide
        if (slideAudioSource != null)
            slideAudioSource.Play();
    }

    // ================= ÁUDIO DE CORRIDA =================
    void HandleRunAudio()
    {
        if (isGrounded && !isDead)
        {
            if (runAudioSource != null && !runAudioSource.isPlaying)
                runAudioSource.Play();
        }
        else
        {
            if (runAudioSource != null && runAudioSource.isPlaying)
                runAudioSource.Stop();
        }
    }

    // ================= CHÃO =================
    void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("GroundCity"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    // ================= MORTE =================
    void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        animator.SetBool("isRunning", false);
        animator.SetTrigger("Die");

        // Para sons
        if (runAudioSource != null)
            runAudioSource.Stop();

        if (GameManager.instance != null)
            GameManager.instance.GameOver();
    }

    // ================= RESTART =================
    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
