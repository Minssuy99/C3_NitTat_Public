using UnityEngine;
using TMPro;
using Photon.Realtime;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI playerCntText;
    int playerCnt = 0;
    int playerMaxCnt = 0;

    public RoomInfo info;

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        nameText.text = _info.Name;
        playerCnt = _info.PlayerCount;
        playerMaxCnt = _info.MaxPlayers;
        playerCntText.text = $"({playerCnt}/{playerMaxCnt})";
    }

    public void OnClick()
    {
        if (playerCnt < playerMaxCnt)
        {
            NetworkManager.Instance.JoinRoom(info);
        }
    }
}
