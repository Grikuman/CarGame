using UnityEngine;
using UnityEngine.UI;

public class MachineBoostUII : MonoBehaviour
{
    [SerializeField] private MachineBoostController _machineBoostController; // �u�[�X�g���W�b�N
    [SerializeField] private Image _fillImage;               // UI Image

    [Header("��Ԑݒ�")]
    private float _displayedFill = 0f;                       // UI�p�\���l
    [SerializeField] private float _smoothSpeed = 5f;       // ��ԑ��x

    [Header("�F�ݒ�")]
    [SerializeField] private Color _emptyColor = Color.red;  // �Q�[�W0%
    [SerializeField] private Color _fullColor = Color.cyan; // �Q�[�W100%

    void Update()
    {
        if (_machineBoostController == null || _fillImage == null) return;

        // ���W�b�N��̃Q�[�W����
        float targetFill = _machineBoostController.GetBoostGaugeNormalized();

        // ���炩���
        _displayedFill = Mathf.Lerp(_displayedFill, targetFill, Time.deltaTime * _smoothSpeed);

        // UI�ɔ��f
        _fillImage.fillAmount = _displayedFill;

        // �F�ω�
        _fillImage.color = Color.Lerp(_emptyColor, _fullColor, _displayedFill);
    }
}
