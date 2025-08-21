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
        // 移動入力
        car.forwardInput = Input.GetAxis("Vertical");
        car.turnInput = Input.GetAxis("Horizontal");

        // ブースト入力
        if (Input.GetKeyDown(KeyCode.Space))
        {
            boost.TryStartBoost();
        }

        // アルティメット入力
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            GetComponent<UltimateSystem>().TryActivateUltimate();
        }
    }
}
