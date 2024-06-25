using UnityEngine;

public class RotateOnTrigger : MonoBehaviour
{
    public GameObject targetObject; // 회전시킬 물체
    public float rotationSpeed = 90.0f; // 회전 속도
    public float targetAngle = 90.0f; // 회전할 목표 각도

    public enum RotationDirection { Clockwise, CounterClockwise } // 회전 방향을 정의하는 enum
    public RotationDirection rotationDirection = RotationDirection.Clockwise; // 회전 방향

    private bool shouldRotate = false; // 물체가 회전하기 시작할지 여부
    private float totalRotation = 0.0f; // 총 회전된 각도

    void Update()
    {
        if (shouldRotate)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            if (rotationDirection == RotationDirection.CounterClockwise)
            {
                rotationAmount = -rotationAmount;
            }

            // 회전할 남은 각도를 계산
            float remainingAngle = targetAngle - Mathf.Abs(totalRotation);

            // 현재 프레임에서 회전할 각도가 남은 각도보다 큰지 확인
            if (Mathf.Abs(rotationAmount) > remainingAngle)
            {
                rotationAmount = Mathf.Sign(rotationAmount) * remainingAngle;
                shouldRotate = false; // 목표 각도에 도달하면 회전 멈춤
            }

            // 물체를 회전
            targetObject.transform.Rotate(0, 0, rotationAmount);
            totalRotation += rotationAmount; // 총 회전된 각도 업데이트
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인
        if (other.CompareTag("Player"))
        {
            shouldRotate = true; // 물체가 회전하기 시작
            totalRotation = 0.0f; // 총 회전된 각도 초기화
        }
    }
}
