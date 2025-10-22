using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Sample Module Data")]
public class SampleModuleData : VehicleModuleFactoryBase
{

    [SerializeField] private int data1;
    [SerializeField] private int data2;
    [SerializeField] private int data3;
    [SerializeField] private int data4;

    [SerializeField] private float dataf1;
    [SerializeField] private float dataf2;
    [SerializeField] private float dataf3;
    [SerializeField] private float dataf4;

    [SerializeField] private Vector3 dataVec1;
    [SerializeField] private Vector3 dataVec2;
    [SerializeField] private Vector3 dataVec3;

    // �O������ǂݎ���p�ɂ���v���p�e�B
    public int Data1 => data1;
    public int Data2 => data2;
    public int Data3 => data3;
    public int Data4 => data4;

    public float Dataf1 => dataf1;
    public float Dataf2 => dataf2;
    public float Dataf3 => dataf3;
    public float Dataf4 => dataf4;

    public Vector3 DataVec1 => dataVec1;
    public Vector3 DataVec2 => dataVec2;
    public Vector3 DataVec3 => dataVec3;


    /// <summary> ���W���[�����쐬���� </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        return new SampleModule();
    }

    /// <summary> ���W���[���̐ݒ�l������������ </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<SampleModuleData> sampleModule)
        {
            sampleModule.ResetModule(this);
        }
    }
}
