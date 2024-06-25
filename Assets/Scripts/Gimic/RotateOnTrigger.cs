using UnityEngine;

public class RotateOnTrigger : MonoBehaviour
{
    public GameObject targetObject; // ȸ����ų ��ü
    public float rotationSpeed = 90.0f; // ȸ�� �ӵ�
    public float targetAngle = 90.0f; // ȸ���� ��ǥ ����

    public enum RotationDirection { Clockwise, CounterClockwise } // ȸ�� ������ �����ϴ� enum
    public RotationDirection rotationDirection = RotationDirection.Clockwise; // ȸ�� ����

    private bool shouldRotate = false; // ��ü�� ȸ���ϱ� �������� ����
    private float totalRotation = 0.0f; // �� ȸ���� ����

    void Update()
    {
        if (shouldRotate)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            if (rotationDirection == RotationDirection.CounterClockwise)
            {
                rotationAmount = -rotationAmount;
            }

            // ȸ���� ���� ������ ���
            float remainingAngle = targetAngle - Mathf.Abs(totalRotation);

            // ���� �����ӿ��� ȸ���� ������ ���� �������� ū�� Ȯ��
            if (Mathf.Abs(rotationAmount) > remainingAngle)
            {
                rotationAmount = Mathf.Sign(rotationAmount) * remainingAngle;
                shouldRotate = false; // ��ǥ ������ �����ϸ� ȸ�� ����
            }

            // ��ü�� ȸ��
            targetObject.transform.Rotate(0, 0, rotationAmount);
            totalRotation += rotationAmount; // �� ȸ���� ���� ������Ʈ
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ������Ʈ�� "Player" �±׸� ������ �ִ��� Ȯ��
        if (other.CompareTag("Player"))
        {
            shouldRotate = true; // ��ü�� ȸ���ϱ� ����
            totalRotation = 0.0f; // �� ȸ���� ���� �ʱ�ȭ
        }
    }
}
