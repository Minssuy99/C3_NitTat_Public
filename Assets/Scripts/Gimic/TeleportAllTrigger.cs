using Photon.Pun;
using UnityEngine;

public class TeleportAllTrigger : MonoBehaviour
{
    public Transform AllteleportTarget; // 텔레포트할 목표 위치

    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인
        if (other.CompareTag("Player"))
        {
            TeleportAllPlayers();
        }
    }

    void TeleportAllPlayers()
    {
        if (AllteleportTarget == null)
            return;

        // 모든 "Player" 태그를 가진 오브젝트를 찾기
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            // 각 플레이어 오브젝트를 목표 위치로 이동
            player.transform.position = AllteleportTarget.position;
        }
    }
}

