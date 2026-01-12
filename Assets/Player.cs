using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float laneDistance = 3f;        // Distância entre faixas
    public float laneChangeSpeed = 10f;    // Suavidade da troca de faixa
    public float jumpForce = 6f;

    [Header("Gravity")]
    public float gravityMultiplier = 2f;

    private Rigidbody rb;
    private Animator animator;

    private int currentLane = 1; // 0 = esquerda | 1 = meio | 2 = direita
    private bool isGrounded = false;
    private bool isDead = false;
    private bool canDie = false;

    // ================= START =================
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // ================= RIGIDBODY CONFIG =================
        rb.freezeRotation = true;
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // ================= POSIÇÃO SEGURA =================
        Vector3 pos = transform.position;
        pos.y = 1.1f; // garante que começa acima do chão
        transform.position = pos;

        // ================= GRAVIDADE =================
        Physics.gravity *= gravityMultiplier;

        // ================= ANIMAÇÃO =================
        animator.SetBool("isRunning", true);
        animator.SetBool("isGrounded", false);

        // Evita morte nos primeiros frames
        Invoke(nameof(EnableDeath), 0.5f);
    }

    void EnableDeath()
    {
        canDie = true;
    }

    // ================= UPDATE =================
    void Update()
    {
        if (isDead) return;
        HandleInput();
    }

    // ================= FIXED UPDATE =================
    void FixedUpdate()
    {
        if (isDead) return;
        MoveLanes();
    }

    // ================= INPUT =================
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
            ChangeLane(-1);

        if (Input.GetKeyDown(KeyCode.D))
            ChangeLane(1);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            Jump();
    }

    // ================= MOVIMENTO LATERAL =================
    void MoveLanes()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.x = (currentLane - 1) * laneDistance;

        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(targetPosition.x, transform.position.y, transform.position.z),
            Time.fixedDeltaTime * laneChangeSpeed
        );
    }

    void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, 0, 2);
    }

    // ================= PULO =================
    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);

        isGrounded = false;
        animator.SetTrigger("jump");
        animator.SetBool("isGrounded", false);
    }

    // ================= COLISÕES =================
    void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("GroundCity"))
    {
        isGrounded = true;
        animator.SetBool("isGrounded", true);
    }
}

    // ================= MORTE =================
    void Die()
    {
        if (isDead) return;
        isDead = true;

        // Para física
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        // Animações
        animator.SetBool("isRunning", false);
        animator.SetTrigger("Die");

        // Reinicia a cena após 2 segundos
        Invoke(nameof(RestartGame), 2f);
    }

    // ================= RESTART =================
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
