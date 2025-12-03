using UnityEngine;

/// <summary>
/// 車のデータを受け取るためのインターフェース
/// </summary>
public interface IVehicleReceiver
{
    //受け取りの優先度(大きいほうから先に)
    public int Priority => 0;

    //車を受け取る
    public void Receipt(GameObject vehicle,Rigidbody rigidbody);
}
