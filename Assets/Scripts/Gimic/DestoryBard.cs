using UnityEngine;
using Photon.Pun;

public class CollisionDestroyer : MonoBehaviourPunCallbacks
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹 �� ����
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
