using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    public Transform[] podiumPositions; // 등수에 따른 시상식 자리 위치 배열
    private int currentMapIndex = 0;
    public int totalMaps = 3; // 총 맵 수 설정

    private int playerCount; // 플레이어 수 변수

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        playerCount = PhotonNetwork.PlayerList.Length; // 플레이어 수 초기화
    }

    public void NextMap()
    {
        Debug.Log(currentMapIndex);
        currentMapIndex++;
        if (currentMapIndex < totalMaps* playerCount)
        {
            LoadNextMap();
        }
        else
        {
            CalculateFinalScores();
        }
    }

    private void LoadNextMap()
    {
        // 다음 맵 로드 로직 추가
        Debug.Log("다음 맵으로 이동합니다. " + currentMapIndex + " " + (totalMaps* playerCount));
        // 예: SceneManager.LoadScene("Map" + currentMapIndex); 
    }

    private void CalculateFinalScores()
    {
        List<Dictionary<GameObject, int>> allMapScores = FinishLineManager.Instance.GetAllMapScores();
        Dictionary<GameObject, int> finalScores = new Dictionary<GameObject, int>();

        // 각 맵의 점수를 합산하여 최종 점수를 계산
        foreach (var mapScores in allMapScores)
        {
            foreach (var playerScore in mapScores)
            {
                if (!finalScores.ContainsKey(playerScore.Key))
                {
                    finalScores[playerScore.Key] = 0;
                }
                finalScores[playerScore.Key] += playerScore.Value;
            }
        }

        // 점수에 따라 내림차순으로 정렬
        List<KeyValuePair<GameObject, int>> sortedScores = new List<KeyValuePair<GameObject, int>>(finalScores);
        sortedScores.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

        // 최종 점수 출력 및 텔레포트
        Debug.Log("------- 최종 등수 -------");
        for (int i = 0; i < sortedScores.Count; i++)
        {
            GameObject player = sortedScores[i].Key;
            int score = sortedScores[i].Value;

            Debug.Log($"{i + 1}등: {player.name}, 총 점수: {score}");

            if (i < podiumPositions.Length)
            {
                TeleportPlayerToPodium(player, podiumPositions[i]);
            }
        }
    }

    private void TeleportPlayerToPodium(GameObject player, Transform podiumPosition)
    {
        if (player != null && podiumPosition != null)
        {
            player.transform.position = podiumPosition.position;
        }

        Invoke("GameStop", 2f);
    }

    public void ForceQuit()
    {
        CalculateFinalScores();
    }

    public void GameStop()
    {
        Time.timeScale = 0f;
    }

    public void GameResume()
    {
        Time.timeScale = 1f;
    }
}
