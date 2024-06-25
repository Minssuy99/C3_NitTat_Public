using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnlineUI : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nicknameInputField;
    [SerializeField]
    private GameObject createRoomUI;

    /// <summary>
    /// 닉네임 입력 확인
    /// </summary>
    public void OnClickCreateRoomButton()
    {
        if(nicknameInputField.text != "")
        {
            PlayerSetting.nickname = nicknameInputField.text;
            createRoomUI.SetActive(true);
        }
        else
        {
            nicknameInputField.GetComponent<Animator>().SetTrigger("Nickname");
        }
    }
}
