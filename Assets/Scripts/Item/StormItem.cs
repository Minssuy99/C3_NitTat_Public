using UnityEngine;
using Photon.Pun;

public class StormItem : MonoBehaviour
{
    public GameObject stormPrefab; // ?¤í†° ?„ë¦¬?¹ì„ ?¬ê¸°??? ë‹¹
    public float startX = 240f; // ?¤í†° ?œì‘ ?„ì¹˜??x ì¢Œí‘œ
    public float endX = -20f; // ?¤í†° ?„ì°© ?„ì¹˜??x ì¢Œí‘œ
    public float moveSpeed = 5f; // ?¤í†°???´ë™ ?ë„
    public float minY = 0f; // ?¤í†°???œë¤ ?’ì´ ìµœì†Œê°?
    public float maxY = 20f; // ?¤í†°???œë¤ ?’ì´ ìµœë?ê°?
    public int stormCount = 5; // ?ì„±???¤í†° ê°œìˆ˜

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ?„ì´???¤ë¸Œ?íŠ¸ ?Œê´´
            Destroy(gameObject);

            // ?¤í†° ?ì„±
            for (int i = 0; i < stormCount; i++)
            {
                // ?œë¤??Y ì¢Œí‘œ ?¤ì •
                float randomY = Random.Range(minY, maxY);

                // ?¤í†° ?„ë¦¬?¹ì„ ?¸ìŠ¤?´ìŠ¤?”í•˜???ì„±
                GameObject storm = Instantiate(stormPrefab);
                storm.transform.position = new Vector3(startX, randomY, 0f); // ?œì‘ ?„ì¹˜???¤í†° ?ì„±

                // ?¤í†°?ê²Œ ?´ë™ ëª…ë ¹ ?„ë‹¬
                StormEffect stormEffect = storm.GetComponent<StormEffect>();
                if (stormEffect != null)
                {
                    stormEffect.SetMovement(endX, moveSpeed); // ?¤í†°?ê²Œ ëª©í‘œ ?„ì¹˜?€ ?´ë™ ?ë„ ?¤ì •
                }
            }
        }
    }
}
