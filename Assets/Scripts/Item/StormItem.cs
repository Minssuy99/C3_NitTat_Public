using UnityEngine;
using Photon.Pun;

public class StormItem : MonoBehaviour
{
    public GameObject stormPrefab; // ?�톰 ?�리?�을 ?�기???�당
    public float startX = 240f; // ?�톰 ?�작 ?�치??x 좌표
    public float endX = -20f; // ?�톰 ?�착 ?�치??x 좌표
    public float moveSpeed = 5f; // ?�톰???�동 ?�도
    public float minY = 0f; // ?�톰???�덤 ?�이 최소�?
    public float maxY = 20f; // ?�톰???�덤 ?�이 최�?�?
    public int stormCount = 5; // ?�성???�톰 개수

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ?�이???�브?�트 ?�괴
            Destroy(gameObject);

            // ?�톰 ?�성
            for (int i = 0; i < stormCount; i++)
            {
                // ?�덤??Y 좌표 ?�정
                float randomY = Random.Range(minY, maxY);

                // ?�톰 ?�리?�을 ?�스?�스?�하???�성
                GameObject storm = Instantiate(stormPrefab);
                storm.transform.position = new Vector3(startX, randomY, 0f); // ?�작 ?�치???�톰 ?�성

                // ?�톰?�게 ?�동 명령 ?�달
                StormEffect stormEffect = storm.GetComponent<StormEffect>();
                if (stormEffect != null)
                {
                    stormEffect.SetMovement(endX, moveSpeed); // ?�톰?�게 목표 ?�치?� ?�동 ?�도 ?�정
                }
            }
        }
    }
}
