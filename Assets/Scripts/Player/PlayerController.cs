using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerFSM playerFSM;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask headLayer;
    [SerializeField] private Transform headCheck;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float groundCheckRadius = 0.2f;
    public float headCheckRadius = 0.2f;

    private Vector2 moveInput;
    private bool isGrounded;
    private bool isHeaded;
    private SpriteRenderer spriteRenderer;
    public GameObject pushableObject;
    private bool isPushing;
    private bool grabInput;

    PhotonView PV; //SH:photonview 사용

    private void Awake()
    {
        playerFSM = GetComponent<PlayerFSM>();
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        PV = GetComponent<PhotonView>(); //SH:photonview 사용

        // Movement Action에 대한 콜백 등록
        playerInput.actions["Move"].performed += OnMovePerformed;
        playerInput.actions["Move"].canceled += OnMoveCanceled;

        // Jump Action에 대한 콜백 등록
        playerInput.actions["Jump"].performed += OnJumpPerformed;

        // Grab Action에 대한 콜백 등록
        playerInput.actions["Grab"].performed += OnGrabPerformed;
        playerInput.actions["Grab"].canceled += OnGrabCanceled;
    }

    private void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }
    }

    private void Update()
    {
        // SH : 한 플레이어에 한 컨트롤러를 위해
        if (!PV.IsMine)
            return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isHeaded = Physics2D.OverlapCircle(headCheck.position, headCheckRadius, headLayer);

        if (isPushing && grabInput)
        {
            playerFSM.ChangeState(PlayerState.PushAndPull);
        }
        else if (moveInput.magnitude > 0)
        {
            playerFSM.ChangeState(PlayerState.Walking);
        }
        else
        {
            playerFSM.ChangeState(PlayerState.Idle);
        }

        // 스프라이트 방향 설정
        if (!isPushing)
        {
            if (moveInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (moveInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private void FixedUpdate()
    {
        // SH : 한 플레이어에 한 FSM를 위해
        if (!PV.IsMine)
            return;
        Move();
    }

    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        // 이동 입력에 따라 FSM 상태 변경
        if (moveInput.magnitude > 0)
        {
            playerFSM.ChangeState(PlayerState.Walking);
        }
        else
        {
            playerFSM.ChangeState(PlayerState.Idle);
        }
    }

    public void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
        playerFSM.ChangeState(PlayerState.Idle);
    }

    private void Move()
    {
        // 오직 수평 속도만 변경
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

        if (pushableObject != null && grabInput)
        {
            Rigidbody2D pushableRb = pushableObject.GetComponent<Rigidbody2D>();
            if (pushableRb != null)
            {
                pushableRb.velocity = new Vector2(moveInput.x * moveSpeed, pushableRb.velocity.y);
                isPushing = true;
            }
        }
        else
        {
            isPushing = false;
        }
    }

    public void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (isGrounded || isHeaded)
        {

            AudioManager.Instance.PlaySFX(SoundName.Jump);

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            playerFSM.ChangeState(PlayerState.Jumping);
        }
    }

    public void OnGrabPerformed(InputAction.CallbackContext context)
    {
        grabInput = true;
    }

    public void OnGrabCanceled(InputAction.CallbackContext context)
    {
        grabInput = false;
        isPushing = false; // 잡기 입력이 취소되면 밀기 상태도 취소
        playerFSM.ChangeState(PlayerState.Idle); // 기본 상태로 복귀
    }
    private void OnDrawGizmos()
    {
        // groundCheck의 위치에 빨간 원을 그려서 시각적으로 확인
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (headCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(headCheck.position, headCheckRadius);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Pushable") || collision.gameObject.CompareTag("Player")) && collision.gameObject == pushableObject)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Mathf.Abs(contact.normal.y) < 0.5f)
                {
                    isPushing = true; // 충돌이 지속되면 계속 밀기 상태 유지
                    return;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pushable") || collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Mathf.Abs(contact.normal.y) < 0.3f)
                {
                    pushableObject = collision.gameObject;
                    isPushing = true;
                    return;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == pushableObject)
        {
            pushableObject = null;
            isPushing = false;
            playerFSM.ChangeState(PlayerState.Idle);
        }
    }
}


