using UnityEngine;
using UnityEngine.InputSystem;

public class MachineDriver : MonoBehaviour
{
    private MachineEngine _machineEngine;
    private VehiclePhysicsModule _vehiclePhysicsModule;

    private void Start()
    {
        _machineEngine = GetComponent<MachineEngine>();
        _vehiclePhysicsModule = GetComponent<VehiclePhysicsModule>();   
    }

    private void Update()
    {
        // 現在のキーボード情報
        var current = Keyboard.current;


        // キーボード接続チェック
        if(current == null)
        {
            return;
        }

        // キーの入力状態を取得する
        var rightKey = current.rightArrowKey;
        var leftKey  = current.leftArrowKey; 
        var upKey    = current.upArrowKey;
        var downKey  = current.downArrowKey;

        // 右キーが押されているとき
        if(rightKey.isPressed)
        {
            Debug.Log("右キーが押された");
            _vehiclePhysicsModule._input = -1.0f;
        }
        // 左キーが押されているとき
        else if(leftKey.isPressed)
        {
            Debug.Log("左キーが押された");
            _vehiclePhysicsModule._input = 1.0f;
        }
        else
        {
            _vehiclePhysicsModule._input = 0.0f;
        }

        // 上キーが押されているとき
        if (upKey.isPressed)
        {
            Debug.Log("上キーが押された");
            _machineEngine.inputKey = 1.0f;
        }
        else
        {
            _machineEngine.inputKey = 0.0f;
        }

        // 下キーが押されているとき
        if (downKey.isPressed)
        {
            Debug.Log("下キーが押された");
        }
    }
}
