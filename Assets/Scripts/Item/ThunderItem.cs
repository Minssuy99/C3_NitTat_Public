using UnityEngine;
using Photon.Pun;

public class ThunderItem : MonoBehaviourPunCallbacks
{
    public string uiTag = "ThunderUI"; // UI ?�브?�트???�그명을 ?�기???�당
    public GameObject thunderPrefab; // Thunder ?�리?�을 ?�기???�당

    public float minX = -20f; // X 좌표 최소�?
    public float maxX = 240f; // X 좌표 최�?�?
    public float heightY = 60f; // Y 좌표 ?�이 ?�정

    public int thunderCount = 60; // ?�성??Thunder 개수

    private Animator uiAnimator; // UI ?�브?�트??Animator 컴포?�트

    void Start()
    {
        // ?�그�??�용?�여 UI ?�브?�트 찾기
        GameObject[] uiObjects = GameObject.FindGameObjectsWithTag(uiTag);
        
        // ?�기?�는 가??처음 찾�? UI ?�브?�트�??�용?�도�??�시�?구현
        if (uiObjects.Length > 0)
        {
            GameObject uiObject = uiObjects[0];
            // UI ?�브?�트?�서 Animator 컴포?�트 가?�오�?
            uiAnimator = uiObject.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("UI ?�브?�트�?찾을 ???�습?�다. ?�그�??�인?�주?�요.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (uiAnimator != null)
                {
                    uiAnimator.SetTrigger("isTrigger");
                }


                for (int i = 0; i < thunderCount; i++)
                {
                    float randomX = Random.Range(minX, maxX);
                    Vector3 spawnPosition = new Vector3(randomX, heightY, 0f);


                    PhotonNetwork.Instantiate(thunderPrefab.name, spawnPosition, Quaternion.identity);
                }

                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
