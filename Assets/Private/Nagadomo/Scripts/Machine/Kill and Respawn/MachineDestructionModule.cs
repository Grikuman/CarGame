using UnityEngine;

public class MachineDestructionModule : IVehicleModule, IResettableVehicleModule<MachineDestructionModuleData>
{
    public float RearHitAngle { get; set; }

    private bool _isActive = true;
    private VehicleController _vehicleController;

    public void Initialize(VehicleController vehicleController)
    {
        _vehicleController = vehicleController;
    }

    public void Start()
    {
        _vehicleController.ResetSettings<MachineDestructionModuleData>();
    }

    public void SetActive(bool value) => _isActive = value;
    public bool GetIsActive() => _isActive;

    public void UpdateModule() 
    {

    }
    public void FixedUpdateModule() 
    {

    }

    public void ResetModule(MachineDestructionModuleData data)
    {
        RearHitAngle = data.RearHitAngle;
    }

    public void Collision(Collision collision)
    {
        if (!_isActive) return;
        if (!collision.collider.CompareTag("Player")) return;
        if (!collision.collider.TryGetComponent(out VehicleController otherVC))return;

        // 相手の forward と、自分方向ベクトルの角度で「後方ヒット」を判定
        Vector3 dirToMe = (_vehicleController.transform.position - otherVC.transform.position).normalized;
        float angle = Vector3.Angle(otherVC.transform.forward, dirToMe);

        if (angle < RearHitAngle)
        {
            // RespawnModule に Kill() を飛ばす
            var respawn = _vehicleController.Find<MachineRespawnModule>();
            respawn?.Kill();
        }

        Debug.Log("DestructionModule: HandleCollision CALLED");
    }

}
