using Photon.Pun;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public Animator targetAnimator; // 물체의 Animator 컴포넌트
    public string animationTriggerName = "FallTrigger"; // 애니메이션 트리거 이름
    public GameObject targetObject;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        //targetObject.SetActive(false);
        PV.RPC("Activing", RpcTarget.All, false); //SH:동기화
    }

    [PunRPC]
    private void Activing(bool isActivate)
    {
        targetObject.SetActive(isActivate);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PV.RPC("Activing", RpcTarget.All, true); //SH:동기화
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인
        if (other.CompareTag("Player"))
        {
            PV.RPC("TriggerAnim", RpcTarget.All); // SH: 애니메이션 서버 동기화
            // 애니메이션 트리거 발동
            //targetAnimator.SetTrigger(animationTriggerName);
        }
    }

    [PunRPC]
    private void TriggerAnim()
    {
        targetAnimator.SetTrigger(animationTriggerName);
    }
}
