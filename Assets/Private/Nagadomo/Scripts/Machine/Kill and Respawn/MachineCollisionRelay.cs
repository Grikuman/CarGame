using UnityEngine;

public class MachineCollisionRelay : MonoBehaviour
{
    private VehicleController _vehicleController;
    private MachineDestructionModule _destruction;

    public void Start()
    {
        _vehicleController = GetComponent<VehicleController>();
        _destruction = _vehicleController.Find<MachineDestructionModule>();

        Debug.Log("Relay Awake: VC=" + (_vehicleController != null) + ", Destruction=" + (_destruction != null));
    }

    private void OnCollisionEnter(Collision collision)
    {
        _destruction?.Collision(collision);
        Debug.Log("Relay hit: " + collision.collider.name);
    }
}
