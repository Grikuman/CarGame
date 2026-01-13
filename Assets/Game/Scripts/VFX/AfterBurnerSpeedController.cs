using UnityEngine;

public class AfterBurnerSpeedControl : MonoBehaviour, IVehicleReceiver
{
    [SerializeField]Rigidbody machineRigidbody;
    [SerializeField] private Renderer burnerRenderer;
    [SerializeField] private float maxSpeed = 150f;

    [SerializeField] private float minimumSpeed = 20f; //アフターバーナーが出る最低速度
    [SerializeField] private float minimumFactor = 0.5f; //アフターバーナーの最低出力

    private Material mat;

    public void Receipt(GameObject vehicle, Rigidbody rigidbody)
    {
        //machineRigidbody = rigidbody;
    }

    private void Start()
    {
        mat = burnerRenderer.material;
    }

    private void Update()
    {
        if (machineRigidbody == null) return;

        float speed = machineRigidbody.linearVelocity.magnitude;

        // 速度が minimumSpeed に満たない場合は0
        if (speed < minimumSpeed)
        {
            mat.SetFloat("_SpeedFactor", 0.0f);
            return;
        }

        // 演出が始まる速度帯の差分を計算
        float effectiveSpeedRange = maxSpeed - minimumSpeed;

        // 現在の速度から最低速度を差し引く
        float effectiveSpeed = speed - minimumSpeed;

        // 正規化
        float normalizedSpeed = Mathf.Clamp01(effectiveSpeed / effectiveSpeedRange);

        float factorRange = 1.0f - minimumFactor;
        float speedFactor = minimumFactor + normalizedSpeed * factorRange;

        // ShaderGraph に渡す
        mat.SetFloat("_SpeedFactor", speedFactor);
    }
}