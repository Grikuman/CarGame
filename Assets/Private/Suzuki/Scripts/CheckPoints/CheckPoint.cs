using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointID;
    [HideInInspector] public bool passed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckpointManager manager = FindObjectOfType<CheckpointManager>();
            if (manager != null)
            {
                manager.PassCheckpoint(other.gameObject, this);
                passed = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = passed ? Color.green : Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.4f);
    }
}
