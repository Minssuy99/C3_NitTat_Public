using Photon.Pun;
using UnityEngine;

public class TeleportAllTrigger : MonoBehaviour
{
    public Transform AllteleportTarget; // �ڷ���Ʈ�� ��ǥ ��ġ

    void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ������Ʈ�� "Player" �±׸� ������ �ִ��� Ȯ��
        if (other.CompareTag("Player"))
        {
            TeleportAllPlayers();
        }
    }

    void TeleportAllPlayers()
    {
        if (AllteleportTarget == null)
            return;

        // ��� "Player" �±׸� ���� ������Ʈ�� ã��
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            // �� �÷��̾� ������Ʈ�� ��ǥ ��ġ�� �̵�
            player.transform.position = AllteleportTarget.position;
        }
    }
}

