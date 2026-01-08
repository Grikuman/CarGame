using UnityEngine;

public class MachineCollisionRelay : MonoBehaviour
{
    private VehicleController _vehicleController;
    private MachineDestructionModule _destruction;

    public void Start()
    {
        // Žæ“¾
        _vehicleController = GetComponent<VehicleController>();
        _destruction = _vehicleController.Find<MachineDestructionModule>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ”»’è‚Ì’Ê’m‚ð‘—‚é
        _destruction?.Trigger(other);
    }
}
