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

    PhotonView PV; //SH:photonview ���

    private void Awake()
    {
        playerFSM = GetComponent<PlayerFSM>();
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        PV = GetComponent<PhotonView>(); //SH:photonview ���

        // Movement Action�� ���� �ݹ� ���
        playerInput.actions["Move"].performed += OnMovePerformed;
        playerInput.actions["Move"].canceled += OnMoveCanceled;

        // Jump Action�� ���� �ݹ� ���
        playerInput.actions["Jump"].performed += OnJumpPerformed;

        // Grab Action�� ���� �ݹ� ���
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
        // SH : �� �÷��̾ �� ��Ʈ�ѷ��� ����
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

        // ��������Ʈ ���� ����
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
        // SH : �� �÷��̾ �� FSM�� ����
        if (!PV.IsMine)
            return;
        Move();
    }

    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        // �̵� �Է¿� ���� FSM ���� ����
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
        // ���� ���� �ӵ��� ����
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
        isPushing = false; // ��� �Է��� ��ҵǸ� �б� ���µ� ���
        playerFSM.ChangeState(PlayerState.Idle); // �⺻ ���·� ����
    }
    private void OnDrawGizmos()
    {
        // groundCheck�� ��ġ�� ���� ���� �׷��� �ð������� Ȯ��
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
                    isPushing = true; // �浹�� ���ӵǸ� ��� �б� ���� ����
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


