using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    //eblo baran'e
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Animation Settings")]
    public Animator playerAnimator;
    public KeyCode jumpButton = KeyCode.Space; //это пробел блять
    //второй пробел ебаный

    private CharacterController controller;
    [SerializeField] Transform orient; 
    private Vector3 velocity;
    private bool isGrounded;
    float moveX, moveZ; 

    private void Start() => controller = GetComponent<CharacterController>();  // Я ДИЛДО

    private void Update() // ВАЛЕРА ДИЛДАЧЬЁ 
    {
        GroundCheck();
        PlayerInput();
        // Управление анимациями (закомментировано)
        UpdateAnimation(moveX, moveZ); // Я ЕМ ГОВНО
    }
    void FixedUpdate() {
        CharacterMovement();
        ApplyingGravity(); 
    }
    void PlayerInput() {
        // Получение ввода
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        // Прыжок
        if ((Input.GetKeyDown(jumpButton) || Input.GetKey(jumpButton)) && isGrounded)
        {
            Jump(); 
        }
    }
    void ApplyingGravity() {
        // Гравитация
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    void GroundCheck() {
        // Проверка земли
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
    void Jump() {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
    void CharacterMovement() {
        // Относительное камере направление движения
        Vector3 moveDirection = orient.right * moveX + orient.forward * moveZ;
        moveDirection.y = 0;
        
        if (moveDirection.magnitude >= 0.1f)
        {
            controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
        }
    }
    private void UpdateAnimation(float moveX, float moveZ)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", new Vector2(moveX, moveZ).magnitude);
            playerAnimator.SetBool("IsGrounded", isGrounded);
        }
    }

    public void PlayDead()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsDead", true);
        }
    }
}