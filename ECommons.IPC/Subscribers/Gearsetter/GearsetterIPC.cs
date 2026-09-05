using System;
using System.Collections.Generic;

namespace ECommons.IPC.Subscribers.Gearsetter
{
    using EzIpcManager;
    using FFXIVClientStructs.FFXIV.Client.Game;
    using FFXIVClientStructs.FFXIV.Client.UI.Misc;

    public sealed class GearsetterIPC : IPCBase
    {
        public GearsetterIPC()
        {
        }

        public GearsetterIPC(SafeWrapper wrapper) : base(wrapper)
        {
        }

        public override string InternalName { get; } = "Gearsetter";

        /*
         * Gearsetter 提供端 (Gearsetter/External/GearsetterIpc.cs) 回傳的 tuple 第三元是 int?,
         * 這裡原本宣告成 byte? —— 是一次窄化。
         * ValueTuple 過 Newtonsoft 靠的是 public 欄位 Item1..ItemN,int? -> byte? 在值不超過 255 時
         * 靜默成功,超過 255 才擲 JsonSerializationException,接著落到 ConvertObject 的替代型別搜尋,
         * 最後擲出 IpcTypeMismatchError —— 而 SafeWrapper.IPCException 只攔 IpcNotReadyError,攔不住它。
         * 目前格位索引都在 255 以內所以踩不到,但那正是「長期看起來正常、只在資料變大那一次炸」的形狀。
         * 改成與提供端逐字相同的 int? 之後型別一致,CallGateChannel 也不必再做 JSON 來回轉換。
         */
        [EzIPC] public Func<byte, List<(uint ItemId, InventoryType? SourceInventory, int? SourceInventorySlot, RaptureGearsetModule.GearsetItemIndex TargetSlot)>> GetRecommendationsForGearset { get; private set; }
    }
}
