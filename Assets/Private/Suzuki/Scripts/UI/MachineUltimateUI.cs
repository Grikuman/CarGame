using UnityEngine;
using UnityEngine.UI;

public class MachineUltimateUI : MonoBehaviour
{
    public VehicleController _vehicleController;
    private MachineUltimateModule _machineUltimateModule;

    [SerializeField] private Image _fillImage;               // �~�`�Q�[�W��Image

    public void Start()
    {
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
