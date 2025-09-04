using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private HoverCarController m_car;
    private BoostSystem m_boost;
    private UltimateSystem m_ultimate;

    void Start()
    {
        // �R���|�[�l���g���擾����
        m_car = GetComponent<HoverCarController>();
        m_boost = GetComponent<BoostSystem>();
        m_ultimate = GetComponent<UltimateSystem>();
    }

    void Update()
    {
        // �ړ�����
        m_car.forwardInput = Input.GetAxis("Vertical");
        m_car.turnInput = Input.GetAxis("Horizontal");

        // �u�[�X�g����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_boost.TryStartBoost();
        }

        // �A���e�B���b�g����
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            m_ultimate.TryActivateUltimate();
        }
    }
}
