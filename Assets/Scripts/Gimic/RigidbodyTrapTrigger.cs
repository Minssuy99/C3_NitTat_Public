using Photon.Pun;
using UnityEngine;

public class RigidbodyTrapTrigger : MonoBehaviour
{
    public GameObject targetObject; // �̵���ų ��ü
    public float moveSpeed; // �̵� �ӵ�
    public float targetDistance; // �̵��� ��ǥ �Ÿ�

    public enum MoveDirection { Up, Down, Left, Right } // �̵� ������ �����ϴ� enum
    public MoveDirection moveDirection = MoveDirection.Down; // �̵� ����

    private bool shouldMove = false; // ��ü�� �̵��ϱ� �������� ����
    private Vector3 startPosition; // ���� ��ġ

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

        // ������ ���⿡ ���� ���� ���� ����
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

        // ���� �����ӿ��� �̵��� �Ÿ� ���
        float moveAmount = moveSpeed * Time.deltaTime;
        Vector3 moveVector = direction * moveAmount;

        // ��ǥ �Ÿ��� ���� �Ÿ��� ���
        float remainingDistance = targetDistance - Vector3.Distance(startPosition, targetObject.transform.position);

        // ���� �����ӿ��� �̵��� �Ÿ��� ���� �Ÿ����� ū�� Ȯ��
        if (moveAmount > remainingDistance)
        {
            moveVector = direction * remainingDistance;
            shouldMove = false; // ��ǥ �Ÿ��� �����ϸ� �̵� ����
        }

        // ��ü�� �̵�
        targetObject.transform.position += moveVector;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ������Ʈ�� "Player" �±׸� ������ �ִ��� Ȯ��
        if (other.CompareTag("Player"))
        {
            shouldMove = true; // ��ü�� �̵��ϱ� ����
            if (targetObject != null)
            {
                startPosition = targetObject.transform.position; // ���� ��ġ ����
            }
            else
            {
                Debug.LogError("Target Object is not assigned!");
            }
        }
    }
}
