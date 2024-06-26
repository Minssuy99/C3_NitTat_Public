using UnityEngine;

public class FinishLine : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FinishLineManager.Instance.PlayerFinished(other.gameObject);
        }
    }
}
