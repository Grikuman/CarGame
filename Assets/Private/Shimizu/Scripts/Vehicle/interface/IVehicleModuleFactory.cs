
public interface IVehicleModuleFactory
{
    /// <summary> ���W���[���̍쐬 </summary>
    public IVehicleModule Create(VehicleController vehicleController);

    /// <summary> �ݒ�l�������l�Ƀ��Z�b�g���� </summary>
    public void ResetSettings(IVehicleModule module);
}
