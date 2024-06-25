using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    public Transform[] podiumPositions; // ����� ���� �û�� �ڸ� ��ġ �迭
    private int currentMapIndex = 0;
    public int totalMaps = 3; // �� �� �� ����

    private int playerCount; // �÷��̾� �� ����

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
        playerCount = PhotonNetwork.PlayerList.Length; // �÷��̾� �� �ʱ�ȭ
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
        // ���� �� �ε� ���� �߰�
        Debug.Log("���� ������ �̵��մϴ�. " + currentMapIndex + " " + (totalMaps* playerCount));
        // ��: SceneManager.LoadScene("Map" + currentMapIndex); 
    }

    private void CalculateFinalScores()
    {
        List<Dictionary<GameObject, int>> allMapScores = FinishLineManager.Instance.GetAllMapScores();
        Dictionary<GameObject, int> finalScores = new Dictionary<GameObject, int>();

        // �� ���� ������ �ջ��Ͽ� ���� ������ ���
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

        // ������ ���� ������������ ����
        List<KeyValuePair<GameObject, int>> sortedScores = new List<KeyValuePair<GameObject, int>>(finalScores);
        sortedScores.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

        // ���� ���� ��� �� �ڷ���Ʈ
        Debug.Log("------- ���� ��� -------");
        for (int i = 0; i < sortedScores.Count; i++)
        {
            GameObject player = sortedScores[i].Key;
            int score = sortedScores[i].Value;

            Debug.Log($"{i + 1}��: {player.name}, �� ����: {score}");

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
