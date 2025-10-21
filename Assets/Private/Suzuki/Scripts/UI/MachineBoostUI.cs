using UnityEngine;
using UnityEngine.UI;

public class MachineBoostUII : MonoBehaviour
{
    [SerializeField] private MachineBoostController _machineBoostController; // ブーストロジック
    [SerializeField] private Image _fillImage;               // UI Image

    [Header("補間設定")]
    private float _displayedFill = 0f;                       // UI用表示値
    [SerializeField] private float _smoothSpeed = 5f;       // 補間速度

    [Header("色設定")]
    [SerializeField] private Color _emptyColor = Color.red;  // ゲージ0%
    [SerializeField] private Color _fullColor = Color.cyan; // ゲージ100%

    void Update()
    {
        if (_machineBoostController == null || _fillImage == null) return;

        // ロジック上のゲージ割合
        float targetFill = _machineBoostController.GetBoostGaugeNormalized();

        // 滑らか補間
        _displayedFill = Mathf.Lerp(_displayedFill, targetFill, Time.deltaTime * _smoothSpeed);

        // UIに反映
        _fillImage.fillAmount = _displayedFill;

        // 色変化
        _fillImage.color = Color.Lerp(_emptyColor, _fullColor, _displayedFill);
    }
}
