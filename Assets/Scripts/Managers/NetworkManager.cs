using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public enum RoomType
{
    PUBLIC,
    PRIVATE
}

public class NetworkManager : PunSingleTon<NetworkManager>
{
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListPrefab;
    [SerializeField] GameObject gameStartButton;

    public RoomType roomType;
    public int playerCnt;

    RoomInfo currentRoomInfo;

    private void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");
    }

    public void CreateRoom()
    {
        AudioManager.Instance.PlaySFX(SoundName.Button);

        if (MenuManager.Instance.CheckRoomOption())
        {
            RoomOptions options = new RoomOptions(); // 초기 룸 정보 세팅
            
            switch (roomType)
            {
                case RoomType.PRIVATE:
                    options.IsOpen = false; break;
                case RoomType.PUBLIC:
                    options.IsOpen = true; break;
            }
            options.MaxPlayers = playerCnt;

            PhotonNetwork.CreateRoom(MenuManager.Instance.roomInputField.text, options);
            MenuManager.Instance.OpenMenu("loading");
        }
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        MenuManager.Instance.roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        currentRoomInfo = PhotonNetwork.CurrentRoom;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        gameStartButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        gameStartButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        MenuManager.Instance.errorText.text = "Room Creation Failed : " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame()
    {
        Debug.Log(currentRoomInfo.PlayerCount);
        if (currentRoomInfo.PlayerCount == currentRoomInfo.MaxPlayers)
        {
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel("GameScene");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public void LeaveRoom()
    {
        AudioManager.Instance.PlaySFX(SoundName.Button);

        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public void LeaveRobby()
    {

        AudioManager.Instance.PlaySFX(SoundName.Button);

        PhotonNetwork.LeaveLobby();
        PhotonNetwork.LoadLevel("StartScene");
        PhotonNetwork.Disconnect();
        GameObject go = GameObject.Find("NetworkManager");
        GameObject go1 = GameObject.Find("RoomManager");
        Destroy(go); Destroy(go1);
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 룸 리스트 초기화
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        // 룸 리스트 업데이트
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }


}
