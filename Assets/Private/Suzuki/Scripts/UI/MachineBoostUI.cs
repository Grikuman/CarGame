using UnityEngine;
using UnityEngine.UI;

public class MachineBoostUII : MonoBehaviour
{
    public VehicleController _vehicleController;
    private MachineBoostModule _machineBoostModule;

    [SerializeField] private Image _fillImage;               // UI Image

    [Header("��Ԑݒ�")]
    private float _displayedFill = 0f;                       // UI�p�\���l
    [SerializeField] private float _smoothSpeed = 5f;       // ��ԑ��x

    [Header("�F�ݒ�")]
    [SerializeField] private Color _emptyColor = Color.red;  // �Q�[�W0%
    [SerializeField] private Color _fullColor = Color.cyan; // �Q�[�W100%

    public void Start()
    {
        _machineBoostModule = _vehicleController.Find<MachineBoostModule>();
    }

    void Update()
    {
        if (_machineBoostModule == null || _fillImage == null) return;

        // ���W�b�N��̃Q�[�W����
        float targetFill = _machineBoostModule.GetBoostGaugeNormalized();

        // ���炩���
        _displayedFill = Mathf.Lerp(_displayedFill, targetFill, Time.deltaTime * _smoothSpeed);

        // UI�ɔ��f
        _fillImage.fillAmount = _displayedFill;

        // �F�ω�
        _fillImage.color = Color.Lerp(_emptyColor, _fullColor, _displayedFill);
    }
}
