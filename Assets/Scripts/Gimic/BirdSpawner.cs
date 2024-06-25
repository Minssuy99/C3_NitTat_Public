using System.Collections;
using UnityEngine;
using Photon.Pun;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab;        // ???�리??
    public float birdSpawnInterval = 60f; // ?��? ?�성?�는 간격
    public float birdSpeed = 10f;         // ?�의 ?�동 ?�도
    public Vector3 spawnPoint;           // ?��? ?�성???�치 지??
    public Vector3 destroyPosition;      // ?�라지???�치 지??

    private bool isArrived = false;

    PhotonView PV;

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        StartCoroutine(SpawnBird());
    }

    IEnumerator SpawnBird()
    {
        while (!isArrived)
        {
            yield return new WaitForSeconds(birdSpawnInterval);
            CreateBird();
        }
    }

    void CreateBird()
    {
        GameObject newBird = PhotonNetwork.Instantiate(birdPrefab.name, spawnPoint, Quaternion.identity);
        BirdController birdController = newBird.GetComponent<BirdController>();
        birdController.speed = birdSpeed;
        birdController.destroyPositionX = destroyPosition.x;
    }
}
