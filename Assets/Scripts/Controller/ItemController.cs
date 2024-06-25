using UnityEngine;
using Photon.Pun;

public class ItemController : MonoBehaviourPunCallbacks
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 30초 후에 DestroyItem 메서드를 호출하여 아이템을 제거
        Invoke("DestroyItem", 5f);
    }

    void Update()
    {
        // y 위치가 -20 이하이면 자동으로 제거
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
