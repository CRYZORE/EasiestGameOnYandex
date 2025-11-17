using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    #region Переменные
    [Header("Movement Settings")]
    private float moveSpeed; 
    [SerializeField] private float walkSpeed = 6f; 
    [SerializeField] private float ladderSpeed = 3f; 
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    private Vector3 moveDir = Vector3.zero; 

    [Header("Animation Settings")]
    public Animator playerAnimator;

    [Header("Keybinds")]
    public KeyCode jumpButton = KeyCode.Space; 

    [Header("LadderClimbing")]
    [SerializeField] private float avoidFloorDistance = .1f; 
    public bool isClimbingLadder = false; 
    [SerializeField] private float ladderGrabDistance = .4f; 
    private Vector3 lastGrabbingLadderDirection; 
    private Ladder currentLadder;
    [SerializeField] private bool canUseLadders = true;
    [SerializeField] private float ladderFloorDropDistance = .1f; 

    private CharacterController controller;
    [SerializeField] Transform orient; 
    private Vector3 velocity;
    private bool isGrounded;
    float moveX, moveZ; 
    #endregion

    private void Start() {
        controller = GetComponent<CharacterController>();  
        moveSpeed = walkSpeed; 
    }

    private void Update()
    { 
        GroundCheck();
        PlayerInput();
        UpdateAnimation(moveX, moveZ); 
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
            if(isClimbingLadder) DropLadder(); 
            Jump(); 
        }
    }
    #region Гравитация и прыжок
    void ApplyingGravity() {
        // Гравитация
        if(isClimbingLadder) {
            velocity.y = 0;
            return; 
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    void GroundCheck() {
        if(isClimbingLadder) {
            isGrounded = true;
            return; 
        }
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
    #endregion

    void CharacterMovement() {
        // Относительное камере направление движения
        moveDir = orient.right * moveX + orient.forward * moveZ;
        moveDir.y = 0;

        if(moveDir.magnitude >= 0.1f) {
            if(canUseLadders) CheckForLadder(); 
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }
    #region Лестницы-лестницы...
    void CheckForLadder() {
        RaycastHit raycastHit; 
        
        if(!isClimbingLadder) {
            if(Physics.Raycast(orient.position + Vector3.up * avoidFloorDistance, moveDir, out raycastHit, ladderGrabDistance)) {
                if(raycastHit.transform.TryGetComponent(out Ladder ladder)) {
                    GrabLadder(moveDir, ladder); // Передаем лестницу
                }
            }
        } else {
            if(Physics.Raycast(orient.position + Vector3.up * avoidFloorDistance, lastGrabbingLadderDirection, out raycastHit, ladderGrabDistance)) {
                if(!raycastHit.transform.TryGetComponent(out Ladder ladder)) {
                    DropLadder();  
                    velocity.y = 4f; 
                }
            } 
            else { 
                DropLadder(); 
                velocity.y = 4f; 
            }
            if(moveZ < 0) {
                if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit floorRaycastHit, ladderFloorDropDistance)) {
                    DropLadder(); 
                }
            }
        }
        
        if(isClimbingLadder) {
            // Получаем локальные направления лестницы
            Vector3 ladderUp = currentLadder.transform.up;
            Vector3 ladderRight = currentLadder.transform.right;
            
            // Преобразуем глобальное движение в локальное относительно лестницы
            Vector3 localMove = Vector3.zero;
            localMove += ladderUp * moveZ;    // W/S - вверх/вниз по лестнице
            
            // Ограничиваем движение только вертикальным (комментируем если нужно движение в стороны)
            localMove.x = 0;
            localMove.z = 0;
            
            moveDir = localMove;
        }
    }

    void GrabLadder(Vector3 lastGrabbingLadderDirection, Ladder ladder) {
        isClimbingLadder = true; 
        this.lastGrabbingLadderDirection = lastGrabbingLadderDirection; 
        currentLadder = ladder; // Сохраняем ссылку на текущую лестницу
        moveSpeed = ladderSpeed; 
    }

    void DropLadder() {
        isClimbingLadder = false; 
        currentLadder = null; // Очищаем ссылку
        moveSpeed = walkSpeed; 
    }
    #endregion

    private void UpdateAnimation(float moveX, float moveZ)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", new Vector2(moveX, moveZ).magnitude);
            playerAnimator.SetBool("IsGrounded", isGrounded);
            if(canUseLadders) {
                playerAnimator.SetBool("IsClimbing", isClimbingLadder);
                if(isClimbingLadder) {
                    if(Mathf.Abs(moveZ) > 0) playerAnimator.SetFloat("climbAnimSpeed", 1f);
                    else playerAnimator.SetFloat("climbAnimSpeed", 0f);
                }
            }
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