using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

public class FinishLineManager : MonoBehaviourPunCallbacks
{
    private static FinishLineManager instance;
    public static FinishLineManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (typeof(FinishLineManager))
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<FinishLineManager>();
                        if (instance == null)
                        {
                            GameObject obj = new GameObject("FinishLineManager");
                            instance = obj.AddComponent<FinishLineManager>();
                            obj.AddComponent<PhotonView>(); // Add PhotonView component
                        }
                    }
                }
            }
            return instance;
        }
    }

    private List<GameObject> finishedPlayers = new List<GameObject>();
    private Dictionary<GameObject, int> playerScores = new Dictionary<GameObject, int>();
    private List<Dictionary<GameObject, int>> allMapScores = new List<Dictionary<GameObject, int>>();

    private int playerCount;
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        if (pv == null)
        {
            Debug.LogError("PhotonView component is missing on FinishLineManager.");
        }
    }

    private void Start()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
    }

    public void PlayerFinished(GameObject player)
    {
        if (player == null)
        {
            Debug.LogError("Player is null");
            return;
        }

        PhotonView playerPhotonView = player.GetComponent<PhotonView>();
        if (playerPhotonView == null)
        {
            Debug.LogError("PhotonView component is missing on the player");
            return;
        }

        if (pv == null)
        {
            Debug.LogError("PhotonView is null on FinishLineManager");
            return;
        }

        lock (finishedPlayers)
        {
            if (!finishedPlayers.Contains(player))
            {
                finishedPlayers.Add(player);
                int rank = finishedPlayers.Count - 1;
                // 순위에 따라 다른 점수 부여
                int[] scores = { 100, 75, 50, 25 }; // 순서대로 1등, 2등, 3등, 4등에 대한 점수
                int score = (rank < scores.Length) ? scores[rank] : 0;
                playerScores[player] = score;
                pv.RPC("RPC_PlayerFinished", RpcTarget.All, playerPhotonView.ViewID, rank + 1, score);
            }
        }
    }

    [PunRPC]
    private void RPC_PlayerFinished(int playerViewID, int rank, int score)
    {
        PhotonView playerPhotonView = PhotonView.Find(playerViewID);
        if (playerPhotonView != null)
        {
            GameObject player = playerPhotonView.gameObject;
            if (!finishedPlayers.Contains(player))
            {
                finishedPlayers.Add(player);
            }
            playerScores[player] = score;
            CheckAllPlayersFinished();
        }
        else
        {
            Debug.LogError("Player PhotonView not found for ID: " + playerViewID);
        }
    }

    private void CheckAllPlayersFinished()
    {
        if (finishedPlayers.Count == playerCount)
        {
            allMapScores.Add(new Dictionary<GameObject, int>(playerScores));
            playerScores.Clear();
            finishedPlayers.Clear();
            pv.RPC("RPC_NextMap", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_NextMap()
    {
        GameManager.Instance.NextMap();
    }

    public List<Dictionary<GameObject, int>> GetAllMapScores()
    {
        return new List<Dictionary<GameObject, int>>(allMapScores);
    }
}
