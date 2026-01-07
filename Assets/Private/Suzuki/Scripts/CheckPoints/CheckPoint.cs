using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointID;
    [HideInInspector] public bool passed;

    private CheckpointManager checkpointManager;

    void Start()
    {
        checkpointManager = FindObjectOfType<CheckpointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerCheck")) return;
        if (checkpointManager == null) return;
        
        var networkChecker = other.GetComponent<NetworkChecker>();
        if(networkChecker)
        {
            if (!networkChecker.IsStateAuthority()) return;
        }

        checkpointManager.PassCheckpoint(this);
        passed = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = passed ? Color.green : Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.4f);
    }
}
