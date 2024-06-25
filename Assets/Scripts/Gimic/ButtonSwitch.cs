using Photon.Pun;
using UnityEngine;

public class ButtonHoldSwitch : MonoBehaviour
{
    public Sprite activatedSprite; // ��ư�� ������ �� ����� ��������Ʈ
    public Sprite deactivatedSprite; // ��ư�� ������ �ʾ��� �� ����� ��������Ʈ
    public GameObject targetObject; // Ȱ��ȭ/��Ȱ��ȭ�� ������Ʈ
    public bool setActiveState = false; // true�� Ȱ��ȭ, false�� ��Ȱ��ȭ
    public bool toggleMode = false; // true�� �� �� ������ �� ��� ��Ȱ��ȭ, false�� ������ ���ȸ� ��Ȱ��ȭ

    private SpriteRenderer spriteRenderer;
    private bool isActivated = false;

    PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ������Ʈ�� "Player" �±׸� ������ �ִ��� Ȯ��
        if (collision.gameObject.CompareTag("Player") && (!isActivated || toggleMode))
        {
            PV.RPC("ActivateButton", RpcTarget.All);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // �浹�� ������ ��, "Player" �±׸� ���� ������Ʈ�� ���� ���
        if (collision.gameObject.CompareTag("Player") && isActivated && !toggleMode)
        {
            PV.RPC("DeactivateButton", RpcTarget.All);
        }
    }

    [PunRPC]
    void ActivateButton()
    {
        // ��ư�� Ȱ��ȭ ���·� ����
        isActivated = true;

        // ��������Ʈ ����
        if (activatedSprite != null)
        {
            spriteRenderer.sprite = activatedSprite;
        }

        // ��ǥ ������Ʈ�� Ȱ��ȭ ���� ����
        if (targetObject != null)
        {
            targetObject.SetActive(setActiveState);
        }
    }

    [PunRPC]
    void DeactivateButton()
    {
        // ��ư�� ��Ȱ��ȭ ���·� ����
        isActivated = false;

        // ��������Ʈ ����
        if (deactivatedSprite != null)
        {
            spriteRenderer.sprite = deactivatedSprite;
        }

        // ��ǥ ������Ʈ�� Ȱ��ȭ ���� ����
        if (targetObject != null)
        {
            targetObject.SetActive(!setActiveState);
        }
    }
}
