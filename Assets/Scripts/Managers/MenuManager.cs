using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] TMP_InputField nicknameInputField;
    [SerializeField] Menu[] menus;

    public TMP_InputField roomInputField;
    public TextMeshProUGUI errorText;
    public TextMeshProUGUI roomNameText;

    private GameObject visibleObj;
    private GameObject cntObj;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// UI 패널 열고 닫기
    /// </summary>
    /// <param name="menuName"></param>
    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if(menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if (menus[i].isOpen)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        if (CheckNickname(menu) == false) return;

        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].isOpen)
            {
                CloseMenu(menus[i]);
            }
            menu.Open();
        }
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
    /// <summary>
    /// 각 inputfield 비어있는지 확인
    /// </summary>
    /// <param name="menu"></param>
    /// <returns></returns>
    private bool CheckNickname(Menu menu)
    {
        if (menu.menuName == "create room" || menu.menuName == "find room")
        {
            if (string.IsNullOrEmpty(nicknameInputField.text))
            {
                nicknameInputField.GetComponent<Animator>().SetTrigger("Nickname");
                AudioManager.Instance.PlaySFX(SoundName.BBip);
                return false;
            }
            else
            {
                Debug.Log("닉네임 저장");
                PlayerSetting.nickname = nicknameInputField.text;
                PhotonNetwork.NickName = nicknameInputField.text;
                return true;
            }
        }

        return true;
    }

    public bool CheckRoomOption()
    {
        if (string.IsNullOrEmpty(roomInputField.text))
        {
            roomInputField.GetComponent<Animator>().SetTrigger("Nickname");
            AudioManager.Instance.PlaySFX(SoundName.BBip);
            return false;
        }

        if(visibleObj == null || cntObj == null)
        {
            // todo:error panel
            AudioManager.Instance.PlaySFX(SoundName.BBip);
            return false;
        }

        AudioManager.Instance.PlaySFX(SoundName.Button);
        return true;
    }

    /// <summary>
    /// 초기 방 설정값 넘기기 
    /// </summary>
    /// <param name="type"></param>
    public void VisibleButtonSelected(int type)
    {
        if(visibleObj != null)
        {
            visibleObj.GetComponent<Image>().enabled = false;
        }
        ChangeButton(1);
    }

    public void ButtonSelected(int playerCnt)
    {
        if (cntObj != null)
        {
            cntObj.GetComponent<Image>().enabled = false;
        }
        NetworkManager.Instance.playerCnt = playerCnt;
        ChangeButton(2);
    }

    /// <summary>
    /// 초기 방 설정 버튼 ui
    /// </summary>
    /// <param name="num"></param>
    private void ChangeButton(int num)
    {
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;

        switch (num)
        {
            case 1:
                visibleObj = clickObj;
                break;
            case 2:
                cntObj = clickObj; break;
        }

        clickObj.GetComponent<Image>().enabled = true;
    }
}
