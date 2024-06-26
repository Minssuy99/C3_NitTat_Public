using UnityEngine;
using Photon.Pun;

public class StormItem : MonoBehaviour
{
    public GameObject stormPrefab; // ?€ν° ?λ¦¬?Ήμ ?¬κΈ°??? λΉ
    public float startX = 240f; // ?€ν° ?μ ?μΉ??x μ’ν
    public float endX = -20f; // ?€ν° ?μ°© ?μΉ??x μ’ν
    public float moveSpeed = 5f; // ?€ν°???΄λ ?λ
    public float minY = 0f; // ?€ν°???λ€ ?μ΄ μ΅μκ°?
    public float maxY = 20f; // ?€ν°???λ€ ?μ΄ μ΅λ?κ°?
    public int stormCount = 5; // ?μ±???€ν° κ°μ

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ?μ΄???€λΈ?νΈ ?κ΄΄
            Destroy(gameObject);

            // ?€ν° ?μ±
            for (int i = 0; i < stormCount; i++)
            {
                // ?λ€??Y μ’ν ?€μ 
                float randomY = Random.Range(minY, maxY);

                // ?€ν° ?λ¦¬?Ήμ ?Έμ€?΄μ€?ν???μ±
                GameObject storm = Instantiate(stormPrefab);
                storm.transform.position = new Vector3(startX, randomY, 0f); // ?μ ?μΉ???€ν° ?μ±

                // ?€ν°?κ² ?΄λ λͺλ Ή ?λ¬
                StormEffect stormEffect = storm.GetComponent<StormEffect>();
                if (stormEffect != null)
                {
                    stormEffect.SetMovement(endX, moveSpeed); // ?€ν°?κ² λͺ©ν ?μΉ? ?΄λ ?λ ?€μ 
                }
            }
        }
    }
}
