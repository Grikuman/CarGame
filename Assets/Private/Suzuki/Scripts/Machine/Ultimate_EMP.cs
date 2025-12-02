using System.Linq;
using UnityEngine;

public class Ultimate_EMP : UltimateBase
{
    private float _range;        // EMPが届く距離
    private float _drainAmount;  // 敵へ与えるブースト減少量
    private float _healAmount;   // 自分が回復する量

    private VehicleController _selfVehicle;      // 自分のVehicle
    private MachineBoostModule _selfBoost;       // 自分のブーストモジュール
    private bool _hasExecuted = false;           // 効果を一度だけ起こすフラグ

    public Ultimate_EMP(float range, float drainAmount, float healAmount, float ultimateTime, VehicleController vc)
    {
        _range = range;
        _drainAmount = drainAmount;
        _healAmount = healAmount;
        _ultimateTime = ultimateTime;
        _selfVehicle = vc;
    }

    /// <summary>
    /// 発動時に呼ばれる
    /// </summary>
    public override void Activate(MachineEngineModule engine)
    {
        base.Activate(engine);

        // 自分のBoostモジュールを取得
        _selfBoost = _selfVehicle.Find<MachineBoostModule>();

        _hasExecuted = false; // 一回実行フラグリセット
    }

    /// <summary>
    /// 毎フレーム更新
    /// </summary>
    public override void Update()
    {
        base.Update();

        if (!_isActive || _hasExecuted) return;

        // EMP効果を一度だけ実行
        ExecuteEMP();
        _hasExecuted = true;
    }

    /// <summary>
    /// EMPの効果処理
    /// </summary>
    private void ExecuteEMP()
    {
        bool hitAnyEnemy = false; // 範囲内の敵を検出したかどうか

        // 全車両を取得
        var vehicles = GameObject.FindObjectsOfType<VehicleController>();

        foreach (var v in vehicles)
        {
            if (v == _selfVehicle) continue; // 自分は除外

            float distance = Vector3.Distance(_selfVehicle.transform.position, v.transform.position);

            // 範囲内の敵のみ
            if (distance <= _range)
            {
                // 敵のブースト
                var enemyBoost = v.Find<MachineBoostModule>();
                if (enemyBoost != null)
                {
                    enemyBoost.CurrentGauge -= _drainAmount;
                    enemyBoost.CurrentGauge = Mathf.Max(enemyBoost.CurrentGauge, 0.0f);

                    hitAnyEnemy = true; // 敵を1台以上巻き込んだ
                }
            }
        }

        // 敵を1台以上巻き込んだ場合のみ自分を回復
        if (hitAnyEnemy && _selfBoost != null)
        {
            _selfBoost.CurrentGauge += _healAmount;
            _selfBoost.CurrentGauge = Mathf.Min(_selfBoost.CurrentGauge, _selfBoost.MaxBoostGauge);
        }

        // ここでエフェクトやサウンドも再生可
        // EMPExplosionEffect.Play(_selfTransform.position);
    }


    public override void End()
    {
        base.End();
        // 継続効果なし
    }
}
