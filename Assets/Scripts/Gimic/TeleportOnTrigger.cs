using Photon.Pun;
using UnityEngine;

public class TeleportOnTrigger : MonoBehaviour
{
    public Transform teleportTarget; // �ڷ���Ʈ�� ��ǥ ��ġ
    public GameObject targetObject; // �ڷ���Ʈ�� ������Ʈ

    void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ������Ʈ�� "Player" �±׸� ������ �ִ��� Ȯ��
        if (other.CompareTag("Player"))
        {
            PhotonView pv = other.gameObject.GetPhotonView();
            if(pv.IsMine)
                // Ÿ�� ������Ʈ�� ��ǥ ��ġ�� �ڷ���Ʈ
                targetObject = other.gameObject;
                Teleport(pv);
        }
    }

    public void Teleport(PhotonView pv)
    {
        if (teleportTarget != null && targetObject != null)
        {
            if (pv.IsMine)
            {
                AudioManager.Instance.PlaySFX(SoundName.Die);
                targetObject.transform.position = teleportTarget.position;
            }
        }
    }
}
