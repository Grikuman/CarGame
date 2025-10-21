using UnityEngine;
using UnityEngine.UI;

public class MachineUltimateUI : MonoBehaviour
{
    [SerializeField] private MachineUltimateController _machineUltimateController; // プレイヤーのUltimateSystem
    [SerializeField] private Image _fillImage;               // 円形ゲージのImage

    void Update()
    {
        if (_machineUltimateController != null && _fillImage != null)
        {
            _fillImage.fillAmount = _machineUltimateController.GetUltimateGaugeNormalized();
        }
    }
}
