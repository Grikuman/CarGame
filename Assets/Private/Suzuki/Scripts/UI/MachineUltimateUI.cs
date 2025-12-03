using UnityEngine;
using UnityEngine.UI;

public class MachineUltimateUI : MonoBehaviour,IVehicleReceiver
{
    private VehicleController _vehicleController;
    private MachineUltimateModule _machineUltimateModule;

    [SerializeField] private Image _fillImage;               // â~å`ÉQÅ[ÉWÇÃImage

    public void Receipt(GameObject vehicle, Rigidbody rigidbody)
    {
        _vehicleController = vehicle.GetComponent<VehicleController>();
        _machineUltimateModule = _vehicleController.Find<MachineUltimateModule>();
    }


    void Update()
    {
        if (_machineUltimateModule != null && _fillImage != null)
        {
            _fillImage.fillAmount = _machineUltimateModule.GetUltimateGaugeNormalized();
        }
    }
}
