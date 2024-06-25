using Photon.Pun;
using UnityEngine;

public class TeleportOnTrigger : MonoBehaviour
{
    public Transform teleportTarget; // 텔레포트할 목표 위치
    public GameObject targetObject; // 텔레포트할 오브젝트

    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인
        if (other.CompareTag("Player"))
        {
            PhotonView pv = other.gameObject.GetPhotonView();
            if(pv.IsMine)
                // 타겟 오브젝트를 목표 위치로 텔레포트
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
