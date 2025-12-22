using UnityEngine;

/// <summary>
/// マシンの
/// </summary>
public class MachineViewModule : IVehicleModule, IResettableVehicleModule<MachineViewModuleData>
{
    GameObject m_vehicleViewModel;
    public GameObject VehcleModel { get {  return m_vehicleViewModel; } set { m_vehicleViewModel = value; } }

    public void FixedUpdateModule()
    {
        
    }

    public bool GetIsActive()
    {
        return true;
    }

    public void Initialize(VehicleController vehicleController)
    {
        if (vehicleController == null) return;
        //マシン生成
        GameObject model = UnityEngine.Object.Instantiate(m_vehicleViewModel,vehicleController.gameObject.transform);

        model.transform.localPosition = Vector3.zero;

    }

    public void ResetModule(MachineViewModuleData settings)
    {
        m_vehicleViewModel = settings.VehcleModel;
    }

    public void SetActive(bool value)
    {
        
    }

    public void UpdateModule()
    {
        
    }

    void IVehicleModule.Start()
    {
        
    }

}
