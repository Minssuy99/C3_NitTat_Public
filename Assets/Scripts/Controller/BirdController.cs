using System.Collections;
using UnityEngine;
using Photon.Pun;

public class BirdController : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public float minItemInterval = 0.5f;
    public float maxItemInterval = 2f;
    public float speed;
    public float destroyPositionX = -10f;

    private bool isArrived = false;
    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            StartCoroutine(DropItems());
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            MoveBird();
            CheckDestroy();
        }
    }

    void MoveBird()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    void CheckDestroy()
    {
        if (transform.position.x <= destroyPositionX && !isArrived)
        {
            isArrived = true;
            Destroy(gameObject);
        }
    }

    IEnumerator DropItems()
    {
        while (!isArrived)
        {
            float randomInterval = Random.Range(minItemInterval, maxItemInterval);
            yield return new WaitForSeconds(randomInterval);
            CreateRandomItem();
        }
    }

    void CreateRandomItem()
    {
        int randomIndex = Random.Range(0, itemPrefabs.Length);
        GameObject itemPrefab = itemPrefabs[randomIndex];

        Vector3 itemPosition = transform.position;
        itemPosition.y -= 0.3f;

        PhotonNetwork.Instantiate(itemPrefab.name, itemPosition, Quaternion.identity);
    }
}
