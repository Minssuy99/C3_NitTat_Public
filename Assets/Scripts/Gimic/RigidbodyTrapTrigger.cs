using Photon.Pun;
using UnityEngine;

public class RigidbodyTrapTrigger : MonoBehaviour
{
    public GameObject targetObject; // 이동시킬 물체
    public float moveSpeed; // 이동 속도
    public float targetDistance; // 이동할 목표 거리

    public enum MoveDirection { Up, Down, Left, Right } // 이동 방향을 정의하는 enum
    public MoveDirection moveDirection = MoveDirection.Down; // 이동 방향

    private bool shouldMove = false; // 물체가 이동하기 시작할지 여부
    private Vector3 startPosition; // 시작 위치

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object is not assigned!");
        }
    }
    void Update()
    {
        if (shouldMove)
        {
            PV.RPC("MoveObj", RpcTarget.All);
        }
    }

    [PunRPC]
    private void MoveObj()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object is not assigned!");
            return;
        }
        Vector3 direction = Vector3.zero;

        // 선택한 방향에 따라 벡터 값을 설정
        switch (moveDirection)
        {
            case MoveDirection.Up:
                direction = Vector3.up;
                break;
            case MoveDirection.Down:
                direction = Vector3.down;
                break;
            case MoveDirection.Left:
                direction = Vector3.left;
                break;
            case MoveDirection.Right:
                direction = Vector3.right;
                break;
        }

        // 현재 프레임에서 이동할 거리 계산
        float moveAmount = moveSpeed * Time.deltaTime;
        Vector3 moveVector = direction * moveAmount;

        // 목표 거리와 남은 거리를 계산
        float remainingDistance = targetDistance - Vector3.Distance(startPosition, targetObject.transform.position);

        // 현재 프레임에서 이동할 거리가 남은 거리보다 큰지 확인
        if (moveAmount > remainingDistance)
        {
            moveVector = direction * remainingDistance;
            shouldMove = false; // 목표 거리에 도달하면 이동 멈춤
        }

        // 물체를 이동
        targetObject.transform.position += moveVector;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인
        if (other.CompareTag("Player"))
        {
            shouldMove = true; // 물체가 이동하기 시작
            if (targetObject != null)
            {
                startPosition = targetObject.transform.position; // 시작 위치 저장
            }
            else
            {
                Debug.LogError("Target Object is not assigned!");
            }
        }
    }
}
