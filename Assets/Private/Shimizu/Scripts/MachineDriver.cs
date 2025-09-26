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
        // ���݂̃L�[�{�[�h���
        var current = Keyboard.current;


        // �L�[�{�[�h�ڑ��`�F�b�N
        if(current == null)
        {
            return;
        }

        // �L�[�̓��͏�Ԃ��擾����
        var rightKey = current.rightArrowKey;
        var leftKey  = current.leftArrowKey; 
        var upKey    = current.upArrowKey;
        var downKey  = current.downArrowKey;

        // �E�L�[��������Ă���Ƃ�
        if(rightKey.isPressed)
        {
            Debug.Log("�E�L�[�������ꂽ");
            _vehiclePhysicsModule._input = -1.0f;
        }
        // ���L�[��������Ă���Ƃ�
        else if(leftKey.isPressed)
        {
            Debug.Log("���L�[�������ꂽ");
            _vehiclePhysicsModule._input = 1.0f;
        }
        else
        {
            _vehiclePhysicsModule._input = 0.0f;
        }

        // ��L�[��������Ă���Ƃ�
        if (upKey.isPressed)
        {
            Debug.Log("��L�[�������ꂽ");
            _machineEngine.inputKey = 1.0f;
        }
        else
        {
            _machineEngine.inputKey = 0.0f;
        }

        // ���L�[��������Ă���Ƃ�
        if (downKey.isPressed)
        {
            Debug.Log("���L�[�������ꂽ");
        }
    }
}
