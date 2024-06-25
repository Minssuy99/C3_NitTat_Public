using Photon.Pun;
using UnityEngine;

public class ButtonHoldSwitch : MonoBehaviour
{
    public Sprite activatedSprite; // 버튼이 눌렸을 때 사용할 스프라이트
    public Sprite deactivatedSprite; // 버튼이 눌리지 않았을 때 사용할 스프라이트
    public GameObject targetObject; // 활성화/비활성화할 오브젝트
    public bool setActiveState = false; // true면 활성화, false면 비활성화
    public bool toggleMode = false; // true면 한 번 눌렀을 때 계속 비활성화, false면 누르는 동안만 비활성화

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
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인
        if (collision.gameObject.CompareTag("Player") && (!isActivated || toggleMode))
        {
            PV.RPC("ActivateButton", RpcTarget.All);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 충돌이 끝났을 때, "Player" 태그를 가진 오브젝트가 떠난 경우
        if (collision.gameObject.CompareTag("Player") && isActivated && !toggleMode)
        {
            PV.RPC("DeactivateButton", RpcTarget.All);
        }
    }

    [PunRPC]
    void ActivateButton()
    {
        // 버튼을 활성화 상태로 설정
        isActivated = true;

        // 스프라이트 변경
        if (activatedSprite != null)
        {
            spriteRenderer.sprite = activatedSprite;
        }

        // 목표 오브젝트의 활성화 상태 변경
        if (targetObject != null)
        {
            targetObject.SetActive(setActiveState);
        }
    }

    [PunRPC]
    void DeactivateButton()
    {
        // 버튼을 비활성화 상태로 설정
        isActivated = false;

        // 스프라이트 변경
        if (deactivatedSprite != null)
        {
            spriteRenderer.sprite = deactivatedSprite;
        }

        // 목표 오브젝트의 활성화 상태 변경
        if (targetObject != null)
        {
            targetObject.SetActive(!setActiveState);
        }
    }
}
