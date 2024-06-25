using Photon.Pun;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public Animator targetAnimator; // ��ü�� Animator ������Ʈ
    public string animationTriggerName = "FallTrigger"; // �ִϸ��̼� Ʈ���� �̸�
    public GameObject targetObject;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        //targetObject.SetActive(false);
        PV.RPC("Activing", RpcTarget.All, false); //SH:����ȭ
    }

    [PunRPC]
    private void Activing(bool isActivate)
    {
        targetObject.SetActive(isActivate);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PV.RPC("Activing", RpcTarget.All, true); //SH:����ȭ
        // �浹�� ������Ʈ�� "Player" �±׸� ������ �ִ��� Ȯ��
        if (other.CompareTag("Player"))
        {
            PV.RPC("TriggerAnim", RpcTarget.All); // SH: �ִϸ��̼� ���� ����ȭ
            // �ִϸ��̼� Ʈ���� �ߵ�
            //targetAnimator.SetTrigger(animationTriggerName);
        }
    }

    [PunRPC]
    private void TriggerAnim()
    {
        targetAnimator.SetTrigger(animationTriggerName);
    }
}
