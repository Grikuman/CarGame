using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private HoverCarController m_car;
    private BoostSystem m_boost;
    private UltimateSystem m_ultimate;

    void Start()
    {
        // コンポーネントを取得する
        m_car = GetComponent<HoverCarController>();
        m_boost = GetComponent<BoostSystem>();
        m_ultimate = GetComponent<UltimateSystem>();
    }

    void Update()
    {
        // 移動入力
        m_car.forwardInput = Input.GetAxis("Vertical");
        m_car.turnInput = Input.GetAxis("Horizontal");

        // ブースト入力
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_boost.TryStartBoost();
        }

        // アルティメット入力
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            m_ultimate.TryActivateUltimate();
        }
    }
}
