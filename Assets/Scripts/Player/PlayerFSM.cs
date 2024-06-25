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

    PhotonView PV; //SH:photonview 사용

    private void Awake()
    {
        PV = GetComponent<PhotonView>(); //SH:photonview 사용
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        currentState = PlayerState.Idle; // 초기 상태 설정
        previousState = currentState;
        UpdateAnimator(); // 초기 상태에 맞는 애니메이션 설정
    }

    void Update()
    {
        // SH : 한 플레이어에 한 FSM를 위해
        if (!PV.IsMine)
            return;

        // 상태에 따라 실행할 동작 설정
        switch (currentState)
        {
            case PlayerState.Idle:
                // Idle 상태의 동작
                break;
            case PlayerState.Walking:
                // Walking 상태의 동작
                break;
            case PlayerState.Jumping:
                // Jumping 상태의 동작
                break;
            case PlayerState.PushAndPull:
                // 밀고 당길 때 상태의 동작
                break;
        }

        // 상태가 변경되었는지 확인
        if (currentState != previousState)
        {
            UpdateAnimator();
            previousState = currentState;
        }
    }

    // 상태 변경 메서드
    public void ChangeState(PlayerState newState)
    {
        currentState = newState;
    }
    private void UpdateAnimator()
    {
        animator.SetInteger("State", (int)currentState);
    }


}
