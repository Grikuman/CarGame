using Logitech;
using UnityEngine;

public class SteeringController
{
    public enum  ButtonID : int
    {
        A       = 0,
        X       = 1,
        B       = 2,
        Y       = 3,
        R_SHIFT = 4,
        L_SHIFT = 5,
        R2      = 6,
        L2      = 7,
        SHARE   = 8,
        OPTIONS = 9,
        R3      = 10,
        L3      = 11,
    };

    public enum POVDirection : int
    {
        CENTER     = -1,
        UP         = 0,
        UP_RIGHT   = 4500,
        RIGHT      = 9000,
        DOWN_RIGHT = 13500,
        DOWN       = 18000,
        DOWN_LEFT  = 22500,
        LEFT       = 27000,
        UP_LEFT    = 31500,
    }

    // ���͒l�̍\����
    private LogitechGSDK.DIJOYSTATE2ENGINES _rec;

    // �O�t���[���̓��͒l
    private byte[] _buttons;

    // �O�t���[����POV�l
    private int _prevPOV = -1;
    // ���t���[����POV�l
    private int _currentPOV = -1;

    // �����Z���^�����O�i�����Ń[���ɖ߂�j�̗L��/�����t���O
    public bool _isAutoCenteringActive { get; set; } = true;

    // �X�e�A�����O�̊��x
    private float _steeringSensitivity;
    // �A�N�Z�����x
    private float _AcceleratorSensitivity;
    // �u���[�L���x
    private float _BrakeSensitivity;

    // �V���O���g���C���X�^���X
    private static SteeringController _instance;
    public static SteeringController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SteeringController();
                _instance.Initialize();
            }
            return _instance;
        }
    }

    /// <summary>
    /// ����������
    /// </summary>
    public void Initialize()
    {
        Debug.Log("SteeringInit:" + LogitechGSDK.LogiSteeringInitialize(false));

        // ���͂̏�Ԃ��X�V
        _rec = LogitechGSDK.LogiGetStateUnity(0);
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    public void Update()
    {
        // �R���g���[���[���ڑ�����Ă���ꍇSDK���X�V����
        if(LogitechGSDK.LogiIsConnected(0))
        {
            // �X�V�O�̓��͒l���擾����
            _buttons = _rec.rgbButtons;
            _prevPOV = _currentPOV;

            // �L����
            if (_isAutoCenteringActive)
            {
                LogitechGSDK.LogiPlaySpringForce(0, 0, 10000, 5000);
            }
            else
            {
                // ����
                LogitechGSDK.LogiStopSpringForce(0);
            }

            // ���W�N�[��SDK���X�V����
            if (LogitechGSDK.LogiUpdate())
            {
                // ���͂̏�Ԃ��X�V
                _rec = LogitechGSDK.LogiGetStateUnity(0);
                _currentPOV = (int)_rec.rgdwPOV[0];

            }
        }
    }

    /// <summary>
    /// �A�v���P�[�V�����I������
    /// </summary>
    public void OnApplicationQuit()
    {
        Debug.Log("SteeringShutdown:" + LogitechGSDK.LogiSteeringShutdown());
    }

    // �X�e�A�����O�R���g���[���[���ڑ�����Ă��邩
    public bool GetState() => LogitechGSDK.LogiIsConnected(0);

    /// <summary>
    /// �{�^����������Ă�����
    /// </summary>
    public bool GetButtonIsPressed(ButtonID id)
    {
        // ��ʓI�ȁu���̏�Ԃ������Ă�v����
        return _rec.rgbButtons[(int)id] == 128;
    }

    /// <summary>
    /// �{�^���������ꂽ�u��
    /// </summary>
    public bool GetButtonWasPressedThisFrame(ButtonID id)
    {
        // �O�t���[���ŉ�����Ă�����
        if (_buttons[(int)id] == 128)
        {
            return false;
        }
        // ������Ă��Ȃ��Ȃ�True��Ԃ�
        return _rec.rgbButtons[(int)id] == 128;
    }

    /// <summary>
    /// �{�^���������ꂽ�u��
    /// </summary>
    public bool GetButtonWasReleasedThisFrame(ButtonID id)
    {
        // �O�t���[���ŉ�����Ă�����
        if (_buttons[(int)id] == 128)
        {
            // ���݂̃t���[��������Ă�����
            return _rec.rgbButtons[(int)id] != 128;
        }

        return false;
    }


    /// <summary>
    /// ���ݎw�������������Ă��邩
    /// </summary>
    public bool GetPOVIsPressed(POVDirection id)
    {
        return _currentPOV == (int)id;
    }

    /// <summary>
    /// �w������������ꂽ�u��
    /// </summary>
    public bool GetPOVWasPressedThisFrame(POVDirection id)
    {
        return _prevPOV != (int)id && _currentPOV == (int)id;
    }

    /// <summary>
    /// �w������������ꂽ�u��
    /// </summary>
    public bool GetPOVWasReleasedThisFrame(POVDirection id)
    {
        return _prevPOV == (int)id && _currentPOV != (int)id;
    }

    /// <summary>
    /// �X�e�A�����O�̍��W���擾����
    /// </summary>
    /// <returns>�X�e�A�����O�̐��K�����l</returns>
    public float GetSteeringPosition()
    {
        // ���W�N�[���f�o�C�X���ڑ�����Ă��邩�ǂ����m�F
        if (LogitechGSDK.LogiIsConnected(0))
        {
            // �X�e�A�����O�̒l�i -32768 �` 32767�j��-1 �` 1�ɐ��K��
            float normalized = Mathf.Clamp(_rec.lX / 32767f, -1f, 1f);

            // �����_���ʂ܂Ő؂�̂�
            return Mathf.Floor(normalized * 100f) * 0.01f;
        }

        return 0f;
    }

    /// <summary>
    /// �A�N�Z���̍��W���擾����
    /// </summary>
    /// <returns>�A�N�Z���̐��K�����l</returns>
    public float GetAcceleratorPosition()
    {
        // ���W�N�[���f�o�C�X���ڑ�����Ă��邩�ǂ����m�F
        if (LogitechGSDK.LogiIsConnected(0))
        {
            // �A�N�Z���̒l�i -32768 �` 32767�j��0 �` 1�ɐ��K��
            float normalized = Mathf.Clamp01((32767f - _rec.lY) / 65535f);

            // �����_���ʂ܂Ő؂�̂�
            return Mathf.Floor(normalized * 100f) * 0.01f;
        }

        return 0f;
    }

    /// <summary>
    /// �u���[�L�̍��W���擾����
    /// </summary>
    /// <returns>�u���[�L�̐��K�����l</returns>
    public float GetBrakePosition()
    {
        // ���W�N�[���f�o�C�X���ڑ�����Ă��邩�ǂ����m�F
        if (LogitechGSDK.LogiIsConnected(0))
        {
            // �A�N�Z���̒l�i -32768 �` 32767�j��0 �` 1�ɐ��K��
            float normalized = Mathf.Clamp01((32767f - _rec.lRz) / 65535f);

            // �����_���ʂ܂Ő؂�̂�
            return Mathf.Floor(normalized * 100f) * 0.01f;
        }

        return 0f;
    }

}
