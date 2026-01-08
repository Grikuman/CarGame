using UnityEngine;

public class ResetMachineOnKey : MonoBehaviour
{
    [SerializeField] private KeyCode resetKey = KeyCode.KeypadEnter;
    [SerializeField] private Transform machine;           

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    void Start()
    {
        if (machine == null)
            machine = this.transform;

        defaultPosition = machine.position;
        defaultRotation = machine.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(resetKey))
        {
            ResetMachine();
        }
    }

    private void ResetMachine()
    {
        machine.SetPositionAndRotation(defaultPosition, defaultRotation);

        // Rigidbody ������ꍇ�͑��x���[����
        if (machine.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
