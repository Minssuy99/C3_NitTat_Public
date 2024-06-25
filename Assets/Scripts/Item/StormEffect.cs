using UnityEngine;
using Photon.Pun;

public class StormEffect : MonoBehaviour
{
    private float targetX;
    private float speed;
    public float pushForce = 20f; // 플레이어를 밀어내는 힘

    private bool hasAppliedForce = false; // 힘이 한 번만 적용되도록 제어하기 위한 변수
    public void SetMovement(float targetX, float speed)
    {
        this.targetX = targetX;
        this.speed = speed;
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // 도착지점에 도달하면 로컬에서 파괴
        if (transform.position.x <= targetX)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasAppliedForce && collision.gameObject.CompareTag("Player"))
        {
            // 충돌한 오브젝트가 플레이어인 경우
            PhotonView pv = collision.gameObject.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                // 플레이어 Rigidbody2D 가져오기
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    // 플레이어를 밀어내는 힘을 적용
                    Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                    pushDirection.x *= -1; // x 방향을 반전시켜서 밀도록 설정
                    playerRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                }
            }
            hasAppliedForce = true; // 힘이 적용되었음을 표시
        }
    }
}
