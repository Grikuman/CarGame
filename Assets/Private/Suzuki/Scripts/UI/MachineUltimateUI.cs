using UnityEngine;
using UnityEngine.UI;

public class MachineUltimateUI : MonoBehaviour
{
    [SerializeField] private MachineUltimateController _machineUltimateController; // �v���C���[��UltimateSystem
    [SerializeField] private Image _fillImage;               // �~�`�Q�[�W��Image

    void Update()
    {
        if (_machineUltimateController != null && _fillImage != null)
        {
            _fillImage.fillAmount = _machineUltimateController.GetUltimateGaugeNormalized();
        }
    }
}
