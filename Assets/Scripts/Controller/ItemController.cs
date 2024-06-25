using UnityEngine;
using Photon.Pun;

public class ItemController : MonoBehaviourPunCallbacks
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 30�� �Ŀ� DestroyItem �޼��带 ȣ���Ͽ� �������� ����
        Invoke("DestroyItem", 5f);
    }

    void Update()
    {
        // y ��ġ�� -20 �����̸� �ڵ����� ����
        if (transform.position.y < -20f)
        {
            DestroyItem();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true;
        }
    }

    void DestroyItem()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
