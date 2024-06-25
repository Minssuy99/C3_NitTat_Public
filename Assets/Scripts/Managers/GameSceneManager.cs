using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameSceneManager : Singleton<GameSceneManager>
{
    public float fadeDuration = 2;
    public CanvasGroup fadePanel;
    public GameObject loading_UI;
    public Text loading_Percent;
    public Text loading_Text;
    public string[] loading_TextMessages;

    public GameObject parentObject; // 캐릭터 랜덤으로 가져올 부모 오브젝트
    private GameObject[] childObjects;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        UIManager.Instance.PrintRandomText(loading_Text, loading_TextMessages);


        if (parentObject != null)
        {
            childObjects = new GameObject[parentObject.transform.childCount];
            for (int i = 0; i < parentObject.transform.childCount; i++)
            {
                childObjects[i] = parentObject.transform.GetChild(i).gameObject;
            }
        }
    }

    // 게임 종료
    public void GameExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void ChangeScene(string sceneName)
    {
        fadePanel.DOFade(1, fadeDuration).OnStart(() => { fadePanel.blocksRaycasts = true; })
            .OnComplete(() => { StartCoroutine("LoadScene", sceneName); });
    }
    IEnumerator LoadScene(string sceneName)
    {
        loading_UI.SetActive(true);
        ActivateRandomChild();
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        float past_time = 0;
        float percentage = 0;

        while (!async.isDone)
        {
            yield return null;

            past_time += Time.deltaTime;

            if (percentage >= 90)
            {
                percentage = Mathf.Lerp(percentage, 100, past_time);

                if (percentage == 100)
                {
                    async.allowSceneActivation = true;
                }
            }
            else
            {
                percentage = Mathf.Lerp(percentage, async.progress * 100f, past_time);
                if (percentage >= 90) past_time = 0;
            }
            loading_Percent.text = percentage.ToString("0") + "%";
        }

    }
    // 이벤트 해제
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬 불러오기
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fadePanel.DOFade(0, fadeDuration)
        .OnStart(() =>
        {
            loading_UI.SetActive(false);
        })
        .OnComplete(() =>
        {
            fadePanel.blocksRaycasts = false;
        });
    }

    // 로딩씬 캐릭터 랜덤으로 활성화
    private void ActivateRandomChild()
    {
        if (childObjects == null || childObjects.Length == 0)
        {
            Debug.LogWarning("No child objects found or parent object is not assigned.");
            return;
        }

        foreach (var child in childObjects)
        {
            child.SetActive(false);
        }

        int randomIndex = Random.Range(0, childObjects.Length);
        childObjects[randomIndex].SetActive(true);
    }
}
