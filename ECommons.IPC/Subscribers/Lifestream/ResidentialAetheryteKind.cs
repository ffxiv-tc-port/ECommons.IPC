using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommons.IPC.Subscribers.LifestreamIPC;

/// <summary>
/// Lifestream <c>Lifestream.Enums.ResidentialAetheryteKind</c> 的鏡像。
/// 提供端的型別是 private 到 Lifestream 組件裡的,跨 IPC 只能用成員名與數值都一致的鏡像型別接;
/// <c>CallGateChannel.ConvertObject</c> 對列舉預設序列化成**數值**,所以數值必須逐一相同。
/// (2026-09-05 逐值核對過 Lifestream/Lifestream/Enums/ResidentialAetheryteKind.cs)
/// </summary>
public enum ResidentialAetheryteKind
{
    Uldah = 9,
    Gridania = 2,
    Limsa = 8,
    Foundation = 70,
    Kugane = 111,
}
