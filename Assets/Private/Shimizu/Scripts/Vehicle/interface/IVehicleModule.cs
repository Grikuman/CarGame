
public interface IVehicleModule
{
    /// <summary> �A�N�e�B�u��Ԃ�ݒ� </summary>
    public void SetActive(bool value);
    /// <summary> �A�N�e�B�u��Ԃ��擾 </summary>
    public bool GetIsActive();
   

    /// <summary> ���������� </summary>
    public void Initialize(VehicleController vehicleController);

    /// <summary> �J�n���� </summary>
    public void Start();

    /// <summary> �X�V���� </summary>
    public void UpdateModule();

    /// <summary> �����v�Z�X�V���� </summary>
    public void FixedUpdateModule();


}
