using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private HoverCarController car;
    private BoostSystem boost;

    void Start()
    {
        car = GetComponent<HoverCarController>();
        boost = GetComponent<BoostSystem>();
    }

    void Update()
    {
        // �ړ�����
        car.forwardInput = Input.GetAxis("Vertical");
        car.turnInput = Input.GetAxis("Horizontal");

        // �u�[�X�g����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            boost.TryStartBoost();
        }

        // �A���e�B���b�g����
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            GetComponent<UltimateSystem>().TryActivateUltimate();
        }
    }
}
