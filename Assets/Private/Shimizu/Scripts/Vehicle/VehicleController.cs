using System;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{

    [SerializeField]
    private List<VehicleModuleFactoryBase> _moduleFactories = new List<VehicleModuleFactoryBase>();
    
    private Rigidbody _rb;
    private List<IVehicleModule> _modules = new List<IVehicleModule>();

    public float Steering { get; set; }
    public float Accelerator { get; set; }
    public float brake { get; set; }

    public bool boost { get; set; }
    public bool Ultimate { get; set; }

    private void Awake()
    {
        var usedTypes = new HashSet<System.Type>();

        foreach (var moduleFactory in _moduleFactories)
        {
            // ���W���[�����쐬����
            var module = moduleFactory.Create(this);
            if (module == null) continue;

            // �^�C�v���擾����
            Type moduleType = module.GetType();

            // �������W���[�����Ȃ����m�F����
            if (usedTypes.Contains(moduleType))
            {
                Debug.LogWarning($"Duplicate module type detected: {moduleType.Name}. Skipping.");
                continue;
            }

            // �^�C�v��ǉ�
            usedTypes.Add(moduleType);
            // ���W���[���̒ǉ�
            _modules.Add(module);
        }

        // �J�n����
        foreach (var module in _modules) module.Start();
    }

    /// <summary> �J�n���� </summary>
    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
    }

    /// <summary> �X�V���� </summary>
    private void Update()
    {
        // �e���W���[���̍X�V����
        foreach (var module in _modules)
        {
            if (module == null || !module.GetIsActive()) continue;

            // �X�V����
            module.UpdateModule();     
        }

        this.DrawRay();
    }
    /// <summary> �����v�Z�X�V���� </summary>
    private void FixedUpdate()
    {
        // �e���W���[���̍X�V����
        foreach (var module in _modules)
        {
            if (module == null || !module.GetIsActive()) continue;

            // �����v�Z����
            module.FixedUpdateModule(); 
        }

        this.DrawRay();
    }

    /// <summary> �w�肵���^�̃��W���[�����擾 </summary>
    /// <typeparam name="T"> �擾���������W���[���̌^ </typeparam>
    /// <returns> �w�肵���^�̃��W���[�� ���݂��Ȃ��ꍇ�� null </returns>
    public T Find<T>() where T : class, IVehicleModule
    {
        foreach (var module in _modules)
        {
            if (module is T tModule) return tModule;
        }

        Debug.LogWarning($"[VehicleController] Module of type {typeof(T).Name} not found.");
        return null;
    }

    /// <summary> �w�肵���^�̃t�@�N�g���[�ɑΉ����郂�W���[���̐ݒ�����Z�b�g���܂� </summary>
    /// <typeparam name="T"> ���Z�b�g�Ώۂ̃t�@�N�g���[�^ </typeparam>
    public void ResetSettings<T>() where T : class, IVehicleModuleFactory
    {
        int n = 0;

        foreach (var module in _moduleFactories)
        {
            if (module is T tModule)
            {
                module.ResetSettings(_modules[n]);
                return;
            }
            n++;
        }
    }

    /// <summary> �x�N�g���̕\�� </summary>
    private void DrawRay()
    {
        Vector3 velocity = _rb.linearVelocity;

        // �e�����̃x�N�g��
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 steeringDirection = Quaternion.Euler(0, 10, 0) * forward;

        // �e�����ւ̑��x�����iDot�ρj
        float forwardMag  = Vector3.Dot(velocity, forward);
        float rightMag    = Vector3.Dot(velocity, right);
        float steeringMag = Vector3.Dot(velocity, steeringDirection);

        // �X�P�[�����O�i���o��̒����j
        float scale = 0.5f;

        // Debug�\���i�����͊e�����̐����ɔ��j
        Debug.DrawRay(transform.position, forward * forwardMag * scale, Color.green);              // �O����
        Debug.DrawRay(transform.position, right * rightMag * scale, Color.red);                    // ������
        Debug.DrawRay(transform.position, steeringDirection * steeringMag * scale, Color.magenta); // �X�e�A����
        Debug.DrawRay(transform.position, velocity * scale, Color.blue);                           // ���ۂ̑��x�x�N�g��
    }


}
