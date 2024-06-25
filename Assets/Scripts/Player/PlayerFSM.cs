using Photon.Pun;
using UnityEngine;

public enum PlayerState
{
    Idle = 0,
    Walking = 1,
    Jumping = 2,
    PushAndPull = 3
}

public class PlayerFSM : MonoBehaviour
{
    public PlayerState currentState;

    private Animator animator;
    private PlayerState previousState;

    PhotonView PV; //SH:photonview ���

    private void Awake()
    {
        PV = GetComponent<PhotonView>(); //SH:photonview ���
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        currentState = PlayerState.Idle; // �ʱ� ���� ����
        previousState = currentState;
        UpdateAnimator(); // �ʱ� ���¿� �´� �ִϸ��̼� ����
    }

    void Update()
    {
        // SH : �� �÷��̾ �� FSM�� ����
        if (!PV.IsMine)
            return;

        // ���¿� ���� ������ ���� ����
        switch (currentState)
        {
            case PlayerState.Idle:
                // Idle ������ ����
                break;
            case PlayerState.Walking:
                // Walking ������ ����
                break;
            case PlayerState.Jumping:
                // Jumping ������ ����
                break;
            case PlayerState.PushAndPull:
                // �а� ��� �� ������ ����
                break;
        }

        // ���°� ����Ǿ����� Ȯ��
        if (currentState != previousState)
        {
            UpdateAnimator();
            previousState = currentState;
        }
    }

    // ���� ���� �޼���
    public void ChangeState(PlayerState newState)
    {
        currentState = newState;
    }
    private void UpdateAnimator()
    {
        animator.SetInteger("State", (int)currentState);
    }


}
