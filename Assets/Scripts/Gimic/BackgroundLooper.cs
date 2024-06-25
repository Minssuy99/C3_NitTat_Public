using System;
using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    [SerializeField] private float speed;   // 흐르는 속도
    [SerializeField] private int startIndex;    // 개체 수
    [SerializeField] private int endIndex;
    [SerializeField] private Transform[] sprites;           
    private float cameraViewWidth;
    private float cameraViewHeight;

    private void Awake()
    {
        cameraViewHeight = Camera.main.orthographicSize * 2;
        cameraViewWidth = Camera.main.aspect * cameraViewHeight;
    }
    private void Update()
    {
        MovingBackground();
        ScrollingBackground();
    }

    // 화면 움직이게 하는 기능
    void MovingBackground()
    {
        Vector3 currentPos = transform.position;
        Vector3 nextPos = Vector3.left * speed * Time.deltaTime;
        transform.position = currentPos + nextPos;
    }
    
    // 배경을 다시 젤 뒤로 보내는 기능
    void ScrollingBackground()
    {
        if (sprites[endIndex].position.x < - cameraViewWidth )
        {
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            Vector3 frontSpritePos = sprites[endIndex].localPosition;

            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.right * cameraViewWidth;

            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = (startIndexSave - 1 == -1) ? sprites.Length - 1 : startIndexSave - 1;
        }
    }
}